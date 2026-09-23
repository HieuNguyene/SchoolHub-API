using AutoMapper;
using SchoolHub.Application.DTOs.Responses;
using SchoolHub.Application.Features.Classes.Commands;
using SchoolHub.Domain.Entities;

namespace SchoolHub.Application.Mappings
{
    public class ClassProfile : Profile
    {
        public ClassProfile(){
            // Đầu ra
            CreateMap<Class,ClassResponse>();
            // Đầu vào
            CreateMap<CreateClassCommand,Class>()
                .ForMember(dest => dest.Students, opt => opt.Ignore());
        }
    }
}