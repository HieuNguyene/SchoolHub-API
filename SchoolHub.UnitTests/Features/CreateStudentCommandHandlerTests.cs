using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SchoolHub.Application.DTOs;
using SchoolHub.Application.Features.Students.Commands;
using SchoolHub.Application.Interfaces;
using SchoolHub.Domain.Entities;
using SchoolHub.Domain.Enums;
using Xunit;

namespace SchoolHub.UnitTests.Features
{
    public class CreateStudentCommandHandlerTests
    {
        private readonly Mock<IStudentRepository> _studentRepoMock = new();
        private readonly Mock<ILogger<CreateStudentCommandHandler>> _loggerMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        [Fact]
        public async Task Handle_WithValidCommand_ShouldSaveStudentAndReturnSuccess()
        {
            // Arrange
            var command = new CreateStudentCommand
            {
                Name = "Le Van B",
                DateOfBirth = new DateTime(2005, 5, 20),
                Gender = GenderType.Male,
                ClassId = "C10A"
            };

            var student = new Student(Guid.NewGuid(), command.Name, command.DateOfBirth, command.Gender, command.ClassId);
            var studentResponse = new StudentResponse
            {
                Id = student.Id,
                Name = student.Name,
                DateOfBirth = student.DateOfBirth,
                Gender = student.Gender,
                ClassId = student.ClassId
            };

            _studentRepoMock
                .Setup(r => r.CreateStudentAsync(It.IsAny<Student>()))
                .ReturnsAsync(student);

            _mapperMock
                .Setup(m => m.Map<StudentResponse>(It.IsAny<Student>()))
                .Returns(studentResponse);

            var handler = new CreateStudentCommandHandler(
                _studentRepoMock.Object,
                _loggerMock.Object,
                _mapperMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.Name.Should().Be("Le Van B");
            _studentRepoMock.Verify(r => r.CreateStudentAsync(It.IsAny<Student>()), Times.Once);
        }
    }
}
