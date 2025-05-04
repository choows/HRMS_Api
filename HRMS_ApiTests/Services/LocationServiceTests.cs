using Microsoft.VisualStudio.TestTools.UnitTesting;
using HRMS_Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using System.Reflection.Metadata;
using HRMS_Api.Models;
using HRMS_Api.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace HRMS_Api.Services.Tests
{
	[TestClass()]
	public class LocationServiceTests
	{
		[TestMethod()]
		public void GetLocationTest()
		{
			// Arrange
			var mockSet = getMockSet();
			var options = new DbContextOptionsBuilder<HRMSDbContext>()
					   .UseMemoryCache(null)
					   .Options;
			var mockContext = new Mock<HRMSDbContext>(options);
			mockContext.Setup(c => c.Locations).Returns(mockSet.Object);
			mockContext.Setup(c=> c.Database).Returns(new Mock<DatabaseFacade>(mockContext.Object).Object);
			var service = new LocationService(mockContext.Object);

			// Act
			var results = service.GetLocationByPage(1, 1).Result;

			// Assert
			Assert.AreEqual(3, results.TotalRecord);
			Assert.AreEqual(1, results.Locations.Count());
			Assert.AreEqual("Test 1", results.Locations.First().Name);

		}

		[TestMethod()]
		public void GetAllLocationTest()
		{
			// Arrange
			var mockSet = getMockSet();
			var options = new DbContextOptionsBuilder<HRMSDbContext>()
					   .UseMemoryCache(null)
					   .Options;
			var mockContext = new Mock<HRMSDbContext>(options);
			mockContext.Setup(c => c.Locations).Returns(mockSet.Object);
			mockContext.Setup(c => c.Database).Returns(new Mock<DatabaseFacade>(mockContext.Object).Object);
			var service = new LocationService(mockContext.Object);

			// Act
			var results = service.GetLocationByPage(null, null).Result;

			// Assert
			Assert.AreEqual(3, results.TotalRecord);
			Assert.AreEqual(3, results.Locations.Count());
			Assert.AreEqual("Test 1", results.Locations.First().Name);
			Assert.AreEqual("Test 3", results.Locations.Last().Name);
		}

		[TestMethod()]
		public void UpdateLocationTest()
		{
			// Arrange
			var mockSet = getMockSet();
			var newName = "Test Name 123";
			var options = new DbContextOptionsBuilder<HRMSDbContext>()
				   .UseInMemoryDatabase(Guid.NewGuid().ToString())
				   .Options;

			var mockContext = new Mock<HRMSDbContext>(options);
			var dbFacade = new Mock<DatabaseFacade>(mockContext.Object);

			dbFacade.Setup(x=> x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
				.Returns(Task.FromResult(new Mock<IDbContextTransaction>().Object));

			mockContext.Setup(c => c.Locations).Returns(mockSet.Object);
			mockContext.Setup(c => c.Database).Returns(dbFacade.Object);
			mockContext.Setup(c=> c.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));
			var service = new LocationService(mockContext.Object);

			// Act
			var result = service.UpdateLocation(2, newName, "City 2", "Country 2").Result;
			var queryResult = service.GetLocationByPage(null, null).Result;
			// Assert
			Assert.IsTrue(result);
			Assert.AreEqual(queryResult.Locations.FirstOrDefault(x=> x.Id == 2).Name, newName);
		}


		private Mock<DbSet<Location>> getMockSet()
		{
			var data = new List<Location>
				{
					new Location { Id = 1, Name = "Test 1", City = "City 1", Country = "Country 1", CreateDateTime = DateTime.Now, UpdateDateTime = DateTime.Now, department =null },
					new Location { Id = 2, Name = "Test 2", City = "City 2", Country = "Country 2", CreateDateTime = DateTime.Now, UpdateDateTime = DateTime.Now, department =null },
					new Location { Id = 3, Name = "Test 3", City = "City 3", Country = "Country 3", CreateDateTime = DateTime.Now, UpdateDateTime = DateTime.Now, department =null },
				}.AsQueryable();

			var mockSet = new Mock<DbSet<Location>>();
			mockSet.As<IQueryable<Location>>().Setup(m => m.Provider).Returns(data.Provider);
			mockSet.As<IQueryable<Location>>().Setup(m => m.Expression).Returns(data.Expression);
			mockSet.As<IQueryable<Location>>().Setup(m => m.ElementType).Returns(data.ElementType);
			mockSet.As<IQueryable<Location>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
			return mockSet;
		}
	}
}