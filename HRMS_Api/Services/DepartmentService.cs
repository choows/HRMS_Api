using HRMS_Api.DatabaseContext;
using HRMS_Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics.Metrics;

namespace HRMS_Api.Services
{
	public class DepartmentService : IDepartmentService
	{
		private HRMSDbContext _dbContext;

		public DepartmentService(HRMSDbContext context)
		{
			_dbContext = context;
		}
		public async Task<bool> AddNewDepartment(string Name, string Description, int LocationId)
		{
			using (var transaction = await _dbContext.Database.BeginTransactionAsync())
			{

				var department = new Department
				{
					Name = Name,
					Description = Description,
					Location = _dbContext.Locations.First(l => l.Id == LocationId),
					CreateDateTime = DateTime.Now,
					UpdateDateTime = DateTime.Now
				};

				if (_dbContext.Departments.Any(dep => dep.Name == Name))
				{
					throw new DuplicateNameException("Department Name Duplicated");
				}
				await _dbContext.Departments.AddAsync(department);
				var result = await _dbContext.SaveChangesAsync();
				await transaction.CommitAsync();
				return result > 0;
			}

		}

		public async Task<GetDepartmentResponse> GetDepartmentByPage(int? Perpage, int? CurrentPage)
		{
			var departments = new List<Department>();

			if (Perpage.HasValue && CurrentPage.HasValue)
			{
				var offset = (CurrentPage.Value - 1) * Perpage.Value;
				departments = _dbContext.Departments.Include(d=> d.Location).Skip(offset).Take(Perpage.Value).ToList();
			}
			else
			{
				//take all 
				departments = _dbContext.Departments.Include(d => d.Location).ToList();
			}
			var total_record = _dbContext.Departments.Count();
			return new GetDepartmentResponse() { Departments = departments, TotalRecord = total_record};
		}

		public async Task<bool> UpdateDepartment(int Id, string Name, string Description, int LocationId)
		{
			using (var transaction = await _dbContext.Database.BeginTransactionAsync())
			{
				var department = _dbContext.Departments.FirstOrDefault(dep => dep.Id == Id);
				if (department == null)
				{
					throw new NullReferenceException("Department not exist");
				}
				department.Name = Name;
				department.Description = Description;
				department.Location = _dbContext.Locations.First(loc => loc.Id == LocationId);
				department.UpdateDateTime = DateTime.Now;

				var result = await _dbContext.SaveChangesAsync();
				await transaction.CommitAsync();
				return result > 0;
			}
		}

		public async Task RemoveDepartment(int Id)
		{
			using (var transaction = await _dbContext.Database.BeginTransactionAsync())
			{
				var department = _dbContext.Departments.FirstOrDefault(loc => loc.Id == Id);
				if (department == null)
				{
					throw new NullReferenceException("Department not exist");
				}
				_dbContext.Departments.Remove(department);

				var result = await _dbContext.SaveChangesAsync();
				await transaction.CommitAsync();
			}
		}

	}
}
