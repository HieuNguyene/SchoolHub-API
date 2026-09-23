using FluentValidation;
using SchoolHub.Application.Features.Auth.Commands.Register;

namespace SchoolHub.Application.Validations
{
    public class RegisterValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Tên đăng nhập không được để trống!")
                .MinimumLength(3).WithMessage("Tên đăng nhập phải có ít nhất 3 ký tự!")
                .MaximumLength(50).WithMessage("Tên đăng nhập không được vượt quá 50 ký tự!");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Mật khẩu không được để trống!")
                .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự!");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email không được để trống!")
                .EmailAddress().WithMessage("Email không đúng định dạng!");
        }
    }
}
