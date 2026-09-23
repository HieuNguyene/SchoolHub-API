using System.Security.Claims;
using MediatR;
using Microsoft.IdentityModel.JsonWebTokens;
using SchoolHub.Application.DTOs;
using SchoolHub.Application.DTOs.Responses;
using SchoolHub.Application.Interfaces;

namespace SchoolHub.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<ApiResponse<LoginResponse>>
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class RefreshTokenCommandHandler(IUserRepository userRepo, ITokenService tokenService)
        : IRequestHandler<RefreshTokenCommand, ApiResponse<LoginResponse>>
    {
        public async Task<ApiResponse<LoginResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            // 1. Giải mã Access Token cũ
            var principal = tokenService.GetClaimsPrincipalFromExpiredToken(request.AccessToken);
            if (principal == null)
            {
                return ApiResponse<LoginResponse>.Fail("Access Token không hợp lệ!", 401);
            }

            var jwtId = principal.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? principal.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            {
                return ApiResponse<LoginResponse>.Fail("Định danh người dùng không hợp lệ!", 401);
            }

            // Kiểm tra Người dùng còn tồn tại trong DB không
            var user = await userRepo.GetByIdAsync(userGuid);
            if (user == null)
            {
                return ApiResponse<LoginResponse>.Fail("Người dùng không còn tồn tại trong hệ thống!", 401);
            }

            // 2. Tìm Refresh Token trong Database qua Repository
            var storedToken = await userRepo.GetRefreshTokenAsync(request.RefreshToken);
            if (storedToken == null)
            {
                return ApiResponse<LoginResponse>.Fail("Refresh Token không tồn tại!", 404);
            }

            // 3. Kiểm tra các điều kiện an toàn & Chống Replay Attack (RFC 6749)
            if (storedToken.IsUsed)
            {
                // Phát hiện token đã dùng bị dùng lại: Lập tức thu hồi toàn bộ phiên đăng nhập của User
                await userRepo.RevokeAllUserTokensAsync(userId);
                return ApiResponse<LoginResponse>.Fail("Phát hiện Token đã qua sử dụng! Tất cả các phiên làm việc đã bị hủy vì lý do bảo mật.", 401);
            }

            if (storedToken.IsRevoked)
            {
                return ApiResponse<LoginResponse>.Fail("Refresh Token đã bị thu hồi!", 401);
            }

            if (storedToken.JwtId != jwtId)
            {
                return ApiResponse<LoginResponse>.Fail("Token không khớp cặp!", 400);
            }

            if (storedToken.ExpiryDate < DateTime.UtcNow)
            {
                return ApiResponse<LoginResponse>.Fail("Refresh Token đã hết hạn!", 401);
            }

            // 4. Đánh dấu token cũ ĐÃ DÙNG
            storedToken.IsUsed = true;
            await userRepo.UpdateRefreshTokenAsync(storedToken);

            // 5. Cấp CẶP TOKEN MỚI (Lấy Role và Username mới nhất từ Database)
            var newAccessToken = tokenService.GenerateToken(user.Id.ToString(), user.Username, user.Role, out string newJwtId);
            var newRefreshToken = tokenService.GenerateRefreshToken();

            var newRefreshTokenEntity = new SchoolHub.Domain.Entities.RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = newRefreshToken,
                JwtId = newJwtId,
                UserId = user.Id.ToString(),
                CreatedAt = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                IsUsed = false,
                IsRevoked = false
            };
            await userRepo.SaveRefreshTokenAsync(newRefreshTokenEntity);

            return ApiResponse<LoginResponse>.Ok(new LoginResponse
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
                Username = user.Username,
                Role = user.Role
            }, "Làm mới Token thành công!");
        }
    }
}