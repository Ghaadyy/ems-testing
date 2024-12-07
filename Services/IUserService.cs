namespace EMS.Services;

public interface IUserService
{
    /// <summary>
    /// Authenticates a user with the given credentials.
    /// </summary>
    bool Login(string email, string password);

    /// <summary>
    /// Registers a new user with the provided details.
    /// </summary>
    bool RegisterUser(string username, string email, string password);

    /// <summary>
    /// Resets the password for a specific user.
    /// </summary>
    bool ResetPassword(string email, string newPassword);
}