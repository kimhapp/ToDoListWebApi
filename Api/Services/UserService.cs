using Microsoft.EntityFrameworkCore;
using ToDoListWebApi.Models;

namespace ToDoListWebApi.Services
{
    public interface IUserService
    {
        Task<User?> LoginAsync(string email, string password);
        Task<User?> RegisterAsync(string name, string email, string password);
    }

    public class UserService(ApplicationDbContext context) : IUserService
    {
        public async Task<User?> LoginAsync(string email, string password)
        {
            User? user = await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHashed))
                return null;

            return user;
        }

        public async Task<User?> RegisterAsync(string name, string email, string password)
        {
            if (await context.Users.AnyAsync(u => u.Email == email))
                return null;

            User user = new()
            {
                Name = name,
                Email = email,
                PasswordHashed = BCrypt.Net.BCrypt.HashPassword(password),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;
        }
    }
}