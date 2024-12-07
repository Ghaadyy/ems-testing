namespace EMS.Services;

using System.Security.Cryptography;
using System.Text;
using EMS.Models;

public class UserService : IUserService
{
    private readonly Dictionary<string, User> _users = new();

    public UserService()
    {
        var admin = new User(
            Id: 1,
            Username: "admin",
            Email: "admin@example.com",
            PasswordHash: HashPassword("admin123")
        );
        _users[admin.Email] = admin;
    }

    public bool Login(string email, string password)
    {
        if (!_users.TryGetValue(email, out var user))
            return false;

        return VerifyPassword(password, user.PasswordHash);
    }

    public bool RegisterUser(string username, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required.");

        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            throw new ArgumentException("Invalid email address.");

        if (_users.ContainsKey(email))
            throw new InvalidOperationException("A user with this email already exists.");

        var userId = _users.Count + 1;
        var newUser = new User(
            Id: userId,
            Username: username,
            Email: email,
            PasswordHash: HashPassword(password)
        );

        _users[email] = newUser;
        return true;
    }

    public bool ResetPassword(string email, string newPassword)
    {
        if (!_users.TryGetValue(email, out var user))
            throw new KeyNotFoundException("User not found.");

        if (string.IsNullOrWhiteSpace(newPassword))
            throw new ArgumentException("Password cannot be empty.");

        _users[email] = user with { PasswordHash = HashPassword(newPassword) };
        return true;
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private static bool VerifyPassword(string password, string hashedPassword)
    {
        return HashPassword(password) == hashedPassword;
    }
}
