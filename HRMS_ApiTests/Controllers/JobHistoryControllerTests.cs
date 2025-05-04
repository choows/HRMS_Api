using Microsoft.VisualStudio.TestTools.UnitTesting;
using HRMS_Api.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS_Api.Models;
using HRMS_Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HRMS_Api.Controllers.Tests
{
	[TestClass()]
	public class JobHistoryControllerTests
	{
		[TestMethod()]
		public void TestAddJobHistorySuccess()
		{
			//Arrange
			AddJobHistory request = new AddJobHistory
			{
				EmployeeId = 1,
				ManagerId = 2,
				JobId = 3,
				StartDate = DateTime.Now,
				EndDate = DateTime.Now.AddMonths(1),
				Status = "Active",
				Comments = "Test comment"
			};

			Mock<IJobHistoryService> mock = new Mock<IJobHistoryService>();
			mock.Setup(m => m.AddNewJobHistory(request.EmployeeId, request.ManagerId, request.JobId, request.StartDate, request.EndDate, request.Status, request.Comments))
				.Returns(Task.FromResult(true));
			JobHistoryController model = new JobHistoryController(mock.Object);
			
			//Act
			var result = model.Add(request).Result as OkObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(200, result.StatusCode);
		}

		[TestMethod()]
		public void TestAddJobHistoryFail()
		{
			//Arrange
			AddJobHistory request = new AddJobHistory
			{
				EmployeeId = 1,
				ManagerId = 2,
				JobId = 3,
				StartDate = DateTime.Now,
				EndDate = DateTime.Now.AddMonths(1),
				Status = "Active",
				Comments = "Test comment"
			};
			Mock<IJobHistoryService> mock = new Mock<IJobHistoryService>();
			mock.Setup(m => m.AddNewJobHistory(request.EmployeeId, request.ManagerId, request.JobId, request.StartDate, request.EndDate, request.Status, request.Comments))
				.Returns(Task.FromResult(false));
			JobHistoryController model = new JobHistoryController(mock.Object);

			//Act
			var result = model.Add(request).Result as BadRequestObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(400, result.StatusCode);
		}

		[TestMethod()]
		public void TestAddJobHistoryThrowException()
		{
			//Arrange
			AddJobHistory request = new AddJobHistory
			{
				EmployeeId = 1,
				ManagerId = 2,
				JobId = 3,
				StartDate = DateTime.Now,
				EndDate = DateTime.Now.AddMonths(1),
				Status = "Active",
				Comments = "Test comment"
			};
			Mock<IJobHistoryService> mock = new Mock<IJobHistoryService>();
			mock.Setup(m => m.AddNewJobHistory(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>()))
				.Throws(new Exception("Test Exception"));
			JobHistoryController model = new JobHistoryController(mock.Object);

			//Act
			var result = model.Add(request).Result as BadRequestObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(400, result.StatusCode);
		}

		[TestMethod()]
		public void TestGetJobHistoryByPage()
		{
			//Arrange
			Mock<IJobHistoryService> mock = new Mock<IJobHistoryService>();
			mock.Setup(m => m.GetJobHistoryByPage(It.IsAny<int>(),It.IsAny<int?>(), It.IsAny<int?>()))
				.Returns(Task.FromResult(new GetJobHistoryResponse() { JobHistories = new List<JobHistory>(), TotalRecord = 1 }));
			JobHistoryController model = new JobHistoryController(mock.Object);

			//Act
			var result = model.Get(1, 1, 1).Result as OkObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(200, result.StatusCode);
		}

		[TestMethod()]
		public void TestGetJobHistoryByPageAll()
		{
			//Arrange
			Mock<IJobHistoryService> mock = new Mock<IJobHistoryService>();
			mock.Setup(m => m.GetJobHistoryByPage(It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<int?>()))
				.Returns(Task.FromResult(new GetJobHistoryResponse() { JobHistories = new List<JobHistory>(), TotalRecord = 1 }));
			JobHistoryController model = new JobHistoryController(mock.Object);

			//Act
			var result = model.Get(1, null, null).Result as OkObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(200, result.StatusCode);
		}

		[TestMethod()]
		public void TestGetJobHistoryByPageThrowException()
		{
			//Arrange
			Mock<IJobHistoryService> mock = new Mock<IJobHistoryService>();
			mock.Setup(m => m.GetJobHistoryByPage(It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<int?>()))
				.Throws(new Exception("Test Exception"));
			JobHistoryController model = new JobHistoryController(mock.Object);

			//Act
			var result = model.Get(1, 1, 1).Result as BadRequestObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(400, result.StatusCode);
		}

		[TestMethod()]
		public void TestupdateJobHistorySuccess()
		{
			//Arrange
			AddJobHistory request = new AddJobHistory
			{
				EmployeeId = 1,
				ManagerId = 2,
				JobId = 3,
				StartDate = DateTime.Now,
				EndDate = DateTime.Now.AddMonths(1),
				Status = "Active",
				Comments = "Test comment"
			};
			Mock<IJobHistoryService> mock = new Mock<IJobHistoryService>();
			mock.Setup(m => m.UpdateJobHistory(request.EmployeeId, request.ManagerId, request.JobId, request.StartDate, request.EndDate, request.Status, request.Comments))
				.Returns(Task.FromResult(true));
			JobHistoryController model = new JobHistoryController(mock.Object);

			//Act
			var result = model.Update(1, request).Result as OkObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(200, result.StatusCode);
		}

		[TestMethod()]
		public void TestupdateJobHistoryFail()
		{
			//Arrange
			AddJobHistory request = new AddJobHistory
			{
				EmployeeId = 1,
				ManagerId = 2,
				JobId = 3,
				StartDate = DateTime.Now,
				EndDate = DateTime.Now.AddMonths(1),
				Status = "Active",
				Comments = "Test comment"
			};
			Mock<IJobHistoryService> mock = new Mock<IJobHistoryService>();
			mock.Setup(m => m.UpdateJobHistory(request.EmployeeId, request.ManagerId, request.JobId, request.StartDate, request.EndDate, request.Status, request.Comments))
				.Returns(Task.FromResult(false));
			JobHistoryController model = new JobHistoryController(mock.Object);

			//Act
			var result = model.Update(1, request).Result as BadRequestObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(400, result.StatusCode);
		}

		[TestMethod()]
		public void TestUpdateJobHistoryThrowException()
		{
			//Arrange
			AddJobHistory request = new AddJobHistory
			{
				EmployeeId = 1,
				ManagerId = 2,
				JobId = 3,
				StartDate = DateTime.Now,
				EndDate = DateTime.Now.AddMonths(1),
				Status = "Active",
				Comments = "Test comment"
			};

			Mock<IJobHistoryService> mock = new Mock<IJobHistoryService>();

			mock.Setup(m => m.UpdateJobHistory(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>()))
				.Throws(new Exception("Test Exception"));
			JobHistoryController model = new JobHistoryController(mock.Object);

			//Act
			var result = model.Update(1, request).Result as BadRequestObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(400, result.StatusCode);
		}

		[TestMethod()]
		public void TestRemoveJobHistorySuccess()
		{
			//Arrange
			Mock<IJobHistoryService> mock = new Mock<IJobHistoryService>();
			mock.Setup(m => m.RemoveJobHistory(It.IsAny<int>()))
				.Returns(Task.FromResult(true));
			JobHistoryController model = new JobHistoryController(mock.Object);

			//Act
			var result = model.Remove(1).Result as OkObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(200, result.StatusCode);
		}

		[TestMethod()]
		public void TestRemoveJobHistoryThrowException()
		{
			//Arrange
			Mock<IJobHistoryService> mock = new Mock<IJobHistoryService>();
			mock.Setup(m => m.RemoveJobHistory(It.IsAny<int>()))
				.Throws(new Exception("Test Exception"));
			JobHistoryController model = new JobHistoryController(mock.Object);

			//Act
			var result = model.Remove(1).Result as BadRequestObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(400, result.StatusCode);
		}
	}
}