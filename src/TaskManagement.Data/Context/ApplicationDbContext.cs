using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Core.Models;
using TaskManagement.Data.RepositoryManager;

namespace TaskManagement.Data.EF
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
        , IUnitOfWork
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Core.Models.Task> Task { get; set; }

        //public virtual DbSet<ApplicationUser> ApplicationUser { get; set; }
    }
}
