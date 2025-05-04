using HRMS_Api.Models;
using HRMS_Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace HRMS_Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LocationController : ControllerBase
	{
		private ILocationService _locationService;
		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
		public LocationController(ILocationService locationService) { 
			this._locationService = locationService;
		}

		[HttpPost]
		public async Task<IActionResult> Add([FromBody] AddLocationModel request)
		{
			try
			{
				var result = await _locationService.AddNewLocation(request.Name, request.City, request.Country);
				if (result)
				{
					return Ok(new { StatusCode = 200, Message = "Added New Location"});
				}
				else
				{
					return BadRequest("Unable to add Location.");
				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Add Location Error");
				return BadRequest("Add new location failed due to unexpected error. Kindly contact Administrator for more information.");
			}
		}

		[HttpGet]
		public async Task<IActionResult> Get(int? PerPage, int? CurrrentPage)
		{
			try
			{
				var result = await _locationService.GetLocationByPage(PerPage, CurrrentPage);
				return Ok(result);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Get Location Error");
				return BadRequest("Get location failed due to unexpected error. Kindly contact Administrator for more information.");
			}
		}

		[HttpPut]
		public async Task<IActionResult> Update(int Id, AddLocationModel request)
		{
			try
			{
				var result = await _locationService.UpdateLocation(Id, request.Name, request.City, request.Country);
				if (result)
				{
					return Ok(new { StatusCode = 200, Message = "Updated" });
				}
				else
				{
					return BadRequest("Unable to update location");
				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Update Location Error");
				return BadRequest("Update location failed due to unexpected error. Kindly contact Administrator for more information.");
			}
		}

		[HttpDelete]
		public async Task<IActionResult> Remove(int Id)
		{
			try
			{
				await _locationService.RemoveLocation(Id);
				return Ok(new { StatusCode = 200, Message = "Removed" });
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Remove Location Error");
				return BadRequest("Update location failed due to unexpected error. Kindly contact Administrator for more information.");
			}
		}
	}
}
