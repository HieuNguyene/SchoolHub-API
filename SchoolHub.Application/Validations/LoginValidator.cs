using FluentValidation;
using SchoolHub.Application.Features.Auth.Commands.Login;

namespace SchoolHub.Application.Validations
{
    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Tên đăng nhập không được để trống!");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Mật khẩu không được để trống!");
        }
    }
}
