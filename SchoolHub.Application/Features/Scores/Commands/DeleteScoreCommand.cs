using MediatR;
using System;
using SchoolHub.Application.DTOs;
using SchoolHub.Application.Interfaces;

namespace SchoolHub.Application.Features.Scores.Commands
{
    public class DeleteScoreCommand : IRequest<ApiResponse<bool>>
    {
        public Guid Id { get; set; }
        public DeleteScoreCommand(Guid id) => Id = id;
    }
    public class DeleteScoreCommandHandler : IRequestHandler<DeleteScoreCommand, ApiResponse<bool>>
    {
        private readonly IScoreRepository _repo;
        public DeleteScoreCommandHandler(IScoreRepository repo) => _repo = repo;
        public async Task<ApiResponse<bool>> Handle(DeleteScoreCommand request, CancellationToken token)
        {
            var result = await _repo.DeleteAsync(request.Id);
            if (result)
            {
                return ApiResponse<bool>.Ok(true, "Xóa điểm số thành công");
            }
            return ApiResponse<bool>.NotFound($"Không tìm thấy điểm số với ID '{request.Id}' để xóa");
        }
    }
}
