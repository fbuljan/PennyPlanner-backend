using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PennyPlanner.Models;

namespace PennyPlanner.Database
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Database.db");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasKey(e => e.Id);
            builder.Entity<Account>().HasKey(e => e.Id);
            builder.Entity<Goal>().HasKey(e => e.Id);
            builder.Entity<Transaction>().HasKey(e => e.Id);

            builder.Entity<User>()
                .HasMany(e => e.Accounts)
                .WithOne(e => e.User);

            builder.Entity<User>()
                .HasMany(e => e.Goals)
                .WithOne(e => e.User);

            builder.Entity<User>()
                .HasMany(e => e.Transactions)
                .WithOne(e => e.User);

            builder.Entity<Account>()
                .HasMany(e => e.Transactions)
                .WithOne(e => e.Account);

            base.OnModelCreating(builder);
        }
    }
}
