using HRMS_Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Metadata;

namespace HRMS_Api.DatabaseContext
{
	public class HRMSDbContext : DbContext
	{
		public virtual DbSet<Location> Locations { get; set; }
		public virtual DbSet<Department> Departments { get; set; }
		public virtual DbSet<Job> Jobs { get; set; }
		public virtual DbSet<Employee> Employees { get; set; }
		public virtual DbSet<JobHistory> JobHistories { get; set; }


		public HRMSDbContext(DbContextOptions<HRMSDbContext> options) : base(options) { 

		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Department>()
			   .HasOne(j => j.Location)
			   .WithMany(e => e.department)
			   .HasForeignKey(j => j.LocationId)
			   .OnDelete(DeleteBehavior.SetNull);

			modelBuilder.Entity<Job>()
			   .HasOne(j => j.Department)
			   .WithMany(e => e.jobs)
			   .HasForeignKey(j => j.DepartmentId)
			   .OnDelete(DeleteBehavior.SetNull);

			modelBuilder.Entity<JobHistory>()
			   .HasOne(j => j.Employee)
			   .WithMany(e => e.JobHistoryAsEmployee)
			   .HasForeignKey(j => j.EmployeeId)
			   .OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<JobHistory>()
				.HasOne(j => j.Manager)
				.WithMany(e => e.JobHistoryAsManager)
				.HasForeignKey(j => j.ManagerId)
				.OnDelete(DeleteBehavior.SetNull);

			modelBuilder.Entity<JobHistory>()
					   .HasOne(j => j.Job)
					   .WithMany(e => e.jobsHistory)
					   .HasForeignKey(j => j.JobId)
					   .OnDelete(DeleteBehavior.SetNull);

			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}
	}
}
