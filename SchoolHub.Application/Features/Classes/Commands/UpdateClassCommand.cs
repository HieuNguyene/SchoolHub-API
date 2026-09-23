using MediatR;
using SchoolHub.Application.DTOs;
using SchoolHub.Application.Interfaces;
using SchoolHub.Application.Common;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace SchoolHub.Application.Features.Classes.Commands
{
    public class UpdateClassCommand : IRequest<ApiResponse<bool>>
    {
        public string ClassId { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
    }

    public class UpdateClassCommandHandler(
        IClassRepository repo,
        IMemoryCache cache,
        ILogger<UpdateClassCommandHandler> logger) : IRequestHandler<UpdateClassCommand, ApiResponse<bool>>
    {
        public async Task<ApiResponse<bool>> Handle(UpdateClassCommand request, CancellationToken token)
        {
            var existing = await repo.GetByIdAsync(request.ClassId);
            if (existing == null) return ApiResponse<bool>.NotFound($"Không tìm thấy lớp học với mã '{request.ClassId}'");

            existing.UpdateClassName(request.ClassName);
            await repo.UpdateAsync(existing);

            // Invalidate Caches
            cache.Remove(CacheKeys.ClassesAll);
            cache.Remove(CacheKeys.ClassById(request.ClassId));
            logger.LogInformation("[CACHE EVICT] Đã xóa cache '{KeyAll}' và '{KeyById}' do cập nhật lớp học",
                CacheKeys.ClassesAll, CacheKeys.ClassById(request.ClassId));

            return ApiResponse<bool>.Ok(true, "Cập nhật lớp học thành công");
        }
    }
}
