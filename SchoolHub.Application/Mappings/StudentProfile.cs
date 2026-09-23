using AutoMapper;
using SchoolHub.Application.DTOs;
using SchoolHub.Application.Features.Students.Commands;
using SchoolHub.Domain.Entities;

namespace SchoolHub.Application.Mappings
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            // Chiều ra: Entity -> DTO
            CreateMap<Student, StudentResponse>();

            // Chiều vào: Command -> Entity
            CreateMap<CreateStudentCommand, Student>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Class, opt => opt.Ignore())
                .ForMember(dest => dest.Scores, opt => opt.Ignore());
        }
    }
}