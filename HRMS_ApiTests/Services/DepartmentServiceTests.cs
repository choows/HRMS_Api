using Moq;
using HRMS_Api.Models;
using HRMS_Api.DatabaseContext;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Location = HRMS_Api.Models.Location;

namespace HRMS_Api.Services.Tests
{
	[TestClass()]
	public class DepartmentServiceTests
	{
		[TestMethod()]
		public void GetDepartmentTest()
		{
			// Arrange
			var mockSet = getMockSet();

			var options = new DbContextOptionsBuilder<HRMSDbContext>()
					   .UseMemoryCache(null)
					   .Options;
			var mockContext = new Mock<HRMSDbContext>(options);
			mockContext.Setup(c => c.Departments).Returns(mockSet.Object);
			mockContext.Setup(c => c.Database).Returns(new Mock<DatabaseFacade>(mockContext.Object).Object);
			var service = new DepartmentService(mockContext.Object);

			// Act
			var results = service.GetDepartmentByPage(1, 1).Result;

			// Assert
			Assert.AreEqual(3, results.TotalRecord);
			Assert.AreEqual(1, results.Departments.Count());
			Assert.AreEqual("HR", results.Departments.First().Name);

		}

		[TestMethod()]
		public void GetAllDepartmentTest()
		{
			// Arrange
			var mockSet = getMockSet();

			var options = new DbContextOptionsBuilder<HRMSDbContext>()
					   .UseMemoryCache(null)
					   .Options;
			var mockContext = new Mock<HRMSDbContext>(options);
			mockContext.Setup(c => c.Departments).Returns(mockSet.Object);
			mockContext.Setup(c => c.Database).Returns(new Mock<DatabaseFacade>(mockContext.Object).Object);
			var service = new DepartmentService(mockContext.Object);

			// Act
			var results = service.GetDepartmentByPage(null, null).Result;

			// Assert
			Assert.AreEqual(3, results.TotalRecord);
			Assert.AreEqual(3, results.Departments.Count());
			Assert.AreEqual("HR", results.Departments.First().Name);
			Assert.AreEqual("Finance", results.Departments.Last().Name);
		}

		[TestMethod()]
		public void UpdateDepartmentTest()
		{
			// Arrange
			var locationData = new List<Location>
				{
					new Location { Id = 1, Name = "Test 1", City = "City 1", Country = "Country 1", CreateDateTime = DateTime.Now, UpdateDateTime = DateTime.Now, department =null },
					new Location { Id = 2, Name = "Test 2", City = "City 2", Country = "Country 2", CreateDateTime = DateTime.Now, UpdateDateTime = DateTime.Now, department =null },
					new Location { Id = 3, Name = "Test 3", City = "City 3", Country = "Country 3", CreateDateTime = DateTime.Now, UpdateDateTime = DateTime.Now, department =null },
				}.AsQueryable();

			var mockLocationSet = new Mock<DbSet<Location>>();
			mockLocationSet.As<IQueryable<Location>>().Setup(m => m.Provider).Returns(locationData.Provider);
			mockLocationSet.As<IQueryable<Location>>().Setup(m => m.Expression).Returns(locationData.Expression);
			mockLocationSet.As<IQueryable<Location>>().Setup(m => m.ElementType).Returns(locationData.ElementType);
			mockLocationSet.As<IQueryable<Location>>().Setup(m => m.GetEnumerator()).Returns(() => locationData.GetEnumerator());

			var newName = "Test 4";
			var mockSet = getMockSet();


			var options = new DbContextOptionsBuilder<HRMSDbContext>()
				   .UseInMemoryDatabase(Guid.NewGuid().ToString())
				   .Options;

			var mockContext = new Mock<HRMSDbContext>(options);
			var dbFacade = new Mock<DatabaseFacade>(mockContext.Object);

			dbFacade.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
				.Returns(Task.FromResult(new Mock<IDbContextTransaction>().Object));

			mockContext.Setup(c => c.Departments).Returns(mockSet.Object);
			mockContext.Setup(c => c.Locations).Returns(mockLocationSet.Object);
			mockContext.Setup(c => c.Database).Returns(dbFacade.Object);
			mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));
			var service = new DepartmentService(mockContext.Object);

			// Act


			var result = service.UpdateDepartment(2, newName, "City 2", 1).Result;
			var queryResult = service.GetDepartmentByPage(null, null).Result;
			// Assert
			Assert.IsTrue(result);
			Assert.AreEqual(queryResult.Departments.FirstOrDefault(x => x.Id == 2).Name, newName);
		}
	
		private Mock<DbSet<Department>> getMockSet()
		{
			var data = new List<Department>
				{
				new Department(){
					Id = 1,
					Name = "HR",
					Description = "HR Department",
					CreateDateTime = DateTime.Now,
					UpdateDateTime = DateTime.Now,
					LocationId = 1
				},
				new Department(){
					Id = 2,
					Name = "IT",
					Description = "IT Department",
					CreateDateTime = DateTime.Now,
					UpdateDateTime = DateTime.Now,
					LocationId = 1
				},
				new Department(){
					Id = 3,
					Name = "Finance",
					Description = "Finance Department",
					CreateDateTime = DateTime.Now,
					UpdateDateTime = DateTime.Now,
					LocationId = 1
				}
			}.AsQueryable();

			var mockSet = new Mock<DbSet<Department>>();
			mockSet.As<IQueryable<Department>>().Setup(m => m.Provider).Returns(data.Provider);
			mockSet.As<IQueryable<Department>>().Setup(m => m.Expression).Returns(data.Expression);
			mockSet.As<IQueryable<Department>>().Setup(m => m.ElementType).Returns(data.ElementType);
			mockSet.As<IQueryable<Department>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
			return mockSet;
		}
	}
}