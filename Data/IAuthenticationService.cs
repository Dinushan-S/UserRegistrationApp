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
    Task<bool> Register2(User model);
}

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserService _userService;


    public AuthenticationService(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<bool> Register(string name, string email, string password)
    {
        var existingUser = await _userService.GetUserByEmail(email);

        if (existingUser != null)
        {
            // User already exists
            return false;
        }

        // string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        // Create a new User object
        var user = new User
        {
            Username = name,
            Email = email,
            // Password = hashedPassword
        };

        user.SetPassword(password);

        await _userService.CreateUser(user);

        return true;
    }

    public async Task<bool> Register2(User model)
    {
        var existingUser = await _userService.GetUserByEmail(model.Email);

        if (existingUser != null)
        {
            // User already exists
            return false;
        }

        // string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

        // Create a new User object
        var user = new User
        {
            Username = model.Username,
            Email = model.Username,
        };

        // user.SetPassword();

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

        bool isValidPassword = user.VerifyPassword(password);
        Console.WriteLine($"isvalidat {isValidPassword}");
        return isValidPassword;

    }
}

