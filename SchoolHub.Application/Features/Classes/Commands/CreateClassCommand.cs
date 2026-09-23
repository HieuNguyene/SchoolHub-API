using MediatR;
using SchoolHub.Application.DTOs;
using SchoolHub.Application.Interfaces;
using SchoolHub.Domain.Entities;
using SchoolHub.Application.DTOs.Responses;
using SchoolHub.Application.Common;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace SchoolHub.Application.Features.Classes.Commands
{
    public class CreateClassCommand : IRequest<ApiResponse<ClassResponse>>
    {
        public string ClassId { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
    }

    public class CreateClassCommandHandler(
        IClassRepository repo,
        IMapper mapper,
        IMemoryCache cache,
        ILogger<CreateClassCommandHandler> logger) : IRequestHandler<CreateClassCommand, ApiResponse<ClassResponse>>
    {
        public async Task<ApiResponse<ClassResponse>> Handle(CreateClassCommand request, CancellationToken token)
        {
            var newClass = new Class(request.ClassId, request.ClassName);
            var result = await repo.CreateAsync(newClass);
            
            // Invalidate Cache
            cache.Remove(CacheKeys.ClassesAll);
            logger.LogInformation("[CACHE EVICT] Đã xóa cache '{CacheKey}' do thêm lớp học mới", CacheKeys.ClassesAll);

            var response = mapper.Map<ClassResponse>(result);
            return new ApiResponse<ClassResponse> { Success = true, Data = response };
        }
    }
}
