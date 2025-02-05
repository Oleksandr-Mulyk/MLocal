using Local.Web.Data.ToDo;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Local.Web.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
        IdentityDbContext<ApplicationUser>(options)
    {
        public virtual DbSet<ToDoItem> ToDoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ToDoItem>()
                .HasMany(e => e.AssignedTo)
                .WithMany();

            modelBuilder.Entity<ToDoItem>()
                .HasMany(e => e.VisibleFor)
                .WithMany();
        }
    }
}
