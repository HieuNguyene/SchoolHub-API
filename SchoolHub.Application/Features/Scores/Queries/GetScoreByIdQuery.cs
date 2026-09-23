using MediatR;
using System;
using SchoolHub.Application.DTOs;
using SchoolHub.Application.Interfaces;
using SchoolHub.Application.DTOs.Responses;
using AutoMapper;

namespace SchoolHub.Application.Features.Scores.Queries
{
    public class GetScoreByIdQuery : IRequest<ApiResponse<ScoreResponse>>
    {
        public Guid Id { get; set; }
        public GetScoreByIdQuery(Guid id) => Id = id;
    }
    public class GetScoreByIdQueryHandler(IScoreRepository repo, IMapper mapper) : IRequestHandler<GetScoreByIdQuery, ApiResponse<ScoreResponse>>
    {
        public async Task<ApiResponse<ScoreResponse>> Handle(GetScoreByIdQuery request, CancellationToken token)
        {
            var result = await repo.GetByIdAsync(request.Id);
            if (result == null)
            {
                return ApiResponse<ScoreResponse>.NotFound($"Không tìm thấy điểm số với ID '{request.Id}'");
            }
            var response = mapper.Map<ScoreResponse>(result);
            return ApiResponse<ScoreResponse>.Ok(response);
        }
    }
}
