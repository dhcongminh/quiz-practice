using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace QuizPracticeApi.Models
{
    public partial class QuizPracticeContext : DbContext
    {
        public QuizPracticeContext()
        {
        }

        public QuizPracticeContext(DbContextOptions<QuizPracticeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserDetail> UserDetails { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=DHCONGMINH;database= QuizPractice;user=sa;password=123;TrustServerCertificate=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Id, "UQ__Users__3213E83E15F09F5C")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt");

                entity.Property(e => e.Password)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Username)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<UserDetail>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__UserDeta__CB9A1CFF0EEF9D5E");

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("userId");

                entity.Property(e => e.Avatar)
                    .IsUnicode(false)
                    .HasColumnName("avatar");

                entity.Property(e => e.Dob)
                    .HasColumnType("date")
                    .HasColumnName("dob");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(16)
                    .HasColumnName("firstName");

                entity.Property(e => e.Gender)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("gender");

                entity.Property(e => e.LastName)
                    .HasMaxLength(16)
                    .HasColumnName("lastName");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UserDetail)
                    .HasForeignKey<UserDetail>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserDetai__userI__3A81B327");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
