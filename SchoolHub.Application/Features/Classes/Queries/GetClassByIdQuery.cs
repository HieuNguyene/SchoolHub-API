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
    public class GetClassByIdQuery : IRequest<ApiResponse<ClassResponse>>
    {
        public string ClassId { get; set; }
        public GetClassByIdQuery(string id) => ClassId = id;
    }

    public class GetClassByIdQueryHandler(
        IClassRepository repo,
        IMapper mapper,
        IMemoryCache cache,
        ILogger<GetClassByIdQueryHandler> logger) : IRequestHandler<GetClassByIdQuery, ApiResponse<ClassResponse>>
    {
        public async Task<ApiResponse<ClassResponse>> Handle(GetClassByIdQuery request, CancellationToken token)
        {
            var cacheKey = CacheKeys.ClassById(request.ClassId);
            if (cache.TryGetValue(cacheKey, out ClassResponse? cachedClass) && cachedClass != null)
            {
                logger.LogInformation("[CACHE HIT] Lấy chi tiết lớp học từ In-Memory Cache (Key: {CacheKey})", cacheKey);
                return ApiResponse<ClassResponse>.Ok(cachedClass);
            }

            logger.LogInformation("[CACHE MISS] Chưa có cache cho Key '{CacheKey}'. Đang truy vấn từ Database...", cacheKey);
            var result = await repo.GetByIdAsync(request.ClassId);
            if (result == null) return ApiResponse<ClassResponse>.NotFound($"Không tìm thấy lớp học với mã '{request.ClassId}'");

            var response = mapper.Map<ClassResponse>(result);
            cache.Set(cacheKey, response, CacheKeys.DefaultOptions);
            logger.LogInformation("[CACHE SET] Đã lưu chi tiết lớp học vào In-Memory Cache (Key: {CacheKey})", cacheKey);

            return ApiResponse<ClassResponse>.Ok(response);
        }
    }
}
