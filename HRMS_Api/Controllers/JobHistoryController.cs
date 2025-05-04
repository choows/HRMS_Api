using HRMS_Api.Models;
using HRMS_Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace HRMS_Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class JobHistoryController : ControllerBase
	{
		private IJobHistoryService _jobHistoryService;
		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
		public JobHistoryController(IJobHistoryService jobHistoryService)
		{
			this._jobHistoryService = jobHistoryService;
		}

		[HttpPost]
		public async Task<IActionResult> Add([FromBody] AddJobHistory request)
		{
			try
			{
				var result = await _jobHistoryService.AddNewJobHistory(request.EmployeeId, request.ManagerId, request.JobId, request.StartDate, request.EndDate, request.Status, request.Comments);
				if (result)
				{
					return Ok(new { StatusCode = 200, Message = "Added New History" });
				}
				else
				{
					return BadRequest("Unable to add Job History.");
				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Add Job History Error");
				return BadRequest("Add new job history failed due to unexpected error. Kindly contact Administrator for more information.");
			}
		}

		[HttpGet]
		public async Task<IActionResult> Get(int EmployeeId, int? PerPage, int? CurrrentPage)
		{
			try
			{
				var result = await _jobHistoryService.GetJobHistoryByPage(EmployeeId, PerPage, CurrrentPage);
				return Ok(result);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Get Job History Error");
				return BadRequest("Get job history failed due to unexpected error. Kindly contact Administrator for more information.");
			}
		}

		[HttpPut]
		public async Task<IActionResult> Update(int Id, AddJobHistory request)
		{
			try
			{
				var result = await _jobHistoryService.UpdateJobHistory(Id, request.ManagerId, request.JobId, request.StartDate, request.EndDate, request.Status, request.Comments);
				if (result)
				{
					return Ok(new { StatusCode = 200, Message = "History Updated" });
				}
				else
				{
					_logger.Error("Update Job History Error");
					return BadRequest("Unable to update job history");
				}
			}
			catch (Exception ex)
			{
				return BadRequest("Update job history failed due to unexpected error. Kindly contact Administrator for more information.");
			}
		}

		[HttpDelete]
		public async Task<IActionResult> Remove(int Id)
		{
			try
			{
				await _jobHistoryService.RemoveJobHistory(Id);
				return Ok(new { StatusCode = 200, Message = "Removed" });
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Remove Job History Error");
				return BadRequest("Remove department failed due to unexpected error. Kindly contact Administrator for more information.");
			}
		}
	}
}
