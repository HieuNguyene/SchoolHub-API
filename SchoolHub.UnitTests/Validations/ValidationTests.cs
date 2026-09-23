using FluentAssertions;
using SchoolHub.Application.Features.Auth.Commands.Register;
using SchoolHub.Application.Features.Classes.Commands;
using SchoolHub.Application.Features.Students.Commands;
using SchoolHub.Application.Validations;
using SchoolHub.Domain.Enums;
using Xunit;

namespace SchoolHub.UnitTests.Validations
{
    public class ValidationTests
    {
        [Fact]
        public void RegisterValidator_WithValidData_ShouldNotHaveErrors()
        {
            var validator = new RegisterValidator();
            var command = new RegisterCommand
            {
                Username = "validuser",
                Password = "ValidPassword123!",
                Email = "test@domain.com"
            };

            var result = validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("", "ValidPassword123!", "test@domain.com")] // Empty Username
        [InlineData("ab", "ValidPassword123!", "test@domain.com")] // Short Username
        [InlineData("validuser", "123", "test@domain.com")] // Short Password
        [InlineData("validuser", "ValidPassword123!", "not-an-email")] // Invalid Email
        public void RegisterValidator_WithInvalidData_ShouldHaveErrors(string username, string password, string email)
        {
            var validator = new RegisterValidator();
            var command = new RegisterCommand
            {
                Username = username,
                Password = password,
                Email = email
            };

            var result = validator.Validate(command);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void CreateClassValidator_WhenClassNameExceeds50Chars_ShouldFail()
        {
            var validator = new CreateClassValidator();
            var command = new CreateClassCommand
            {
                ClassId = "C10A",
                ClassName = new string('A', 51) // Exceeds 50 chars
            };

            var result = validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateClassCommand.ClassName));
        }

        [Fact]
        public void CreateStudentValidator_WhenNameExceeds100Chars_ShouldFail()
        {
            var validator = new CreateStudentValidator();
            var command = new CreateStudentCommand
            {
                Name = new string('X', 101), // Exceeds 100 chars
                DateOfBirth = DateTime.UtcNow.AddYears(-15),
                Gender = GenderType.Male
            };

            var result = validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateStudentCommand.Name));
        }

        [Fact]
        public void CreateStudentValidator_WhenDobIsInFuture_ShouldFail()
        {
            var validator = new CreateStudentValidator();
            var command = new CreateStudentCommand
            {
                Name = "Nguyen Van A",
                DateOfBirth = DateTime.UtcNow.AddDays(1), // Future DOB
                Gender = GenderType.Male
            };

            var result = validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateStudentCommand.DateOfBirth));
        }
    }
}
