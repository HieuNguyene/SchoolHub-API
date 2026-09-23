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
    public class GetAllSubjectsQuery : IRequest<ApiResponse<List<SubjectResponse>>> { }

    public class GetAllSubjectsQueryHandler(
        ISubjectRepository repo,
        IMapper mapper,
        IMemoryCache cache,
        ILogger<GetAllSubjectsQueryHandler> logger) : IRequestHandler<GetAllSubjectsQuery, ApiResponse<List<SubjectResponse>>>
    {
        public async Task<ApiResponse<List<SubjectResponse>>> Handle(GetAllSubjectsQuery request, CancellationToken token)
        {
            if (cache.TryGetValue(CacheKeys.SubjectsAll, out List<SubjectResponse>? cachedSubjects) && cachedSubjects != null)
            {
                logger.LogInformation("[CACHE HIT] Lấy danh sách môn học từ In-Memory Cache (Key: {CacheKey})", CacheKeys.SubjectsAll);
                return new ApiResponse<List<SubjectResponse>> { Success = true, Data = cachedSubjects };
            }

            logger.LogInformation("[CACHE MISS] Chưa có cache cho Key '{CacheKey}'. Đang truy vấn từ Database...", CacheKeys.SubjectsAll);
            var result = await repo.GetAllAsync();
            var response = mapper.Map<List<SubjectResponse>>(result);

            cache.Set(CacheKeys.SubjectsAll, response, CacheKeys.DefaultOptions);
            logger.LogInformation("[CACHE SET] Đã lưu danh sách môn học vào In-Memory Cache (Key: {CacheKey})", CacheKeys.SubjectsAll);

            return new ApiResponse<List<SubjectResponse>> { Success = true, Data = response };
        }
    }
}
