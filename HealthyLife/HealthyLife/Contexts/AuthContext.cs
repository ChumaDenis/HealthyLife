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
                entity.ToTable("User");
                entity.Property(e => e.Id).HasColumnName("UserId");
                entity.Property(e => e.UserName).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.UserEmail).HasMaxLength(25).IsUnicode(false);
                entity.Property(e => e.PasswordSalt).HasMaxLength(128).IsUnicode(false);
                entity.Property(e => e.PasswordHash).HasMaxLength(64).IsUnicode(false);
            });

            modelBuilder.Entity<Token>(entity =>
            {
                entity.ToTable("Token");
                entity.Property(e => e.Id).HasColumnName("TokenID");
                entity.Property(e => e.Value).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.CreateDate).IsUnicode(false);
                entity.Property(e => e.UserId).HasMaxLength(256).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
