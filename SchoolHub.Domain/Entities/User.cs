namespace SchoolHub.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Role { get; private set; } = "User"; // "Admin" hoặc "User"
        public DateTime CreatedAt { get; private set; }

        protected User() { }

        public User(Guid id, string username, string passwordHash, string email, string role)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username không được để trống");
            if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentException("Password không được để trống");

            Id = id;
            Username = username;
            PasswordHash = passwordHash;
            Email = email;
            Role = string.IsNullOrWhiteSpace(role) ? "User" : role;
            CreatedAt = DateTime.UtcNow;
        }

        public void ChangePassword(string newPasswordHash)
        {
            PasswordHash = newPasswordHash;
        }
    }
}