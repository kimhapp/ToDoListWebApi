using Microsoft.EntityFrameworkCore;
using ToDoListWebApi.Models;

namespace ToDoListWebApi.Services
{
    public interface IToDoService
    {
        Task<List<ToDo>> GetAllByUserIdAsync(Guid userId);
        Task<ToDo?> GetByIdAsync(Guid id, Guid userId);
        Task<ToDo> CreateAsync(Guid userId, string title, string description);
        Task<bool> UpdateAsync(Guid id, Guid userId, string title, string description, bool complete);
        Task<bool> RemoveAsync(Guid id, Guid userId);
    }

    public class TodoService(ApplicationDbContext context) : IToDoService
    {
        public async Task<List<ToDo>> GetAllByUserIdAsync(Guid userId)
        {
            return await context.ToDos.AsNoTracking().Where(t => t.UserId == userId).ToListAsync();
        }

        public async Task<ToDo?> GetByIdAsync(Guid id, Guid userId)
        {
            return await context.ToDos.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        }

        public async Task<ToDo> CreateAsync(Guid userId, string title, string description)
        {
            ToDo toDo = new()
            {
                UserId = userId,
                Title = title,
                Description = description,
                Complete = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            context.ToDos.Add(toDo);
            await context.SaveChangesAsync();
            return toDo;
        }

        public async Task<bool> UpdateAsync(Guid id, Guid userId, string title, string description, bool complete)
        {
            ToDo? toDo = await context.ToDos.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (toDo == null) return false;

            toDo.Title = title;
            toDo.Description = description;
            toDo.Complete = complete;
            toDo.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAsync(Guid id, Guid userId)
        {
            ToDo? toDo = await context.ToDos.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (toDo == null) return false;

            context.ToDos.Remove(toDo);
            await context.SaveChangesAsync();
            return true;
        }
    }
}