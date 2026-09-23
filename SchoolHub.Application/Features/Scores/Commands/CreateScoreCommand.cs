using MediatR;
using System;
using SchoolHub.Application.DTOs;
using SchoolHub.Application.Interfaces;
using SchoolHub.Domain.Entities;
using SchoolHub.Application.DTOs.Responses;
using AutoMapper;

namespace SchoolHub.Application.Features.Scores.Commands
{
    public class CreateScoreCommand : IRequest<ApiResponse<ScoreResponse>>
    {
        public float Value { get; set; }
        public Guid StudentId { get; set; }
        public string SubjectId { get; set; } = string.Empty;
    }
    public class CreateScoreCommandHandler(IScoreRepository repo, IMapper mapper) : IRequestHandler<CreateScoreCommand, ApiResponse<ScoreResponse>>
    {
        public async Task<ApiResponse<ScoreResponse>> Handle(CreateScoreCommand request, CancellationToken token)
        {
            var newEntity = new Score(Guid.NewGuid(), request.Value, request.StudentId, request.SubjectId);
            var result = await repo.CreateAsync(newEntity);
            var response = mapper.Map<ScoreResponse>(result);
            return ApiResponse<ScoreResponse>.Ok(response, "Nhập điểm thành công");
        }
    }
}
