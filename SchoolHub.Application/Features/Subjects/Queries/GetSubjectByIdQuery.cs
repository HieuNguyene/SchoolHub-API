using MediatR;
using SchoolHub.Application.DTOs;
using SchoolHub.Application.Interfaces;
using SchoolHub.Domain.Entities;
using SchoolHub.Application.DTOs.Responses;
using SchoolHub.Application.Common;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace SchoolHub.Application.Features.Subjects.Queries
{
    public class GetSubjectByIdQuery : IRequest<ApiResponse<SubjectResponse>>
    {
        public string SubjectId { get; set; }
        public GetSubjectByIdQuery(string id) => SubjectId = id;
    }

    public class GetSubjectByIdQueryHandler(
        ISubjectRepository repo,
        IMapper mapper,
        IMemoryCache cache,
        ILogger<GetSubjectByIdQueryHandler> logger) : IRequestHandler<GetSubjectByIdQuery, ApiResponse<SubjectResponse>>
    {
        public async Task<ApiResponse<SubjectResponse>> Handle(GetSubjectByIdQuery request, CancellationToken token)
        {
            var cacheKey = CacheKeys.SubjectById(request.SubjectId);
            if (cache.TryGetValue(cacheKey, out SubjectResponse? cachedSubject) && cachedSubject != null)
            {
                logger.LogInformation("[CACHE HIT] Lấy chi tiết môn học từ In-Memory Cache (Key: {CacheKey})", cacheKey);
                return ApiResponse<SubjectResponse>.Ok(cachedSubject);
            }

            logger.LogInformation("[CACHE MISS] Chưa có cache cho Key '{CacheKey}'. Đang truy vấn từ Database...", cacheKey);
            var result = await repo.GetByIdAsync(request.SubjectId);
            if (result == null) return ApiResponse<SubjectResponse>.NotFound($"Không tìm thấy môn học với mã '{request.SubjectId}'");

            var response = mapper.Map<SubjectResponse>(result);
            cache.Set(cacheKey, response, CacheKeys.DefaultOptions);
            logger.LogInformation("[CACHE SET] Đã lưu chi tiết môn học vào In-Memory Cache (Key: {CacheKey})", cacheKey);

            return ApiResponse<SubjectResponse>.Ok(response);
        }
    }
}
