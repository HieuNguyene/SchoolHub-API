using FluentAssertions;
using SchoolHub.Domain.Entities;
using SchoolHub.Domain.Enums;
using Xunit;

namespace SchoolHub.UnitTests.Domain
{
    public class StudentEntityTests
    {
        [Fact]
        public void ChangeDob_WhenGivenFutureDate_ShouldThrowArgumentException()
        {
            var student = new Student(Guid.NewGuid(), "Nguyen Van A", DateTime.UtcNow.AddYears(-16), GenderType.Male, "C10A");
            var futureDate = DateTime.UtcNow.AddDays(5);

            var action = () => student.ChangeDob(futureDate);

            action.Should().Throw<ArgumentException>()
                .WithMessage("*Ngày sinh không thể lớn hơn ngày hiện tại*");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void ChangeName_WhenGivenInvalidName_ShouldThrowArgumentException(string? invalidName)
        {
            var student = new Student(Guid.NewGuid(), "Nguyen Van A", DateTime.UtcNow.AddYears(-16), GenderType.Male, "C10A");

            var action = () => student.ChangeName(invalidName!);

            action.Should().Throw<ArgumentException>();
        }
    }
}
