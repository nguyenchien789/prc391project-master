using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace prc391project.Models
{
    public partial class PRC391Context : DbContext
    {
        public PRC391Context()
        {
        }

        public PRC391Context(DbContextOptions<PRC391Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<CourseClass> CourseClass { get; set; }
        public virtual DbSet<Room> Room { get; set; }
        public virtual DbSet<Subject> Subject { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(e => e.CourseId).HasColumnName("CourseID");

                entity.Property(e => e.EndTime).HasColumnType("date");

                entity.Property(e => e.RoomId).HasColumnName("RoomID");

                entity.Property(e => e.StartTime).HasColumnType("date");

                entity.Property(e => e.SubjectId)
                    .HasColumnName("SubjectID")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Course)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK_Course_Room");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Course)
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("FK_Course_Subject");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Course)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Course_User");
            });

            modelBuilder.Entity<CourseClass>(entity =>
            {
                entity.HasKey(e => new { e.CourseId, e.UserId })
                    .HasName("PK_Class");

                entity.Property(e => e.CourseId).HasColumnName("CourseID");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CourseClass)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Class_Course");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CourseClass)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Class_User");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.Property(e => e.RoomId).HasColumnName("RoomID");

                entity.Property(e => e.RoomName).HasMaxLength(50);
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.Property(e => e.SubjectId)
                    .HasColumnName("SubjectID")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.SubjectName).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.Password)
                    .HasMaxLength(20)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
