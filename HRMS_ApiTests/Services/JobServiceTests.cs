using Microsoft.VisualStudio.TestTools.UnitTesting;
using HRMS_Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS_Api.DatabaseContext;
using HRMS_Api.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.EntityFrameworkCore.Storage;

namespace HRMS_Api.Services.Tests
{
	[TestClass()]
	public class JobServiceTests
	{
		[TestMethod()]
		public void GetjobTest()
		{
			// Arrange
			var mockSet = getMockSet();

			var options = new DbContextOptionsBuilder<HRMSDbContext>()
					   .UseMemoryCache(null)
					   .Options;
			var mockContext = new Mock<HRMSDbContext>(options);
			mockContext.Setup(c => c.Jobs).Returns(mockSet.Object);
			mockContext.Setup(c => c.Database).Returns(new Mock<DatabaseFacade>(mockContext.Object).Object);
			var service = new JobService(mockContext.Object);

			// Act
			var results = service.GetJobByPage(1, 1).Result;

			// Assert
			Assert.AreEqual(3, results.TotalRecord);
			Assert.AreEqual(1, results.Jobs.Count());
			Assert.AreEqual("Job1", results.Jobs.First().Name);

		}

		[TestMethod()]
		public void GetAlljobTest()
		{
			// Arrange
			var mockSet = getMockSet();

			var options = new DbContextOptionsBuilder<HRMSDbContext>()
					   .UseMemoryCache(null)
					   .Options;
			var mockContext = new Mock<HRMSDbContext>(options);
			mockContext.Setup(c => c.Jobs).Returns(mockSet.Object);
			mockContext.Setup(c => c.Database).Returns(new Mock<DatabaseFacade>(mockContext.Object).Object);
			var service = new JobService(mockContext.Object);

			// Act
			var results = service.GetJobByPage(null, null).Result;

			// Assert
			Assert.AreEqual(3, results.TotalRecord);
			Assert.AreEqual(3, results.Jobs.Count());

		}

		[TestMethod()]
		public void UpdateJobTest()
		{
			// Arrange
			var mockSet = getMockSet();
			var newName = "Test 4";
			var options = new DbContextOptionsBuilder<HRMSDbContext>()
				   .UseInMemoryDatabase(Guid.NewGuid().ToString())
				   .Options;

			var mockContext = new Mock<HRMSDbContext>(options);
			var dbFacade = new Mock<DatabaseFacade>(mockContext.Object);

			dbFacade.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
				.Returns(Task.FromResult(new Mock<IDbContextTransaction>().Object));

			mockContext.Setup(c => c.Jobs).Returns(mockSet.Object);
			mockContext.Setup(c => c.Database).Returns(dbFacade.Object);
			mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));
			var service = new JobService(mockContext.Object);

			// Act
			var result = service.UpdateJob(2, newName, "City 2", 1).Result;
			var queryResult = service.GetJobByPage(null, null).Result;
			// Assert
			Assert.IsTrue(result);
			Assert.AreEqual(queryResult.Jobs.FirstOrDefault(x => x.Id == 2).Name, newName);
		}

		private Mock<DbSet<Job>> getMockSet()
		{
			var data = new List<Job>
				{
				new Job
				{
					Id = 1,
					Name = "Job1",
					Description = "Description1",
					DepartmentId = 1,
					CreateDateTime = DateTime.Now,
					UpdateDateTime = DateTime.Now
				},
				new Job
				{
					Id = 2,
					Name = "Job2",
					Description = "Description2",
					DepartmentId = 2,
					CreateDateTime = DateTime.Now,
					UpdateDateTime = DateTime.Now
				},
				new Job
				{
					Id = 3,
					Name = "Job3",
					Description = "Description3",
					DepartmentId = 3,
					CreateDateTime = DateTime.Now,
					UpdateDateTime = DateTime.Now
				}
			}.AsQueryable();

			var newName = "Test 4";

			var mockSet = new Mock<DbSet<Job>>();
			mockSet.As<IQueryable<Job>>().Setup(m => m.Provider).Returns(data.Provider);
			mockSet.As<IQueryable<Job>>().Setup(m => m.Expression).Returns(data.Expression);
			mockSet.As<IQueryable<Job>>().Setup(m => m.ElementType).Returns(data.ElementType);
			mockSet.As<IQueryable<Job>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

			return mockSet;
		}
	}
}