using AutoMapper;
using SchoolHub.Application.DTOs.Responses;
using SchoolHub.Application.Features.Scores.Commands;
using SchoolHub.Domain.Entities;

namespace SchoolHub.Application.Mappings
{
    public class ScoreProfile : Profile
    {
        public ScoreProfile()
        {
            // Chiều ra: Entity -> DTO
            CreateMap<Score, ScoreResponse>();

            // Chiều vào: Command -> Entity
            CreateMap<CreateScoreCommand, Score>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Student, opt => opt.Ignore())
                .ForMember(dest => dest.Subject, opt => opt.Ignore());
        }
    }
}
