using HRMS_Api.DatabaseContext;
using HRMS_Api.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace HRMS_Api.Services
{
	public class EmployeeService : IEmployeeService
	{
		private HRMSDbContext _dbContext;

		public EmployeeService(HRMSDbContext context)
		{
			_dbContext = context;
		}
		public async Task<bool> AddNewEmployee(string Name, DateTime BirthDate, string Phone, string Email, string Gender, string Address, string Status)
		{
			using (var transaction = await _dbContext.Database.BeginTransactionAsync())
			{
				var employee = new Employee
				{
					Name = Name,
					BirthDate = BirthDate,
					Phone = Phone,
					Email = Email,
					Gender = Gender,
					Address = Address,
					Status = Status
				};
				

				if (_dbContext.Employees.Any(dep => dep.Name == Name))
				{
					throw new DuplicateNameException("Employee Name Duplicated");
				}

				await _dbContext.Employees.AddAsync(employee);
				var result = await _dbContext.SaveChangesAsync();
				await transaction.CommitAsync();
				return result > 0;
			}

		}

		public async Task<GetEmployeeResponse> GetEmployeeByPage(int? Perpage, int? CurrentPage)
		{
			var employees = new List<Employee>();

			if (Perpage.HasValue && CurrentPage.HasValue)
			{
				var offset = (CurrentPage.Value - 1) * Perpage.Value;
				employees = _dbContext.Employees.Skip(offset).Take(Perpage.Value).ToList();
			}
			else
			{
				//take all 
				employees = _dbContext.Employees.ToList();
			}
			var totalRecord = _dbContext.Employees.Count();

			return new GetEmployeeResponse() { Employees = employees, TotalRecord = totalRecord};
		}
		public async Task<GetEmployeeResponse> SearchEmployee(string Name, int? Department, string Status, string Gender, int? PerPage, int? CurrentPage, DateTime? StartDate, DateTime? EndDate)
		{
			var response = new GetEmployeeResponse();
			var employees = _dbContext.Employees.Where(emp =>
				(string.IsNullOrEmpty(Name) ? true : emp.Name.Contains(Name)) &&
				(string.IsNullOrEmpty(Status) ? true : emp.Status == Status) &&
				(string.IsNullOrEmpty(Gender) ? true : emp.Gender == Gender) && 
				(StartDate.HasValue ? emp.BirthDate >= StartDate.Value : true) &&
				(EndDate.HasValue ? emp.BirthDate <= EndDate.Value : true)
				).ToList();

			//if filter by department 
			if (Department.HasValue)
			{
				var active_jobs_hist = (from hist in _dbContext.JobHistories
										where !hist.EndDate.HasValue && hist.Status == "active"
										select hist);
				var employee_current_job = (from hist in active_jobs_hist
											join emp in _dbContext.Employees
											on hist.EmployeeId equals emp.Id
											select new
											{
												histId = hist.Id,
												JobId = hist.JobId,
												dep_id = hist.Job.DepartmentId,
												empId = emp.Id
											});

				employees = employees.Where(emp => employee_current_job.Any(cur_job => cur_job.empId == emp.Id && cur_job.dep_id == Department.Value)).ToList();
			}
			response.TotalRecord = employees.Count();

			if (PerPage.HasValue && CurrentPage.HasValue) {
				var offset = (CurrentPage.Value - 1) * PerPage.Value;
				response.Employees = _dbContext.Employees.Skip(offset).Take(PerPage.Value).ToList();
			}
			else
			{
				response.Employees = employees;
			}
			return response;
		}
		public async Task<bool> UpdateEmployee(int Id, string Name, DateTime BirthDate, string Phone, string Email, string Gender, string Address, string Status)
		{
			using (var transaction = await _dbContext.Database.BeginTransactionAsync())
			{
				var employee = _dbContext.Employees.FirstOrDefault(emp => emp.Id == Id);
				if (employee == null)
				{
					throw new NullReferenceException("Employee not exist");
				}
				employee.Name = Name;
				employee.BirthDate = BirthDate;	
				employee.Phone = Phone;	
				employee.Email = Email;
				employee.Gender = Gender;
				employee.Address = Address;
				employee.Status = Status;

				var result = await _dbContext.SaveChangesAsync();
				await transaction.CommitAsync();
				return result > 0;
			}
		}

		public async Task RemoveEmployee(int Id)
		{
			using (var transaction = await _dbContext.Database.BeginTransactionAsync())
			{
				var employee = _dbContext.Employees.FirstOrDefault(emp => emp.Id == Id);
				if (employee == null)
				{
					throw new NullReferenceException("Employee not exist");
				}
				_dbContext.Employees.Remove(employee);

				var result = await _dbContext.SaveChangesAsync();
				await transaction.CommitAsync();
			}
		}
	}
}
