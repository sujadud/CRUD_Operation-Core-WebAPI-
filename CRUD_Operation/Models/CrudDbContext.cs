using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CRUD_Operation.Models
{
    public partial class CrudDbContext : DbContext
    {
        public CrudDbContext()
        {
        }

        public CrudDbContext(DbContextOptions<CrudDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<EmployeeAttendance> EmployeeAttendances { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=CrudDb;Trusted_Connection=True;MultipleActiveResultSets=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.SlId);

                entity.Property(e => e.SlId).HasColumnName("slId");

                entity.Property(e => e.EmployeeCode).HasColumnName("employeeCode");

                entity.Property(e => e.EmployeeId).HasColumnName("employeeId");

                entity.Property(e => e.EmployeeName).HasColumnName("employeeName");

                entity.Property(e => e.EmployeeSalary).HasColumnName("employeeSalary");
            });

            modelBuilder.Entity<EmployeeAttendance>(entity =>
            {
                entity.HasKey(e => e.SlId);

                entity.Property(e => e.SlId).HasColumnName("slId");

                entity.Property(e => e.AttendanceDate).HasColumnName("attendanceDate");

                entity.Property(e => e.EmployeeId).HasColumnName("employeeId");

                entity.Property(e => e.IsAbsent).HasColumnName("isAbsent");

                entity.Property(e => e.IsOffday).HasColumnName("isOffday");

                entity.Property(e => e.IsPresent).HasColumnName("isPresent");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
