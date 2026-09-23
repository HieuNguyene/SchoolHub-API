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
    public class UpdateSubjectValidator : AbstractValidator<UpdateSubjectCommand>
    {
        public UpdateSubjectValidator()
        {
            RuleFor(x => x.SubjectName)
                .NotEmpty().WithMessage("Tên môn học không được để trống!")
                .MaximumLength(100).WithMessage("Tên môn học không được vượt quá 100 ký tự!");
        }
    }
}










