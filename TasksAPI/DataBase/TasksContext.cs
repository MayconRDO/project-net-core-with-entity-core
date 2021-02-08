using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TasksAPI.Models;

namespace TasksAPI.DataBase
{
    public class TasksContext : IdentityDbContext<ApplicationUser>
    {
        public TasksContext(DbContextOptions<TasksContext> options) : base(options)
        {

        }

        public DbSet<Task> Tasks { get; set; }
    }
}
