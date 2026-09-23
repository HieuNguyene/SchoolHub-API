using SchoolHub.Domain.Enums;
using System;
namespace SchoolHub.Application.DTOs
{
    public class StudentResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public GenderType Gender { get; set; }
        public string? ClassId { get; set; }
    }
}

