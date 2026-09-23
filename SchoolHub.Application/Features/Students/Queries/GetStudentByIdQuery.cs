using MediatR;
using Microsoft.Extensions.Logging;
using SchoolHub.Application.DTOs;
using SchoolHub.Domain.Entities;
using SchoolHub.Application.Interfaces;
using AutoMapper;

namespace SchoolHub.Application.Features.Students.Queries
{
    public class GetStudentByIdQuery : IRequest<ApiResponse<StudentResponse>>
    {
        public Guid Id { get; set; }
        public GetStudentByIdQuery(Guid id)
        {
            Id = id;
        }
    }
    public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, ApiResponse<StudentResponse>>
    {
        private readonly ILogger<GetStudentByIdQueryHandler> _logger;
        private readonly IStudentRepository _repository;
        private readonly IMapper _mapper;

        public GetStudentByIdQueryHandler(ILogger<GetStudentByIdQueryHandler> logger, IStudentRepository repository,IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ApiResponse<StudentResponse>> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get student: id={Id}", request.Id);
            Student? student = await _repository.GetByIdAsync(request.Id);
            if (student == null)
            {
                _logger.LogWarning("Student not found. Id={Id}", request.Id);
                throw new KeyNotFoundException("Student not found");
            }
            var data = _mapper.Map<StudentResponse>(student);
            return new ApiResponse<StudentResponse>
            {
                Success = true,
                Message = "Success",
                Data = data
            };
        }
    }
}
