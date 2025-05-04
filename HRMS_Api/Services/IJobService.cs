using HRMS_Api.Models;

namespace HRMS_Api.Services
{
	public interface IJobService
	{
		public Task<bool> AddNewJob(string Name, string Description, int DepartmentId);
		public Task<GetJobResponse> GetJobByPage(int? Perpage, int? CurrentPage);
		public Task<bool> UpdateJob(int Id, string Name, string Description, int DepartmentId);
		public Task RemoveJob(int Id);
	}
}
