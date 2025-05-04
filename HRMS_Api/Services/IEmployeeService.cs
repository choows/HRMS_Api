using HRMS_Api.Models;

namespace HRMS_Api.Services
{
	public interface IEmployeeService
	{
		public Task<bool> AddNewEmployee(string Name, DateTime BirthDate, string Phone, string Email, string Gender, string Address, string Status);
		public Task<GetEmployeeResponse> GetEmployeeByPage(int? Perpage, int? CurrentPage);
		public Task<bool> UpdateEmployee(int Id, string Name, DateTime BirthDate, string Phone, string Email, string Gender, string Address, string Status);
		public Task RemoveEmployee(int Id);
		public Task<GetEmployeeResponse> SearchEmployee(string Name, int? Department, string Status, string Gender, int? PerPage, int? CurrentPage, DateTime? StartDate, DateTime? EndDate);
	}
}