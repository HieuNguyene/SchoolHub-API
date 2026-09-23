using MediatR;
using Microsoft.Extensions.Logging;
using SchoolHub.Application.DTOs;
using SchoolHub.Domain.Entities;
using SchoolHub.Application.Interfaces;

namespace SchoolHub.Application.Features.Students.Commands
{
    public class DeleteStudentCommand : IRequest<ApiResponse<bool>>
    {
        public Guid Id { get; set; }
        public DeleteStudentCommand(Guid id)
        {
            Id = id;
        }
    }
    public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, ApiResponse<bool>>
    {
        private readonly ILogger<DeleteStudentCommandHandler> _logger;
        private readonly IStudentRepository _repository;

        public DeleteStudentCommandHandler(ILogger<DeleteStudentCommandHandler> logger, IStudentRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }
        public async Task<ApiResponse<bool>> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Delete Student: Id={Id}", request.Id);
            Student? student = await _repository.GetByIdAsync(request.Id);
            if (student == null)
            {
                _logger.LogWarning("Not found Student: Id={Id}", request.Id);
                return ApiResponse<bool>.NotFound($"Không tìm thấy sinh viên với ID '{request.Id}' để xóa");
            }
            await _repository.DeleteByIdAsync(request.Id);
            return ApiResponse<bool>.Ok(true, "Xóa sinh viên thành công");
        }
    }
}
