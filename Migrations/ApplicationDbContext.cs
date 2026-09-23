using Microsoft.EntityFrameworkCore;
using ToDoListWebApi.Models;

namespace ToDoListWebApi.Migrations
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
        public DbSet<ToDo> ToDos { get; set; }
    }
}