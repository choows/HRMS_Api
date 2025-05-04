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
	public class EmployeeServiceTests
	{
		[TestMethod()]
		public void GetEmployeeTest()
		{
			// Arrange
			var mockSet = getMockSet();

			var options = new DbContextOptionsBuilder<HRMSDbContext>()
					   .UseMemoryCache(null)
					   .Options;
			var mockContext = new Mock<HRMSDbContext>(options);
			mockContext.Setup(c => c.Employees).Returns(mockSet.Object);
			mockContext.Setup(c => c.Database).Returns(new Mock<DatabaseFacade>(mockContext.Object).Object);
			var service = new EmployeeService(mockContext.Object);

			// Act
			var results = service.GetEmployeeByPage(1, 1).Result;

			// Assert
			Assert.AreEqual(3, results.TotalRecord);
			Assert.AreEqual(1, results.Employees.Count());
			Assert.AreEqual("John Doe", results.Employees.First().Name);
		}

		[TestMethod()]
		public void GetAllEmployeeTest()
		{
			// Arrange
			var mockSet = getMockSet();

			var options = new DbContextOptionsBuilder<HRMSDbContext>()
					   .UseMemoryCache(null)
					   .Options;
			var mockContext = new Mock<HRMSDbContext>(options);
			mockContext.Setup(c => c.Employees).Returns(mockSet.Object);
			mockContext.Setup(c => c.Database).Returns(new Mock<DatabaseFacade>(mockContext.Object).Object);
			var service = new EmployeeService(mockContext.Object);

			// Act
			var results = service.GetEmployeeByPage(null, null).Result;

			// Assert
			Assert.AreEqual(3, results.TotalRecord);
			Assert.AreEqual(3, results.Employees.Count());
			Assert.AreEqual("John Doe", results.Employees.First().Name);
		}

		[TestMethod()]
		public void SearchEmployeeByNameTest()
		{
			// Arrange
			var mockSet = getMockSet();
			var jobHistorySet = getJobHistoryMockSet();
			var jobSet = getJobMockSet();
			var departmentSet = getDepartmentMockSet();

			var options = new DbContextOptionsBuilder<HRMSDbContext>()
					   .UseMemoryCache(null)
					   .Options;
			var mockContext = new Mock<HRMSDbContext>(options);
			mockContext.Setup(c => c.Employees).Returns(mockSet.Object);
			mockContext.Setup(c => c.JobHistories).Returns(jobHistorySet.Object);
			mockContext.Setup(c => c.Jobs).Returns(jobSet.Object);
			mockContext.Setup(c => c.Departments).Returns(departmentSet.Object);
			mockContext.Setup(c => c.Database).Returns(new Mock<DatabaseFacade>(mockContext.Object).Object);
			var service = new EmployeeService(mockContext.Object);

			// Act
			var result = service.SearchEmployee("2", null, string.Empty, string.Empty, null, null, null, null).Result;
			// Assert
			Assert.AreEqual(1, result.TotalRecord);
			Assert.AreEqual(1, result.Employees.Count());
			Assert.AreEqual("John Doe 2", result.Employees.First().Name);
		}

		[TestMethod()]
		public void SearchEmployeeByDepartmentTest()
		{
			// Arrange
			var mockSet = getMockSet();
			var jobHistorySet = getJobHistoryMockSet();
			var jobSet = getJobMockSet();
			var departmentSet = getDepartmentMockSet();

			var options = new DbContextOptionsBuilder<HRMSDbContext>()
					   .UseMemoryCache(null)
					   .Options;
			var mockContext = new Mock<HRMSDbContext>(options);
			mockContext.Setup(c => c.Employees).Returns(mockSet.Object);
			mockContext.Setup(c => c.JobHistories).Returns(jobHistorySet.Object);
			mockContext.Setup(c => c.Jobs).Returns(jobSet.Object);
			mockContext.Setup(c => c.Departments).Returns(departmentSet.Object);
			mockContext.Setup(c => c.Database).Returns(new Mock<DatabaseFacade>(mockContext.Object).Object);
			var service = new EmployeeService(mockContext.Object);

			// Act
			var result = service.SearchEmployee(string.Empty, 1, string.Empty, string.Empty, null, null, null, null).Result;

			// Assert
			Assert.AreEqual(1, result.TotalRecord);
			Assert.AreEqual(1, result.Employees.Count());
			Assert.AreEqual("John Doe", result.Employees.First().Name);
		}

		[TestMethod()]
		public void UpdateEmployeeTest()
		{
			// Arrange
			var mockSet = getMockSet();
			var newName = "John Doe Updated";

			var options = new DbContextOptionsBuilder<HRMSDbContext>()
				   .UseInMemoryDatabase(Guid.NewGuid().ToString())
				   .Options;

			var mockContext = new Mock<HRMSDbContext>(options);
			var dbFacade = new Mock<DatabaseFacade>(mockContext.Object);

			dbFacade.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
				.Returns(Task.FromResult(new Mock<IDbContextTransaction>().Object));

			mockContext.Setup(c => c.Employees).Returns(mockSet.Object);
			mockContext.Setup(c => c.Database).Returns(dbFacade.Object);
			mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));
			var service = new EmployeeService(mockContext.Object);

			// Act


			var result = service.UpdateEmployee(2, newName, DateTime.Now,"City 2","Test","Female", "Country 2", "New Status").Result;
			var queryResult = service.GetEmployeeByPage(null, null).Result;
			// Assert
			Assert.IsTrue(result);
			Assert.AreEqual(queryResult.Employees.FirstOrDefault(x => x.Id == 2).Name, newName);
		}
	
		private Mock<DbSet<Employee>> getMockSet()
		{
			var data = new List<Employee>
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
					new Employee()
					{
						Id = 2,
						Name = "John Doe 2",
						BirthDate = DateTime.Now,
						Phone = "1234567890",
						Email = "test@gmail.com",
						Gender = "Mal",
						Address = "123 Main St",
						Status = "Active"
					},
					new Employee()
					{
						Id = 3,
						Name = "John Doe 3",
						BirthDate = DateTime.Now,
						Phone = "1234567890",
						Email = "test@gmail.com",
						Gender = "Mal",
						Address = "123 Main St",
						Status = "Active"
					}
			}.AsQueryable();
			var mockSet = new Mock<DbSet<Employee>>();
			mockSet.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(data.Provider);
			mockSet.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(data.Expression);
			mockSet.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(data.ElementType);
			mockSet.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
			return mockSet;
		}
		private Mock<DbSet<Job>> getJobMockSet()
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
		private Mock<DbSet<JobHistory>> getJobHistoryMockSet()
		{
			var data = new List<JobHistory>
				{
					new JobHistory(){
						Id = 1,
						EmployeeId = 1,
						ManagerId = 1,
						StartDate = DateTime.Now,
						EndDate = null,
						Status = "active",
						Comments = "Test Comment",
						JobId = 1,
						Job = new Job()
						{
							Id = 1,
							Name = "Job1",
							Description = "Description1",
							DepartmentId = 1,
							CreateDateTime = DateTime.Now,
							UpdateDateTime = DateTime.Now
						}
					},
					new JobHistory(){
						Id = 2,
						EmployeeId = 1,
						ManagerId = 2,
						StartDate = DateTime.Now,
						EndDate = DateTime.Now.AddDays(20),
						Status = "Inactive",
						Comments = "Test Comment 2",
						JobId = 2,
						Job = new Job()
						{
							Id = 2,
							Name = "Job1",
							Description = "Description2",
							DepartmentId = 2,
							CreateDateTime = DateTime.Now,
							UpdateDateTime = DateTime.Now
						}
					},
					new JobHistory(){
						Id = 3,
						EmployeeId = 3,
						ManagerId = 3,
						StartDate = DateTime.Now,
						EndDate = DateTime.Now.AddDays(30),
						Status = "Active",
						Comments = "Test Comment 3",
						JobId = 3,
						Job = new Job()
						{
							Id = 3,
							Name = "Job3",
							Description = "Description3",
							DepartmentId = 3,
							CreateDateTime = DateTime.Now,
							UpdateDateTime = DateTime.Now
						}
					}

				}.AsQueryable();

			var mockSet = new Mock<DbSet<JobHistory>>();
			mockSet.As<IQueryable<JobHistory>>().Setup(m => m.Provider).Returns(data.Provider);
			mockSet.As<IQueryable<JobHistory>>().Setup(m => m.Expression).Returns(data.Expression);
			mockSet.As<IQueryable<JobHistory>>().Setup(m => m.ElementType).Returns(data.ElementType);
			mockSet.As<IQueryable<JobHistory>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
			return mockSet;
		}
		private Mock<DbSet<Department>> getDepartmentMockSet()
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