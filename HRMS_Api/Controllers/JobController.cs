using HRMS_Api.Models;
using HRMS_Api.Services;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace HRMS_Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class JobController : ControllerBase
	{
		private IJobService _jobService;
		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
		public JobController(IJobService jobService)
		{
			this._jobService = jobService;
		}

		[HttpPost]
		public async Task<IActionResult> Add([FromBody] AddJobModel request)
		{
			try
			{
				var result = await _jobService.AddNewJob(request.Name, request.Description, request.DepartmentId);
				if (result)
				{
					return Ok(new { StatusCode = 200, Message = "Added New Job" });
				}
				else
				{
					return BadRequest("Unable to add job.");
				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Add Job Error");
				return BadRequest("Add new job failed due to unexpected error. Kindly contact Administrator for more information.");
			}
		}

		[HttpGet]
		public async Task<IActionResult> Get(int? PerPage, int? CurrrentPage)
		{
			try
			{
				var result = await _jobService.GetJobByPage(PerPage, CurrrentPage);
				return Ok(result);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Get Job Error");
				return BadRequest("Get job failed due to unexpected error. Kindly contact Administrator for more information.");
			}
		}

		[HttpPut]
		public async Task<IActionResult> Update(int Id, AddJobModel request)
		{
			try
			{
				var result = await _jobService.UpdateJob(Id, request.Name, request.Description, request.DepartmentId);
				if (result)
				{
					return Ok(new { StatusCode = 200, Message = "Updated" });
				}
				else
				{
					_logger.Error("Update Job Error");
					return BadRequest("Unable to update job");
				}
			}
			catch (Exception ex)
			{
				return BadRequest("Update job failed due to unexpected error. Kindly contact Administrator for more information.");
			}
		}

		[HttpDelete]
		public async Task<IActionResult> Remove(int Id)
		{
			try
			{
				await _jobService.RemoveJob(Id);
				return Ok(new { StatusCode = 200, Message = "Removed" });
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Remove Job Error");
				return BadRequest("Remove department failed due to unexpected error. Kindly contact Administrator for more information.");
			}
		}
	}
}
