using HRMS_Api.Models;

namespace HRMS_Api.Services
{
	public interface IJobHistoryService
	{
		public Task<bool> AddNewJobHistory(int EmployeeId, int ManagerId, int JobId, DateTime StartDate, DateTime? EndDate, string Status, string Comments);
		public Task<GetJobHistoryResponse> GetJobHistoryByPage(int EmployeeId, int? Perpage, int? CurrentPage);
		public Task<bool> UpdateJobHistory(int Id, int ManagerId, int JobId, DateTime StartDate, DateTime? EndDate, string Status, string Comments);
		public Task RemoveJobHistory(int Id);
	}
}
