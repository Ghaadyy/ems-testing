namespace EMS;

using EMS.Services;

public class UserTest
{
    private readonly UserService _userService;

    public UserTest() => _userService = new UserService();

    [Fact]
    public void RegisterUser_ShouldRegisterSuccessfully()
    {
        string username = "johndoe";
        string email = "john.doe@example.com";
        string password = "securepassword";

        bool result = _userService.RegisterUser(username, email, password);

        Assert.True(result, "User registration should succeed.");
    }

    [Fact]
    public void RegisterUser_WithDuplicateEmail_ShouldThrowException()
    {
        string username1 = "johndoe";
        string email = "john.doe@example.com";
        string password1 = "securepassword";

        string username2 = "anotheruser";
        string password2 = "anotherpassword";

        _userService.RegisterUser(username1, email, password1);

        Assert.Throws<InvalidOperationException>(() =>
            _userService.RegisterUser(username2, email, password2)
        );
    }

    [Fact]
    public void Login_WithValidCredentials_ShouldReturnTrue()
    {
        string username = "johndoe";
        string email = "john.doe@example.com";
        string password = "securepassword";

        _userService.RegisterUser(username, email, password);

        bool loginResult = _userService.Login(email, password);

        Assert.True(loginResult, "Login with valid credentials should succeed.");
    }

    [Fact]
    public void Login_WithInvalidPassword_ShouldReturnFalse()
    {
        string username = "johndoe";
        string email = "john.doe@example.com";
        string password = "securepassword";

        _userService.RegisterUser(username, email, password);

        bool loginResult = _userService.Login(email, "wrongpassword");

        Assert.False(loginResult, "Login with an invalid password should fail.");
    }

    [Fact]
    public void Login_WithNonExistentUser_ShouldReturnFalse()
    {
        string email = "nonexistent@example.com";
        string password = "somepassword";

        bool loginResult = _userService.Login(email, password);

        Assert.False(loginResult, "Login with non-existent user should fail.");
    }

    [Fact]
    public void ResetPassword_ShouldUpdatePassword()
    {
        string username = "johndoe";
        string email = "john.doe@example.com";
        string initialPassword = "securepassword";
        string newPassword = "newpassword";

        _userService.RegisterUser(username, email, initialPassword);

        bool resetResult = _userService.ResetPassword(email, newPassword);
        bool loginWithNewPassword = _userService.Login(email, newPassword);
        bool loginWithOldPassword = _userService.Login(email, initialPassword);

        Assert.True(resetResult, "Password reset should succeed.");
        Assert.True(loginWithNewPassword, "Login with new password should succeed.");
        Assert.False(loginWithOldPassword, "Login with old password should fail.");
    }

    [Fact]
    public void ResetPassword_ForNonExistentUser_ShouldThrowException()
    {
        string email = "nonexistent@example.com";
        string newPassword = "newpassword";

        Assert.Throws<KeyNotFoundException>(() =>
            _userService.ResetPassword(email, newPassword)
        );
    }
}
