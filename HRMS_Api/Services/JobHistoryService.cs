using HRMS_Api.DatabaseContext;
using HRMS_Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HRMS_Api.Services
{
	public class JobHistoryService : IJobHistoryService
	{
		private HRMSDbContext _dbContext;
		public JobHistoryService(HRMSDbContext context)
		{
			_dbContext = context;
		}
	
		public async Task<bool> AddNewJobHistory(int EmployeeId, int ManagerId, int JobId, DateTime StartDate, DateTime? EndDate, string Status, string Comments)
		{
			using (var transaction = await _dbContext.Database.BeginTransactionAsync())
			{
				var history = new JobHistory
				{
					StartDate = StartDate,
					EndDate = EndDate,
					Status = Status,
					Comments = Comments,
					Employee = _dbContext.Employees.First(x => x.Id == EmployeeId),
					Manager = _dbContext.Employees.First(x => x.Id == ManagerId),
					Job = _dbContext.Jobs.First(j => j.Id == JobId)
				};
				await _dbContext.JobHistories.AddAsync(history);
				var result = await _dbContext.SaveChangesAsync();
				await transaction.CommitAsync();
				return result > 0;
			}
		}


		public async Task<GetJobHistoryResponse> GetJobHistoryByPage(int EmployeeId, int? Perpage, int? CurrentPage)
		{
			var jobHistories = new List<JobHistory>();
			jobHistories = _dbContext.JobHistories.Include(j => j.Employee).Include(j => j.Manager).Include(j => j.Job).Where(j => j.EmployeeId == EmployeeId).ToList();
			if (Perpage.HasValue && CurrentPage.HasValue)
			{
				var offset = (CurrentPage.Value - 1) * Perpage.Value;
				return new GetJobHistoryResponse() { JobHistories = jobHistories.Skip(offset).Take(Perpage.Value).ToList(), TotalRecord = jobHistories.Count() };
			}

			return new GetJobHistoryResponse() { JobHistories = jobHistories, TotalRecord = jobHistories.Count() };
		}


		public async Task<bool> UpdateJobHistory(int Id, int ManagerId, int JobId, DateTime StartDate, DateTime? EndDate, string Status, string Comments)
		{

			using (var transaction = await _dbContext.Database.BeginTransactionAsync())
			{
				var jobHistory = _dbContext.JobHistories.FirstOrDefault(job => job.Id == Id);
				if (jobHistory == null)
				{
					throw new NullReferenceException("Job History not exist");
				}
				jobHistory.StartDate = StartDate;
				jobHistory.EndDate = EndDate;
				jobHistory.Status = Status;
				jobHistory.Comments = Comments;
				jobHistory.Manager = _dbContext.Employees.First(emp => emp.Id == ManagerId);
				jobHistory.Job = _dbContext.Jobs.First(job => job.Id == JobId);

				var result = await _dbContext.SaveChangesAsync();
				await transaction.CommitAsync();
				return result > 0;
			}
		}

		public async Task RemoveJobHistory(int Id)
		{
			using (var transaction = await _dbContext.Database.BeginTransactionAsync())
			{
				var job = _dbContext.JobHistories.FirstOrDefault(job => job.Id == Id);
				if (job == null)
				{
					throw new NullReferenceException("Job History not exist");
				}
				_dbContext.JobHistories.Remove(job);

				var result = await _dbContext.SaveChangesAsync();
				await transaction.CommitAsync();
			}
		}

	}
}
