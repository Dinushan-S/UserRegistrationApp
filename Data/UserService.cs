using System.Threading.Tasks;
using UserRegistrationApp.Models;
using Microsoft.EntityFrameworkCore;


namespace UserRegistrationApp.Data;

public interface IUserService
{

    Task<bool> CreateUser(User user);
    Task<User> GetUserByEmail(string email);
}

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserByEmail(string email)
    {

        if (_context.Users == null)
        {
            throw new InvalidOperationException("Users DbSet is null");
        }
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user != null)
        {
            return user;
        }
        else
        {
            return null;
        }
        //throw new InvalidOperationException($"No user found with email {email}");
    }

    public async Task<bool> CreateUser(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (_context == null || _context.Users == null)
        {
            throw new InvalidOperationException("The database context or Users set is null.");
        }

        _context.Users.Add(user);
        var created = await _context.SaveChangesAsync();

        return created > 0;
    }
}