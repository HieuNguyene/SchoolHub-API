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
    public class StudentQueryValidator : AbstractValidator<GetStudentByKeyWordQuery>
    {
        public StudentQueryValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            When(x => !string.IsNullOrEmpty(x.Keyword), () =>
            {
                RuleFor(x => x.Keyword)
                    .NotEmpty().WithMessage("Từ khóa tìm kiếm (Keyword) không được để trống nếu có truyền!")
                    .MinimumLength(2).WithMessage("Từ khóa tìm kiếm nên có ít nhất 2 ký tự.");
            });

            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1).WithMessage("Trang (Page) phải lớn hơn hoặc bằng 1.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Số lượng trên mỗi trang (PageSize) phải lớn hơn 0.")
                .LessThanOrEqualTo(100).WithMessage("Số lượng trên mỗi trang (PageSize) không được vượt quá 100.");
        }
    }
}









