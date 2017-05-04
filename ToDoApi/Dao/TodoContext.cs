using Microsoft.EntityFrameworkCore;
using ToDoApi.Models;

namespace ToDoApi.Dao
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
        }

        public DbSet<Todo> Todos { get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }
    }
}