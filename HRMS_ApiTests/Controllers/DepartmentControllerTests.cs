using Microsoft.VisualStudio.TestTools.UnitTesting;
using HRMS_Api.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS_Api.Services;
using Moq;
using HRMS_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace HRMS_Api.Controllers.Tests
{
	[TestClass()]
	public class DepartmentControllerTests
	{

		[TestMethod()]
		public void TestAddDepartmentSuccess()
		{
			//Arrange
			AddDepartmentModel request = new AddDepartmentModel() { Description = "Sample Description", LocationId = 1, Name = "Test Name" };

			Mock<IDepartmentService> mock = new Mock<IDepartmentService>();
			mock.Setup(m => m.AddNewDepartment(request.Name, request.Description, request.LocationId))
				.Returns(Task.FromResult(true));
			DepartmentController model = new DepartmentController(mock.Object);

			//Act
			var result = model.Add(request).Result as OkObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(200, result.StatusCode);
		}

		[TestMethod()]
		public void TestAddDepartmentFailed()
		{
			//Arrange
			AddDepartmentModel request = new AddDepartmentModel() { Description = "Sample Description", LocationId = 1, Name = "Test Name" };
			Mock<IDepartmentService> mock = new Mock<IDepartmentService>();
			mock.Setup(m => m.AddNewDepartment(request.Name, request.Description, request.LocationId))
				.Returns(Task.FromResult(false));
			DepartmentController model = new DepartmentController(mock.Object);
			
			//Act
			var result = model.Add(request).Result as BadRequestObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(400, result.StatusCode);
		}

		[TestMethod()]
		public void TestAddDepartmentThrowException()
		{
			//Arrange
			Mock<IDepartmentService> mock = new Mock<IDepartmentService>();
			mock.Setup(m => m.AddNewDepartment(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Throws(new Exception("Test Exception"));
			DepartmentController model = new DepartmentController(mock.Object);
			AddDepartmentModel request = new AddDepartmentModel() { Description = "Sample Description", LocationId = 1, Name = "Test Name" };

			//Act
			var result = model.Add(request).Result as BadRequestObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(400, result.StatusCode);
		}

		[TestMethod()]
		public void TestGetDepartmentWithValueSuccess()
		{
			//Arrange
			Mock<IDepartmentService> mock = new Mock<IDepartmentService>();
			mock.Setup(m => m.GetDepartmentByPage(It.IsAny<int?>(), It.IsAny<int?>()))
				.Returns(Task.FromResult(new GetDepartmentResponse() { Departments = new List<Department>(), TotalRecord = 0 }));
			DepartmentController model = new DepartmentController(mock.Object);

			//Act
			var result = model.Get(1, 1).Result as OkObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(200, result.StatusCode);
		}

		[TestMethod()]
		public void TestGetDepartmentWithoutValueSuccess()
		{
			//Arrange
			Mock<IDepartmentService> mock = new Mock<IDepartmentService>();
			mock.Setup(m => m.GetDepartmentByPage(It.IsAny<int?>(), It.IsAny<int?>()))
				.Returns(Task.FromResult(new GetDepartmentResponse() { Departments = new List<Department>(), TotalRecord = 0 }));
			DepartmentController model = new DepartmentController(mock.Object);

			//Act
			var result = model.Get(null,null).Result as OkObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(200, result.StatusCode);
		}

		[TestMethod()]
		public void TestGetDepartmentFailed()
		{
			//Arrange
			Mock<IDepartmentService> mock = new Mock<IDepartmentService>();
			mock.Setup(m => m.GetDepartmentByPage(It.IsAny<int?>(), It.IsAny<int?>()))
				.Throws(new Exception("Test Exception"));
			DepartmentController model = new DepartmentController(mock.Object);

			//Act
			var result = model.Get(null, null).Result as BadRequestObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(400, result.StatusCode);
		}

		[TestMethod()]
		public void TestUpdateDepartmentSuccess()
		{
			//Arrange
			AddDepartmentModel request = new AddDepartmentModel() { Description = "Sample Description", LocationId = 1, Name = "Test Name" };
			Mock<IDepartmentService> mock = new Mock<IDepartmentService>();
			mock.Setup(m => m.UpdateDepartment(1, request.Name, request.Description, request.LocationId))
				.Returns(Task.FromResult(true));
			DepartmentController model = new DepartmentController(mock.Object);
			
			//Act
			var result = model.Update(1, request).Result as OkObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(200, result.StatusCode);
		}

		[TestMethod()]
		public void TestUpdateDepartmentFail()
		{
			//Arrange
			AddDepartmentModel request = new AddDepartmentModel() { Description = "Sample Description", LocationId = 1, Name = "Test Name" };
			Mock<IDepartmentService> mock = new Mock<IDepartmentService>();
			mock.Setup(m => m.UpdateDepartment(1, request.Name, request.Description, request.LocationId))
				.Returns(Task.FromResult(false));
			DepartmentController model = new DepartmentController(mock.Object);

			//Act
			var result = model.Update(1, request).Result as BadRequestObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(400, result.StatusCode);
		}

		[TestMethod()]
		public void TestUpdateDepartmentThrowException()
		{
			//Arrange
			Mock<IDepartmentService> mock = new Mock<IDepartmentService>();
			mock.Setup(m => m.UpdateDepartment(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
				.Throws(new Exception("Test Exception"));
			DepartmentController model = new DepartmentController(mock.Object);
			AddDepartmentModel request = new AddDepartmentModel() { Description = "Sample Description", LocationId = 1, Name = "Test Name" };

			//Act
			var result = model.Update(1, request).Result as BadRequestObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(400, result.StatusCode);
		}

		[TestMethod()]
		public void TestRemoveDepartmentSuccess()
		{
			//Arrange
			Mock<IDepartmentService> mock = new Mock<IDepartmentService>();
			mock.Setup(m => m.RemoveDepartment(It.IsAny<int>()))
				.Returns(Task.FromResult(true));
			DepartmentController model = new DepartmentController(mock.Object);

			//Act
			var result = model.Remove(1).Result as OkObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(200, result.StatusCode);
		}

		[TestMethod()]
		public void TestRemoveDepartmentThrowException()
		{
			//Arrange
			Mock<IDepartmentService> mock = new Mock<IDepartmentService>();
			mock.Setup(m => m.RemoveDepartment(It.IsAny<int>()))
				.Throws(new Exception("Test Exception"));
			DepartmentController model = new DepartmentController(mock.Object);

			//Act
			var result = model.Remove(1).Result as BadRequestObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(400, result.StatusCode);
		}
	}
}