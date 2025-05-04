using HRMS_Api.Models;
using HRMS_Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace HRMS_Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DepartmentController : ControllerBase
	{
		private IDepartmentService _departmentService;
		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
		public DepartmentController(IDepartmentService departmentService)
		{
			this._departmentService = departmentService;
		}

		[HttpPost]
		public async Task<IActionResult> Add([FromBody] AddDepartmentModel request)
		{
			try
			{
				var result = await _departmentService.AddNewDepartment(request.Name, request.Description, request.LocationId);
				if (result)
				{
					return Ok(new { StatusCode = 200, Message = "Added New Department" });
				}
				else
				{
					return BadRequest("Unable to add Department.");
				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Add Department Error");
				return BadRequest("Add new department failed due to unexpected error. Kindly contact Administrator for more information.");
			}
		}

		[HttpGet]
		public async Task<IActionResult> Get(int? PerPage, int? CurrrentPage)
		{
			try
			{
				var result = await _departmentService.GetDepartmentByPage(PerPage, CurrrentPage);
				return Ok(result);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Get Department Error");
				return BadRequest("Get department failed due to unexpected error. Kindly contact Administrator for more information.");
			}
		}

		[HttpPut]
		public async Task<IActionResult> Update(int Id, AddDepartmentModel request)
		{
			try
			{
				var result = await _departmentService.UpdateDepartment(Id, request.Name, request.Description, request.LocationId);
				if (result)
				{
					return Ok(new { StatusCode = 200, Message = "Updated" });
				}
				else
				{
					return BadRequest("Unable to update department");
				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Update Department Error");
				return BadRequest("Update department failed due to unexpected error. Kindly contact Administrator for more information.");
			}
		}

		[HttpDelete]
		public async Task<IActionResult> Remove(int Id)
		{
			try
			{
				await _departmentService.RemoveDepartment(Id);
				return Ok(new { StatusCode = 200, Message = "Removed" });
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Remove Department Error");
				return BadRequest("Remove department failed due to unexpected error. Kindly contact Administrator for more information.");
			}
		}
	}
}
