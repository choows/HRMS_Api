using HRMS_Api.Models;

namespace HRMS_Api.Services
{
	public interface IDepartmentService
	{
		public Task<bool> AddNewDepartment(string Name, string Description, int LocationId);
		public Task<GetDepartmentResponse> GetDepartmentByPage(int? Perpage, int? CurrentPage);
		public Task<bool> UpdateDepartment(int Id, string Name, string Description, int LocationId);
		public Task RemoveDepartment(int Id);
	}
}
