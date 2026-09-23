using MediatR;
using Microsoft.Extensions.Logging;
using SchoolHub.Application.DTOs;
using SchoolHub.Domain.Entities;
using SchoolHub.Domain.Enums;
using SchoolHub.Application.Interfaces;

namespace SchoolHub.Application.Features.Students.Commands
{
    public class UpdateStudentCommand : IRequest<ApiResponse<bool>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public GenderType Gender { get; set; }

        private string? _classId;
        public string? ClassId 
        { 
            get => _classId; 
            set => _classId = string.IsNullOrWhiteSpace(value) ? null : value; 
        }
    }

    public class UpdateStudentHandler : IRequestHandler<UpdateStudentCommand, ApiResponse<bool>>
    {
        private readonly IStudentRepository _repository;
        private readonly ILogger<UpdateStudentHandler> _logger;
        public UpdateStudentHandler(IStudentRepository repository, ILogger<UpdateStudentHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task<ApiResponse<bool>> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Update Student: Id={Id}", request.Id);
            Student? student = await _repository.GetByIdAsync(request.Id);
            if (student == null)
            {
                _logger.LogWarning("Student not found. Id={Id}", request.Id);
                throw new KeyNotFoundException("Sinh viên này không tồn tại");
            }
            student.ChangeName(request.Name);
            student.ChangeDob(request.DateOfBirth);
            student.ChangeGender(request.Gender);
            if (request.ClassId != null)
            {
                student.TransferToClass(request.ClassId);
            }
            await _repository.UpdateAsync(student);
            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Cập nhật thành công!",
                Data = true
            };
        }
    }
}
