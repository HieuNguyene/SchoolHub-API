using MediatR;
using SchoolHub.Application.DTOs;
using SchoolHub.Application.Interfaces;
using SchoolHub.Application.Common;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace SchoolHub.Application.Features.Classes.Commands
{
    public class DeleteClassCommand : IRequest<ApiResponse<bool>>
    {
        public string ClassId { get; set; }
        public DeleteClassCommand(string id) => ClassId = id;
    }

    public class DeleteClassCommandHandler(
        IClassRepository repo,
        IMemoryCache cache,
        ILogger<DeleteClassCommandHandler> logger) : IRequestHandler<DeleteClassCommand, ApiResponse<bool>>
    {
        public async Task<ApiResponse<bool>> Handle(DeleteClassCommand request, CancellationToken token)
        {
            var result = await repo.DeleteByIdAsync(request.ClassId);
            if (result)
            {
                // Invalidate Caches
                cache.Remove(CacheKeys.ClassesAll);
                cache.Remove(CacheKeys.ClassById(request.ClassId));
                logger.LogInformation("[CACHE EVICT] Đã xóa cache '{KeyAll}' và '{KeyById}' do xóa lớp học",
                    CacheKeys.ClassesAll, CacheKeys.ClassById(request.ClassId));
                return ApiResponse<bool>.Ok(true, "Xóa lớp học thành công");
            }
            return ApiResponse<bool>.NotFound($"Không tìm thấy lớp học với mã '{request.ClassId}' để xóa");
        }
    }
}
