using SchoolHub.Application.DTOs;
using SchoolHub.Application.Features.Classes.Commands;
using SchoolHub.Application.Features.Students.Commands;
using SchoolHub.Application.Features.Subjects.Commands;
using SchoolHub.Application.Features.Scores.Commands;
using SchoolHub.Application.Features.Students.Queries;
using SchoolHub.Application.Validations;
using SchoolHub.Application.Interfaces;
using FluentValidation;

namespace SchoolHub.Application.Validations
{
    public class CreateClassValidator : AbstractValidator<CreateClassCommand>
    {
        public CreateClassValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.ClassId)
                .NotEmpty().WithMessage("Mã lớp (ClassId) không được để trống!")
                .MaximumLength(50).WithMessage("Mã lớp không được vượt quá 50 ký tự.");

            RuleFor(x => x.ClassName)
                .NotEmpty().WithMessage("Tên lớp (ClassName) không được để trống!")
                .MinimumLength(3).WithMessage("Tên lớp nên có ít nhất 3 ký tự.")
                .MaximumLength(50).WithMessage("Tên lớp không được vượt quá 50 ký tự.");
        }
    }
}










