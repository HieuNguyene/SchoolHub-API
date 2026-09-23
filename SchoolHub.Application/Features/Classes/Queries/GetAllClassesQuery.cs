using MediatR;
using SchoolHub.Application.DTOs;
using SchoolHub.Application.Interfaces;
using SchoolHub.Domain.Entities;
using SchoolHub.Application.DTOs.Responses;
using SchoolHub.Application.Common;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace SchoolHub.Application.Features.Classes.Queries
{
    public class GetAllClassesQuery : IRequest<ApiResponse<List<ClassResponse>>> { }

    public class GetAllClassesQueryHandler(
        IClassRepository repo,
        IMapper mapper,
        IMemoryCache cache,
        ILogger<GetAllClassesQueryHandler> logger) : IRequestHandler<GetAllClassesQuery, ApiResponse<List<ClassResponse>>>
    {
        public async Task<ApiResponse<List<ClassResponse>>> Handle(GetAllClassesQuery request, CancellationToken token)
        {
            if (cache.TryGetValue(CacheKeys.ClassesAll, out List<ClassResponse>? cachedClasses) && cachedClasses != null)
            {
                logger.LogInformation("[CACHE HIT] Lấy danh sách lớp học từ In-Memory Cache (Key: {CacheKey})", CacheKeys.ClassesAll);
                return new ApiResponse<List<ClassResponse>> { Success = true, Data = cachedClasses };
            }

            logger.LogInformation("[CACHE MISS] Chưa có cache cho Key '{CacheKey}'. Đang truy vấn từ Database...", CacheKeys.ClassesAll);
            var result = await repo.GetAllClassAsync();
            var response = mapper.Map<List<ClassResponse>>(result);

            cache.Set(CacheKeys.ClassesAll, response, CacheKeys.DefaultOptions);
            logger.LogInformation("[CACHE SET] Đã lưu danh sách lớp học vào In-Memory Cache (Key: {CacheKey})", CacheKeys.ClassesAll);

            return new ApiResponse<List<ClassResponse>> { Success = true, Data = response };
        }
    }
}
