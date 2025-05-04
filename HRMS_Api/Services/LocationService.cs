using HRMS_Api.DatabaseContext;
using HRMS_Api.Models;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Threading;

namespace HRMS_Api.Services
{
	public class LocationService : ILocationService
	{
		private HRMSDbContext _dbContext;
		public LocationService(HRMSDbContext context) {
			_dbContext = context;
		}
		public async Task<bool> AddNewLocation(string Name, string City, string Country)
		{
			using (var transaction = await _dbContext.Database.BeginTransactionAsync())
			{
				var location = new Location
				{
					Name = Name,
					City = City,
					Country = Country,
					CreateDateTime = DateTime.Now,
					UpdateDateTime = DateTime.Now
				};
				if (_dbContext.Locations.Any(loc => loc.Name == Name))
				{
					throw new DuplicateNameException("Location Name Duplicated");
				}
				await _dbContext.Locations.AddAsync(location);
				var result = await _dbContext.SaveChangesAsync();
				await transaction.CommitAsync();
				return result > 0;
			}
		}

		public async Task<GetLocationResponse> GetLocationByPage(int? Perpage, int? CurrentPage)
		{
			var response = new GetLocationResponse();
			if (Perpage.HasValue && CurrentPage.HasValue)
			{
				var offset = (CurrentPage.Value -1 ) * Perpage.Value;
				var total = _dbContext.Locations.Count();
				var total_page = total % Perpage.Value != 0 ? (total / Perpage.Value) + 1 : (total / Perpage.Value);
				response.Locations = _dbContext.Locations.OrderBy(x => x.Id).Skip(offset).Take(Perpage.Value).ToList();
				response.TotalRecord = total;
			}
			else
			{
				//take all 
				var total = _dbContext.Locations.OrderBy(x => x.Id).ToList();
				response.Locations = total;
				response.TotalRecord = total.Count();
			}

			return response;
		}

		public async Task<bool> UpdateLocation(int Id, string Name, string City, string Country)
		{

			using (var transaction = await _dbContext.Database.BeginTransactionAsync())
			{
				var location = _dbContext.Locations.FirstOrDefault(loc => loc.Id == Id);
				if (location == null)
				{
					throw new NullReferenceException("Location not exist");
				}

				location.Name = Name;
				location.City = City;
				location.Country = Country;
				location.UpdateDateTime = DateTime.Now;

				var result = await _dbContext.SaveChangesAsync();
				await transaction.CommitAsync();
				return result > 0;
			}
		}

		public async Task RemoveLocation(int Id)
		{
			using (var transaction = await _dbContext.Database.BeginTransactionAsync())
			{
				var location = _dbContext.Locations.FirstOrDefault(loc => loc.Id == Id);
				if (location == null)
				{
					throw new NullReferenceException("Location not exist");
				}
				_dbContext.Locations.Remove(location);

				var result = await _dbContext.SaveChangesAsync();
				await transaction.CommitAsync();
			}
		}
	}
}
