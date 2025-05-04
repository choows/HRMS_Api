using HRMS_Api.DatabaseContext;
using HRMS_Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HRMS_Api.Services
{
	public class JobService : IJobService
	{
		private HRMSDbContext _dbContext;
		public JobService(HRMSDbContext context)
		{
			_dbContext = context;
		}

		public async Task<bool> AddNewJob(string Name, string Description, int DepartmentId)
		{
			using (var transaction = await _dbContext.Database.BeginTransactionAsync())
			{
				var job = new Job
				{
					Name = Name,
					Description = Description,
					Department = _dbContext.Departments.First(d => d.Id == DepartmentId),
					CreateDateTime = DateTime.Now,
					UpdateDateTime = DateTime.Now
				};


				if (_dbContext.Jobs.Any(job => job.Name == Name))
				{
					throw new DuplicateNameException("Job Name Duplicated");
				}
				await _dbContext.Jobs.AddAsync(job);
				var result = await _dbContext.SaveChangesAsync();
				await transaction.CommitAsync();
				return result > 0;
			}
		}

		public async Task<GetJobResponse> GetJobByPage(int? Perpage, int? CurrentPage)
		{
			var jobs = new List<Job>();

			if (Perpage.HasValue && CurrentPage.HasValue)
			{
				var offset = (CurrentPage.Value - 1) * Perpage.Value;
				jobs = _dbContext.Jobs.Include(j => j.Department).ThenInclude(j => j.Location).Skip(offset).Take(Perpage.Value).ToList();
			}
			else
			{
				//take all 
				jobs = _dbContext.Jobs.Include(j => j.Department).ThenInclude(j => j.Location).ToList();
			}
			var totalRecord = _dbContext.Jobs.Count();

			return new GetJobResponse { TotalRecord = totalRecord,Jobs = jobs };
		}

		public async Task<bool> UpdateJob(int Id, string Name, string Description, int DepartmentId)
		{

			using (var transaction = await _dbContext.Database.BeginTransactionAsync())
			{
				var job = _dbContext.Jobs.FirstOrDefault(job => job.Id == Id);
				if (job == null)
				{
					throw new NullReferenceException("Job not exist");
				}

				job.Name = Name;
				job.Description = Description;
				job.DepartmentId = DepartmentId;

				job.UpdateDateTime = DateTime.Now;

				var result = await _dbContext.SaveChangesAsync();
				await transaction.CommitAsync();
				return result > 0;
			}
		}

		public async Task RemoveJob(int Id)
		{
			using (var transaction = await _dbContext.Database.BeginTransactionAsync())
			{
				var job = _dbContext.Jobs.FirstOrDefault(job => job.Id == Id);
				if (job == null)
				{
					throw new NullReferenceException("Job not exist");
				}
				_dbContext.Jobs.Remove(job);

				var result = await _dbContext.SaveChangesAsync();
				await transaction.CommitAsync();
			}
		}
	}
}
