using MediatR;
using SchoolHub.Application.DTOs;
using SchoolHub.Application.Interfaces;
using SchoolHub.Application.Common;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace SchoolHub.Application.Features.Subjects.Commands
{
    public class UpdateSubjectCommand : IRequest<ApiResponse<bool>>
    {
        public string SubjectId { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
    }

    public class UpdateSubjectCommandHandler(
        ISubjectRepository repo,
        IMemoryCache cache,
        ILogger<UpdateSubjectCommandHandler> logger) : IRequestHandler<UpdateSubjectCommand, ApiResponse<bool>>
    {
        public async Task<ApiResponse<bool>> Handle(UpdateSubjectCommand request, CancellationToken token)
        {
            var existing = await repo.GetByIdAsync(request.SubjectId);
            if (existing == null) return ApiResponse<bool>.NotFound($"Không tìm thấy môn học với mã '{request.SubjectId}'");

            existing.UpdateSubjectName(request.SubjectName);
            await repo.UpdateAsync(existing);

            // Invalidate Caches
            cache.Remove(CacheKeys.SubjectsAll);
            cache.Remove(CacheKeys.SubjectById(request.SubjectId));
            logger.LogInformation("[CACHE EVICT] Đã xóa cache '{KeyAll}' và '{KeyById}' do cập nhật môn học",
                CacheKeys.SubjectsAll, CacheKeys.SubjectById(request.SubjectId));

            return ApiResponse<bool>.Ok(true, "Cập nhật môn học thành công");
        }
    }
}
