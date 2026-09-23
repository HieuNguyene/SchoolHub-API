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
    public class UpdateStudentValidator : AbstractValidator<UpdateStudentCommand>
    {
        public UpdateStudentValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("Name is required!").MinimumLength(3).WithMessage("Tên tối thiểu phải có 3 ký tự");
            RuleFor(x => x.DateOfBirth).LessThan(DateTime.Now).WithMessage("Ngày sinh phải nhỏ hơn hiện tại");
            RuleFor(x => x.Gender).IsInEnum().WithMessage("Giới tính không hợp lệ!");

            When(x => !string.IsNullOrEmpty(x.ClassId), () =>
            {
                RuleFor(x => x.ClassId)
                    .NotEmpty().WithMessage("ClassId không được để trống nếu có truyền!");
            });
        }
    }
}










