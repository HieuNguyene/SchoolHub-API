using MediatR;
using SchoolHub.Application.DTOs;
using SchoolHub.Application.DTOs.Responses;
using SchoolHub.Application.Interfaces;
using SchoolHub.Domain.Entities;

namespace SchoolHub.Application.Features.Auth.Commands.Login
{
    public class LoginCommand : IRequest<ApiResponse<LoginResponse>>
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginCommandHandler(
        IUserRepository userRepo, 
        IPasswordHasher passwordHasher, 
        ITokenService tokenService) 
        : IRequestHandler<LoginCommand, ApiResponse<LoginResponse>>
    {
        public async Task<ApiResponse<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // 1. Tìm User trong Database
            var user = await userRepo.GetByUsernameAsync(request.Username);
            if (user == null)
            {
                return new ApiResponse<LoginResponse> { Success = false, Message = "Tài khoản hoặc mật khẩu không chính xác!" };
            }

            // 2. Kiểm tra mật khẩu đã băm
            if (!passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                return new ApiResponse<LoginResponse> { Success = false, Message = "Tài khoản hoặc mật khẩu không chính xác!" };
            }

            // 3. Sinh Access Token và nhận mã jwtId
            var accessToken = tokenService.GenerateToken(user.Id.ToString(), user.Username, user.Role, out string jwtId);

            // 4. Sinh Refresh Token ngẫu nhiên
            var refreshToken = tokenService.GenerateRefreshToken();

            // 5. Lưu Refresh Token vào Database
            var refreshTokenEntity = new SchoolHub.Domain.Entities.RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = refreshToken,
                JwtId = jwtId,
                UserId = user.Id.ToString(),
                CreatedAt = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(7), // Hạn 7 ngày
                IsUsed = false,
                IsRevoked = false
            };
            await userRepo.SaveRefreshTokenAsync(refreshTokenEntity);

            // 6. Trả kết quả
            return new ApiResponse<LoginResponse>
            {
                Success = true,
                Message = "Đăng nhập thành công!",
                Data = new LoginResponse
                {
                    Token = accessToken,
                    RefreshToken = refreshToken,
                    Username = user.Username,
                    Role = user.Role
                }
            };
        }
    }
}