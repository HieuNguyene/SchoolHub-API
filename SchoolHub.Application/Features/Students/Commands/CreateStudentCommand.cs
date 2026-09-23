using MediatR;
using SchoolHub.Application.DTOs;
using SchoolHub.Application.Interfaces;
using SchoolHub.Domain.Entities;
using SchoolHub.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AutoMapper;
namespace SchoolHub.Application.Features.Students.Commands
{
    // Dữ liệu đầu vào
    public class CreateStudentCommand : IRequest<ApiResponse<StudentResponse>>
    {
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
    // Nơi xử lí logic
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, ApiResponse<StudentResponse>>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ILogger<CreateStudentCommandHandler> _logger;
        private readonly IMapper _mapper;   
        public CreateStudentCommandHandler(IStudentRepository studentRepository, ILogger<CreateStudentCommandHandler> logger,IMapper mapper)
        {
            _studentRepository = studentRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<ApiResponse<StudentResponse>> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Đang tạo mới một sinh viên");
            var student = new Student(Guid.NewGuid(), request.Name, request.DateOfBirth, request.Gender, request.ClassId);

            var createdStudent = await _studentRepository.CreateStudentAsync(student);
            _logger.LogInformation("Đã tạo thành công một sinh viên");
            var responseData = _mapper.Map<StudentResponse>(createdStudent);
            return new ApiResponse<StudentResponse> { Success = true, Data = responseData, Message = "Tạo sinh viên thành công" };
        }
    }
}
