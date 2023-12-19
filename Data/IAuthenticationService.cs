// using UserRegistrationApp.Data;
using BCrypt.Net;
using UserRegistrationApp.Models; // Replace with the actual namespace of your User model
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace UserRegistrationApp.Data;

public interface IAuthenticationService
{
    Task<bool> Login(string email, string password);
    Task<bool> Register(string name, string email, string password);
    Task<bool> IsLoggedIn();


}

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserService _userService;
    private readonly IJSRuntime _jsRuntime;

    // public AuthenticationService(IUserService userService)
    // {
    //     _userService = userService;
    // }

    public AuthenticationService(IUserService userService, IJSRuntime jsRuntime)
    {
        _userService = userService;
        _jsRuntime = jsRuntime;
    }

    private async Task<string> GetToken()
    {
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
        Console.WriteLine($"Token: {token}");
        return token;
        //return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
    }

    public async Task<bool> IsLoggedIn()
    {
        var token = await GetToken();
        var isLoggedIn = !string.IsNullOrEmpty(token);
        Console.WriteLine($"IsLoggedIn: {isLoggedIn}");
        return isLoggedIn;
        //var token = await GetToken();
        //return !string.IsNullOrEmpty(token);
    }

    public async Task<bool> Register(string name, string email, string password)
    {
        Console.WriteLine($"User: {name}, {email}, {password}");

        // Check if a user with the same email already exists
        var existingUser = await _userService.GetUserByEmail(email);
        Console.WriteLine($"User: {existingUser}");

        if (existingUser != null)
        {
            // User already exists
            return false;
        }

        // Hash the password
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        Console.WriteLine($"User: {name}, {email}, {password}");

        // Create a new User object
        var user = new User
        {
            Username = name,
            Email = email,
            Password = hashedPassword
        };
        Console.WriteLine($"User: {user.Username}, {user.Email}, {user.Password}");

        // Save the new user in the database
        await _userService.CreateUser(user);

        return true;
    }

    public async Task<bool> Login(string email, string password)
    {
        var user = await _userService.GetUserByEmail(email);

        if (user == null)
        {
            return false;
        }

        bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.Password);

        return isValidPassword;

        //return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }
}