using MediatR;
using System;
using SchoolHub.Application.DTOs;
using SchoolHub.Application.Interfaces;

namespace SchoolHub.Application.Features.Scores.Commands
{
    public class UpdateScoreCommand : IRequest<ApiResponse<bool>>
    {
        public Guid Id { get; set; }
        public float Value { get; set; }
    }
    public class UpdateScoreCommandHandler : IRequestHandler<UpdateScoreCommand, ApiResponse<bool>>
    {
        private readonly IScoreRepository _repo;
        public UpdateScoreCommandHandler(IScoreRepository repo) => _repo = repo;
        public async Task<ApiResponse<bool>> Handle(UpdateScoreCommand request, CancellationToken token)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
            {
                return ApiResponse<bool>.NotFound($"Không tìm thấy bản ghi điểm số với ID '{request.Id}'");
            }

            existing.UpdateValue(request.Value);
            await _repo.UpdateAsync(existing);
            return ApiResponse<bool>.Ok(true, "Cập nhật điểm thành công");
        }
    }
}
