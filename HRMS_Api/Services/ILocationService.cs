using HRMS_Api.Models;
using System.ComponentModel;

namespace HRMS_Api.Services
{
	public interface ILocationService
	{
		public Task<bool> AddNewLocation(string Name, string City, string Country);
		public Task<GetLocationResponse> GetLocationByPage(int? Perpage, int? CurrentPage);
		public Task<bool> UpdateLocation(int Id, string Name, string City, string Country);
		public Task RemoveLocation(int Id);
	}
}
