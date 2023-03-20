using HealthyLife.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthyLife.Contexts
{
    public partial class AuthContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Token> Tokens { get; set; }

        public AuthContext(DbContextOptions<AuthContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("User");
                entity.Property(e => e.Id).HasColumnName("UserId");
                entity.Property(e => e.UserName).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.Mail).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.PasswordSalt).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.PasswordHash).HasMaxLength(50).IsUnicode(false);
            });

            modelBuilder.Entity<Token>(entity =>
            {
                entity.ToTable("Employee");
                entity.Property(e => e.Id).HasColumnName("EmployeeID");
                entity.Property(e => e.Value).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.ExpirationTime).IsUnicode(false);
                entity.Property(e => e.UserId).HasMaxLength(256).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
