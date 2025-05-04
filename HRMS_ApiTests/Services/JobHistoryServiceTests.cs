using Microsoft.VisualStudio.TestTools.UnitTesting;
using HRMS_Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS_Api.DatabaseContext;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Moq;
using HRMS_Api.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace HRMS_Api.Services.Tests
{
	[TestClass()]
	public class JobHistoryServiceTests
	{
		[TestMethod()]
		public void GetJobHistoryTest()
		{
			// Arrange
			var mockSet = getMockSet();
			var options = new DbContextOptionsBuilder<HRMSDbContext>()
					   .UseMemoryCache(null)
					   .Options;
			var mockContext = new Mock<HRMSDbContext>(options);
			mockContext.Setup(c => c.JobHistories).Returns(mockSet.Object);
			mockContext.Setup(c => c.Database).Returns(new Mock<DatabaseFacade>(mockContext.Object).Object);
			var service = new JobHistoryService(mockContext.Object);

			// Act
			var results = service.GetJobHistoryByPage(1, 1, 1).Result;

			// Assert
			Assert.AreEqual(2, results.TotalRecord);
			Assert.AreEqual(1, results.JobHistories.Count());
		}

		[TestMethod()]
		public void GetAllJobHistoryTest()
		{
			// Arrange
			var mockSet = getMockSet();

			var options = new DbContextOptionsBuilder<HRMSDbContext>()
					   .UseMemoryCache(null)
					   .Options;
			var mockContext = new Mock<HRMSDbContext>(options);
			mockContext.Setup(c => c.JobHistories).Returns(mockSet.Object);
			mockContext.Setup(c => c.Database).Returns(new Mock<DatabaseFacade>(mockContext.Object).Object);
			var service = new JobHistoryService(mockContext.Object);

			// Act
			var results = service.GetJobHistoryByPage(1, null, null).Result;

			// Assert
			Assert.AreEqual(2, results.TotalRecord);
			Assert.AreEqual(2, results.JobHistories.Count());
		}

		[TestMethod()]
		public void UpdateJobHistoryTest()
		{
			// Arrange
			var mockSet = getMockSet();

			#region Mock Employee Data
			var Empdata = new List<Employee>
				{
					new Employee()
					{
						Id = 1,
						Name = "John Doe",
						BirthDate = DateTime.Now,
						Phone = "1234567890",
						Email = "test@gmail.com",
						Gender = "Mal",
						Address = "123 Main St",
						Status = "Active"
					},
			}.AsQueryable();
			var employeemockSet = new Mock<DbSet<Employee>>();
			employeemockSet.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(Empdata.Provider);
			employeemockSet.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(Empdata.Expression);
			employeemockSet.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(Empdata.ElementType);
			employeemockSet.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(() => Empdata.GetEnumerator());

			#endregion
			#region Mock Job Data

			var Jobdata = new List<Job>
				{
				new Job
				{
					Id = 1,
					Name = "Job1",
					Description = "Description1",
					DepartmentId = 1,
					CreateDateTime = DateTime.Now,
					UpdateDateTime = DateTime.Now
				}
			}.AsQueryable();
			var JobmockSet = new Mock<DbSet<Job>>();
			JobmockSet.As<IQueryable<Job>>().Setup(m => m.Provider).Returns(Jobdata.Provider);
			JobmockSet.As<IQueryable<Job>>().Setup(m => m.Expression).Returns(Jobdata.Expression);
			JobmockSet.As<IQueryable<Job>>().Setup(m => m.ElementType).Returns(Jobdata.ElementType);
			JobmockSet.As<IQueryable<Job>>().Setup(m => m.GetEnumerator()).Returns(() => Jobdata.GetEnumerator());
			#endregion

			var options = new DbContextOptionsBuilder<HRMSDbContext>()
				   .UseInMemoryDatabase(Guid.NewGuid().ToString())
				   .Options;

			var mockContext = new Mock<HRMSDbContext>(options);
			var dbFacade = new Mock<DatabaseFacade>(mockContext.Object);

			dbFacade.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
				.Returns(Task.FromResult(new Mock<IDbContextTransaction>().Object));

			mockContext.Setup(c => c.JobHistories).Returns(mockSet.Object);
			mockContext.Setup(c => c.Employees).Returns(employeemockSet.Object);
			mockContext.Setup(c => c.Jobs).Returns(JobmockSet.Object);
			mockContext.Setup(c => c.Database).Returns(dbFacade.Object);
			mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));
			var service = new JobHistoryService(mockContext.Object);

			// Act
			var result = service.UpdateJobHistory(3, 1, 1, DateTime.Now, null, "Status", "Comments").Result;
			var queryResult = service.GetJobHistoryByPage(3, null, null).Result;

			// Assert
			Assert.IsTrue(result);
			Assert.AreEqual(queryResult.JobHistories.FirstOrDefault(x => x.Id == 3).Status, "Status");
		}

		private Mock<DbSet<JobHistory>> getMockSet()
		{
			var data = new List<JobHistory>
				{
					new JobHistory(){
						Id = 1,
						EmployeeId = 1,
						ManagerId = 1,
						StartDate = DateTime.Now,
						EndDate = DateTime.Now.AddDays(10),
						Status = "Active",
						Comments = "Test Comment",
						JobId = 1
					},
					new JobHistory(){
						Id = 2,
						EmployeeId = 1,
						ManagerId = 2,
						StartDate = DateTime.Now,
						EndDate = DateTime.Now.AddDays(20),
						Status = "Inactive",
						Comments = "Test Comment 2",
						JobId = 2
					},
					new JobHistory(){
						Id = 3,
						EmployeeId = 3,
						ManagerId = 3,
						StartDate = DateTime.Now,
						EndDate = DateTime.Now.AddDays(30),
						Status = "Active",
						Comments = "Test Comment 3",
						JobId = 3
					}

				}.AsQueryable();

			var mockSet = new Mock<DbSet<JobHistory>>();
			mockSet.As<IQueryable<JobHistory>>().Setup(m => m.Provider).Returns(data.Provider);
			mockSet.As<IQueryable<JobHistory>>().Setup(m => m.Expression).Returns(data.Expression);
			mockSet.As<IQueryable<JobHistory>>().Setup(m => m.ElementType).Returns(data.ElementType);
			mockSet.As<IQueryable<JobHistory>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
			return mockSet;
		}
	}
}