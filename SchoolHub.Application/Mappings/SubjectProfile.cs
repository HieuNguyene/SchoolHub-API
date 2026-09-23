using AutoMapper;
using SchoolHub.Application.DTOs.Responses;
using SchoolHub.Application.Features.Subjects.Commands;
using SchoolHub.Domain.Entities;

namespace SchoolHub.Application.Mappings
{
    public class SubjectProfile : Profile
    {
        public SubjectProfile()
        {
            // Chiều ra: Entity -> DTO
            CreateMap<Subject, SubjectResponse>();

            // Chiều vào: Command -> Entity
            CreateMap<CreateSubjectCommand, Subject>()
                .ForMember(dest => dest.Scores, opt => opt.Ignore());
        }
    }
}
