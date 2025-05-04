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
	public class JobControllerTests
	{
		[TestMethod()]
		public void TestAddJobSuccess()
		{
			//Arrange
			AddJobModel request = new AddJobModel()
			{
				Name = "Test Job",
				Description = "Test Description",
				DepartmentId = 1
			};
			Mock<IJobService> mock = new Mock<IJobService>();
			mock.Setup(m => m.AddNewJob(request.Name, request.Description, request.DepartmentId))
				.Returns(Task.FromResult(true));
			JobController model = new JobController(mock.Object);

			//Act
			var result = model.Add(request).Result as OkObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(200, result.StatusCode);

		}

		[TestMethod()]
		public void TestAddJobFail()
		{
			//Arrange
			AddJobModel request = new AddJobModel()
			{
				Name = "Test Job",
				Description = "Test Description",
				DepartmentId = 1
			};
			Mock<IJobService> mock = new Mock<IJobService>();
			mock.Setup(m => m.AddNewJob(request.Name, request.Description, request.DepartmentId))
				.Returns(Task.FromResult(false));
			JobController model = new JobController(mock.Object);

			//Act
			var result = model.Add(request).Result as BadRequestObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(400, result.StatusCode);

		}

		[TestMethod()]
		public void TestAddJobThrowException()
		{
			//Arrange
			AddJobModel request = new AddJobModel()
			{
				Name = "Test Job",
				Description = "Test Description",
				DepartmentId = 1
			};
			Mock<IJobService> mock = new Mock<IJobService>();
			mock.Setup(m => m.AddNewJob(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
				.Throws(new Exception("Test Exception"));
			JobController model = new JobController(mock.Object);

			//Act
			var result = model.Add(request).Result as BadRequestObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(400, result.StatusCode);
		}

		[TestMethod()]
		public void TestGetJobByPage()
		{
			//Arrange
			Mock<IJobService> mock = new Mock<IJobService>();
			mock.Setup(m => m.GetJobByPage(It.IsAny<int?>(), It.IsAny<int?>()))
				.Returns(Task.FromResult(new GetJobResponse() { Jobs = new List<Job>(), TotalRecord = 1 }));
			JobController model = new JobController(mock.Object);

			//Act
			var result = model.Get(1, 1).Result as OkObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(200, result.StatusCode);
		}

		[TestMethod()]
		public void TestGetJobByPageAll()
		{
			//Arrange
			Mock<IJobService> mock = new Mock<IJobService>();
			mock.Setup(m => m.GetJobByPage(It.IsAny<int?>(), It.IsAny<int?>()))
				.Returns(Task.FromResult(new GetJobResponse() { Jobs = new List<Job>(), TotalRecord = 1 }));
			JobController model = new JobController(mock.Object);

			//Act
			var result = model.Get(null, null).Result as OkObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(200, result.StatusCode);
		}

		[TestMethod()]
		public void TestGetJobByPageThrowException()
		{
			//Arrange
			Mock<IJobService> mock = new Mock<IJobService>();
			mock.Setup(m => m.GetJobByPage(It.IsAny<int?>(), It.IsAny<int?>()))
				.Throws(new Exception("Test Exception"));	
			JobController model = new JobController(mock.Object);

			//Act
			var result = model.Get(1, 1).Result as BadRequestObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(400, result.StatusCode);
		}

		[TestMethod()]
		public void TestUpdateJobSuccess()
		{
			//Arrange
			AddJobModel request = new AddJobModel()
			{
				Name = "Test Job",
				Description = "Test Description",
				DepartmentId = 1
			};
			Mock<IJobService> mock = new Mock<IJobService>();
			mock.Setup(m => m.UpdateJob(1, request.Name, request.Description, request.DepartmentId))
				.Returns(Task.FromResult(true));
			JobController model = new JobController(mock.Object);

			//Act
			var result = model.Update(1, request).Result as OkObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(200, result.StatusCode);
		}

		[TestMethod()]
		public void TestUpdateJobFail()
		{
			//Arrange
			AddJobModel request = new AddJobModel()
			{
				Name = "Test Job",
				Description = "Test Description",
				DepartmentId = 1
			};
			Mock<IJobService> mock = new Mock<IJobService>();
			mock.Setup(m => m.UpdateJob(1, request.Name, request.Description, request.DepartmentId))
				.Returns(Task.FromResult(false));
			JobController model = new JobController(mock.Object);

			//Act
			var result = model.Update(1, request).Result as BadRequestObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(400, result.StatusCode);
		}

		[TestMethod()]
		public void TestUpdateJobThrowException()
		{
			//Arrange
			Mock<IJobService> mock = new Mock<IJobService>();
			mock.Setup(m => m.UpdateJob(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
				.Throws(new Exception("Test Exception"));
			JobController model = new JobController(mock.Object);
			AddJobModel request = new AddJobModel()
			{
				Name = "Test Job",
				Description = "Test Description",
				DepartmentId = 1
			};

			//Act
			var result = model.Update(1, request).Result as BadRequestObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(400, result.StatusCode);
		}

		[TestMethod()]
		public void TestRemoveJobSuccess()
		{
			//Arrange
			Mock<IJobService> mock = new Mock<IJobService>();
			mock.Setup(m => m.RemoveJob(It.IsAny<int>()))
				.Returns(Task.FromResult(true));
			JobController model = new JobController(mock.Object);

			//Act
			var result = model.Remove(1).Result as OkObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(200, result.StatusCode);
		}

		[TestMethod()]
		public void TestRemoveJobThrowException()
		{
			//Arrange
			Mock<IJobService> mock = new Mock<IJobService>();
			mock.Setup(m => m.RemoveJob(It.IsAny<int>()))
				.Throws(new Exception("Test Exception"));
			JobController model = new JobController(mock.Object);

			//Act
			var result = model.Remove(1).Result as BadRequestObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(400, result.StatusCode);
		}
	}
}