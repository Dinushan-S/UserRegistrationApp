using Microsoft.EntityFrameworkCore;
using UserRegistrationApp.Models; // Replace with the actual namespace of your User model

namespace UserRegistrationApp.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User>? Users { get; set; }
}