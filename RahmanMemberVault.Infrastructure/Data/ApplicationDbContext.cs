using Microsoft.EntityFrameworkCore;
using RahmanMemberVault.Core.Entities;

namespace RahmanMemberVault.Infrastructure.Data
{

    /// EF Core database context for the application.
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// Gets or sets the Members table.
        public DbSet<Member> Members { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Add any custom configurations here
        }
    }
}