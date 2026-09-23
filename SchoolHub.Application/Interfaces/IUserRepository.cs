using SchoolHub.Domain.Entities;

namespace SchoolHub.Application.Interfaces
{
    public interface IUserRepository
    {
        // Thao tác với User
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByIdAsync(Guid id);
        Task<bool> ExistsByUsernameAsync(string username);
        Task<User> CreateUserAsync(User user);

        // Thao tác với RefreshToken
        Task SaveRefreshTokenAsync(RefreshToken token);
        Task<RefreshToken?> GetRefreshTokenAsync(string token);
        Task UpdateRefreshTokenAsync(RefreshToken token);
        Task RevokeRefreshTokenAsync(RefreshToken token);
        Task RevokeAllUserTokensAsync(string userId);
    }
}