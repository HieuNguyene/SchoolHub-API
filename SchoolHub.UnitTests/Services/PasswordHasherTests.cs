using FluentAssertions;
using SchoolHub.Infrastructure.Services;
using Xunit;

namespace SchoolHub.UnitTests.Services
{
    public class PasswordHasherTests
    {
        private readonly PasswordHasher _hasher = new();

        [Fact]
        public void HashPassword_ShouldGenerateValidHashFormat()
        {
            // Arrange
            var password = "SecurePassword123!";

            // Act
            var hash = _hasher.HashPassword(password);

            // Assert
            hash.Should().NotBeNullOrWhiteSpace();
            var parts = hash.Split('.');
            parts.Length.Should().Be(3);
            parts[0].Should().Be("10000"); // Iterations
        }

        [Fact]
        public void VerifyPassword_WithCorrectPassword_ShouldReturnTrue()
        {
            // Arrange
            var password = "MySecretPassword@2026";
            var hash = _hasher.HashPassword(password);

            // Act
            var result = _hasher.VerifyPassword(password, hash);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void VerifyPassword_WithIncorrectPassword_ShouldReturnFalse()
        {
            // Arrange
            var password = "CorrectPassword123";
            var wrongPassword = "WrongPassword123";
            var hash = _hasher.HashPassword(password);

            // Act
            var result = _hasher.VerifyPassword(wrongPassword, hash);

            // Assert
            result.Should().BeFalse();
        }
    }
}
