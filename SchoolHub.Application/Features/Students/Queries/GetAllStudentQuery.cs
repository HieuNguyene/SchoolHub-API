using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SchoolHub.Application.DTOs;
using SchoolHub.Application.Interfaces;

namespace SchoolHub.Application.Features.Students.Queries
{
    public class GetAllStudentQuery : IRequest<ApiResponse<List<StudentResponse>>>
    {

    }
    public class GetAllStudentQueryHandler : IRequestHandler<GetAllStudentQuery, ApiResponse<List<StudentResponse>>>
    {
        private readonly ILogger<GetAllStudentQueryHandler> _logger;
        private readonly IStudentRepository _repository;
        private readonly IMapper _mapper;

        public GetAllStudentQueryHandler(ILogger<GetAllStudentQueryHandler> logger, IStudentRepository repository,IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;   
        }
        public async Task<ApiResponse<List<StudentResponse>>> Handle(GetAllStudentQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Lấy toàn bộ danh sách sinh viên");
            var query = await _repository.GetAllStudentAsync();
            var data = _mapper.Map<List<StudentResponse>>(query);
            return new ApiResponse<List<StudentResponse>>
            {
                Success = true,
                Message = "Đã lấy thành công danh sách sinh viên",
                Data = data
            };
        }
    }
}
