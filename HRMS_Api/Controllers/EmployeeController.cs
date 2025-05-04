using HRMS_Api.Models;
using HRMS_Api.Services;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace HRMS_Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EmployeeController : ControllerBase
	{
		private IEmployeeService _employeeService;
		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
		public EmployeeController(IEmployeeService employeeService)
		{
			this._employeeService = employeeService;
		}

		[HttpPost("Search")]
		public async Task<IActionResult> Search([FromBody] EmployeeSearchCriteriaModel request)
		{
			try
			{
				var result = await _employeeService.SearchEmployee(request.name, request.Department, request.Status, request.Gender, request.PerPage, request.CurrentPage, request.StartDate, request.EndDate);
				return Ok(new { StatusCode = 200, employees = result });
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Search Employee Error");
				return BadRequest("Search employee failed due to unexpected error. Kindly contact Administrator for more information.");
			}
		}
		[HttpPost]
		public async Task<IActionResult> Add([FromBody] AddEmployeeModel request)
		{
			try
			{
				var result = await _employeeService.AddNewEmployee(request.Name, request.BirthDate, request.Phone, request.Email, request.Gender, request.Address , request.Status);
				if (result)
				{
					return Ok(new { StatusCode = 200, Message = "Added New Employee" });
				}
				else
				{
					return BadRequest("Unable to add Employee.");
				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Add Employee Error");
				return BadRequest("Add new employee failed due to unexpected error. Kindly contact Administrator for more information.");
			}
		}

		[HttpGet]
		public async Task<IActionResult> Get(int? PerPage, int? CurrrentPage)
		{
			try
			{
				var result = await _employeeService.GetEmployeeByPage(PerPage, CurrrentPage);
				return Ok(result);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Get Employee Error");
				return BadRequest("Get employee failed due to unexpected error. Kindly contact Administrator for more information.");
			}
		}

		[HttpPut]
		public async Task<IActionResult> Update(int Id, AddEmployeeModel request)
		{
			try
			{
				var result = await _employeeService.UpdateEmployee(Id, request.Name, request.BirthDate, request.Phone, request.Email, request.Gender, request.Address, request.Status);
				if (result)
				{
					return Ok(new { StatusCode = 200, Message = "Updated" });
				}
				else
				{
					return BadRequest("Unable to update employee");
				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Update Employee Error");
				return BadRequest("Update employee failed due to unexpected error. Kindly contact Administrator for more information.");
			}
		}

		[HttpDelete]
		public async Task<IActionResult> Remove(int Id)
		{
			try
			{
				await _employeeService.RemoveEmployee(Id);
				return Ok(new { StatusCode = 200, Message = "Removed" });
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Remove Employee Error");
				return BadRequest("Remove department failed due to unexpected error. Kindly contact Administrator for more information.");
			}
			
		}
	}
	
}
