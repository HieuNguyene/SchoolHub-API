using MediatR;
using SchoolHub.Application.DTOs;
using SchoolHub.Application.Interfaces;
using SchoolHub.Domain.Entities;

namespace SchoolHub.Application.Features.Auth.Commands.Register
{
    public class RegisterCommand : IRequest<ApiResponse<Guid>>
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "User"; // "Admin" hoặc "User"
    }

    public class RegisterCommandHandler(IUserRepository userRepo, IPasswordHasher passwordHasher) 
        : IRequestHandler<RegisterCommand, ApiResponse<Guid>>
    {
        public async Task<ApiResponse<Guid>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // 1. Kiểm tra username đã tồn tại chưa
            if (await userRepo.ExistsByUsernameAsync(request.Username))
            {
                return ApiResponse<Guid>.Fail("Tên đăng nhập đã tồn tại!", 400);
            }

            // 2. Băm mật khẩu (Hash Password)
            var passwordHash = passwordHasher.HashPassword(request.Password);

            // 3. Tạo Entity User và lưu vào Database (Luôn gán Role mặc định là User để bảo mật)
            const string defaultRole = "User";
            var user = new User(Guid.NewGuid(), request.Username, passwordHash, request.Email, defaultRole);
            await userRepo.CreateUserAsync(user);

            return ApiResponse<Guid>.Ok(user.Id, "Đăng ký tài khoản thành công!");
        }
    }
}