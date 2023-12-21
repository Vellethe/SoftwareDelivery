using Backend1.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public class TodoContext : DbContext
    {

        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<Note> Notes { get; set; }
    }
}