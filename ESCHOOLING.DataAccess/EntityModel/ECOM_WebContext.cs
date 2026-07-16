using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ESCHOOLING.DataAccess.EntityModel
{
    public partial class ECOM_WebContext : DbContext
    {
        public ECOM_WebContext()
        {
        }

        public ECOM_WebContext(DbContextOptions<ECOM_WebContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblAttendance> TblAttendances { get; set; } = null!;
        public virtual DbSet<TblBehaviour> TblBehaviours { get; set; } = null!;
        public virtual DbSet<TblCounselor> TblCounselors { get; set; } = null!;
        public virtual DbSet<TblCounsellingReferral> TblCounsellingReferrals { get; set; } = null!;
        public virtual DbSet<TblEvent> TblEvents { get; set; } = null!;
        public virtual DbSet<TblHomework> TblHomeworks { get; set; } = null!;
        public virtual DbSet<TblParentNote> TblParentNotes { get; set; } = null!;
        public virtual DbSet<TblStudentBehaviourEntry> TblStudentBehaviourEntries { get; set; } = null!;
        public virtual DbSet<TblStudentMark> TblStudentMarks { get; set; } = null!;
        public virtual DbSet<TblStudentMarksEntry> TblStudentMarksEntries { get; set; } = null!;
        public virtual DbSet<TblUserRegistration> TblUserRegistrations { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Connection is configured via DI (see Program.cs: AddDbContext), reading
            // ConnectionStrings:DefaultConnection from appsettings.json.
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblAttendance>(entity =>
            {
                entity.ToTable("TblAttendance");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.MonthForSearch).HasMaxLength(50);

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.TblAttendances)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TblAttend__Stude__76969D2E");
            });

            modelBuilder.Entity<TblBehaviour>(entity =>
            {
                entity.ToTable("TblBehaviour");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Behaviour1)
                    .HasMaxLength(50)
                    .HasColumnName("behaviour1");

                entity.Property(e => e.Behaviour2)
                    .HasMaxLength(50)
                    .HasColumnName("behaviour2");

                entity.Property(e => e.Behaviour3)
                    .HasMaxLength(50)
                    .HasColumnName("behaviour3");

                entity.Property(e => e.Behaviour4)
                    .HasMaxLength(50)
                    .HasColumnName("behaviour4");

                entity.Property(e => e.Behaviour5)
                    .HasMaxLength(50)
                    .HasColumnName("behaviour5");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.TblBehaviours)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK__TblBehavi__Stude__7D439ABD");
            });

            modelBuilder.Entity<TblCounselor>(entity =>
            {
                entity.HasKey(e => e.CounselorId);

                entity.ToTable("TblCounselor");

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MobileNo)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Specialization)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblCounsellingReferral>(entity =>
            {
                entity.ToTable("TblCounsellingReferral");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Reason).HasMaxLength(500);

                entity.HasOne(d => d.Student)
                    .WithMany()
                    .HasForeignKey(d => d.StudentId);

                entity.HasOne(d => d.Counselor)
                    .WithMany()
                    .HasForeignKey(d => d.CounselorId);

                entity.HasOne(d => d.Teacher)
                    .WithMany()
                    .HasForeignKey(d => d.TeacherId);
            });

            modelBuilder.Entity<TblEvent>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.EventName).HasMaxLength(50);

                entity.Property(e => e.Place).HasMaxLength(50);

                entity.Property(e => e.Time).HasMaxLength(50);
            });

            modelBuilder.Entity<TblStudentMark>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.PredictedMark).HasMaxLength(50);

                entity.Property(e => e.Subject).HasMaxLength(50);

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.TblStudentMarks)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK__TblStuden__Stude__7E37BEF6");
            });

            modelBuilder.Entity<TblStudentBehaviourEntry>(entity =>
            {
                entity.ToTable("TblStudentBehaviourEntry");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.BehaviourType).HasMaxLength(20);

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.MonthForSearch).HasMaxLength(50);

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.TblStudentBehaviourEntries)
                    .HasForeignKey(d => d.StudentId);
            });

            modelBuilder.Entity<TblStudentMarksEntry>(entity =>
            {
                entity.ToTable("TblStudentMarksEntry");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Term).HasMaxLength(50);

                entity.Property(e => e.Subject).HasMaxLength(50);

                entity.Property(e => e.Marks).HasColumnType("decimal(5, 2)");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.TblStudentMarksEntries)
                    .HasForeignKey(d => d.StudentId);
            });

            modelBuilder.Entity<TblHomework>(entity =>
            {
                entity.ToTable("TblHomework");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.Property(e => e.Subject).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.TblHomeworks)
                    .HasForeignKey(d => d.TeacherId)
                    .HasConstraintName("FK__TblHomewo__Teach__4BAC3F29");
            });

            modelBuilder.Entity<TblParentNote>(entity =>
            {
                entity.ToTable("TblParentNote");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.NoteText).HasMaxLength(1000);

                entity.HasOne(d => d.Parent)
                    .WithMany()
                    .HasForeignKey(d => d.ParentId);

                entity.HasOne(d => d.Student)
                    .WithMany()
                    .HasForeignKey(d => d.StudentId);
            });

            modelBuilder.Entity<TblUserRegistration>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__TblUserR__1788CC4C938262B3");

                entity.ToTable("TblUserRegistration");

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MobileNo)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
