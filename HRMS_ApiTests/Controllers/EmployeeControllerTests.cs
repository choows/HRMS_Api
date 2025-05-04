using Microsoft.VisualStudio.TestTools.UnitTesting;
using HRMS_Api.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using HRMS_Api.Services;
using HRMS_Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_Api.Controllers.Tests
{
	[TestClass()]
	public class EmployeeControllerTests
	{
		[TestMethod()]
		public void TestAddEmployeeSuccess()
		{
			//Arrange
			AddEmployeeModel request = new AddEmployeeModel()
			{
				Name = "John Doe",
				BirthDate = DateTime.Now,
				Phone = "1234567890",
				Address = "123 Main St",
				Email = "1234567890",
				Gender = "Male",
				Status = "Active"
			};
			Mock<IEmployeeService> mock = new Mock<IEmployeeService>();
			mock.Setup(m => m.AddNewEmployee(request.Name, request.BirthDate, request.Phone, request.Email, request.Gender, request.Address , request.Status))
				.Returns(Task.FromResult(true));
			EmployeeController model = new EmployeeController(mock.Object);

			//Act
			var result = model.Add(request).Result as OkObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(200, result.StatusCode);

		}

		[TestMethod()]
		public void TestAddEmployeeFail()
		{
			//Arrange
			AddEmployeeModel request = new AddEmployeeModel()
			{
				Name = "John Doe",
				BirthDate = DateTime.Now,
				Phone = "1234567890",
				Address = "123 Main St",
				Email = "1234567890",
				Gender = "Male",
				Status = "Active"
			};
			Mock<IEmployeeService> mock = new Mock<IEmployeeService>();
			mock.Setup(m => m.AddNewEmployee(request.Name, request.BirthDate, request.Phone, request.Email, request.Gender, request.Address, request.Status))
				.Returns(Task.FromResult(false));
			EmployeeController model = new EmployeeController(mock.Object);

			//Act
			var result = model.Add(request).Result as BadRequestObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(400, result.StatusCode);
		}

		[TestMethod()]
		public void TestAddEmployeeThrowException()
		{
			//Arrange
			Mock<IEmployeeService> mock = new Mock<IEmployeeService>();
			mock.Setup(m => m.AddNewEmployee(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Throws(new Exception("Test exception"));
			EmployeeController model = new EmployeeController(mock.Object);
			AddEmployeeModel request = new AddEmployeeModel()
			{
				Name = "John Doe",
				BirthDate = DateTime.Now,
				Phone = "1234567890",
				Address = "123 Main St",
				Email = "1234567890",
				Gender = "Male",
				Status = "Active"
			};

			//Act
			var result = model.Add(request).Result as BadRequestObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(400, result.StatusCode);
		}

		[TestMethod()]
		public void TestGetEmployeeByPage()
		{
			//Arrange
			Mock<IEmployeeService> mock = new Mock<IEmployeeService>();
			mock.Setup(m => m.GetEmployeeByPage(It.IsAny<int?>(), It.IsAny<int?>()))
				.Returns(Task.FromResult(new GetEmployeeResponse() { Employees = new List<Employee>(), TotalRecord = 1 }));
			EmployeeController model = new EmployeeController(mock.Object);

			//Act
			var result = model.Get(1, 1).Result as OkObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(200, result.StatusCode);
		}

		[TestMethod()]
		public void TestGetEmployeeByPageAll()
		{
			//Arrange
			Mock<IEmployeeService> mock = new Mock<IEmployeeService>();
			mock.Setup(m => m.GetEmployeeByPage(It.IsAny<int?>(), It.IsAny<int?>()))
				.Returns(Task.FromResult(new GetEmployeeResponse() { Employees = new List<Employee>(), TotalRecord = 1 }));
			EmployeeController model = new EmployeeController(mock.Object);

			//Act
			var result = model.Get(null, null).Result as OkObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(200, result.StatusCode);
		}

		[TestMethod()]
		public void TestGetEmployeeByPageThrowException()
		{
			//Arrange
			Mock<IEmployeeService> mock = new Mock<IEmployeeService>();
			mock.Setup(m => m.GetEmployeeByPage(It.IsAny<int?>(), It.IsAny<int?>()))
				.Throws(new Exception("Test exception"));
			EmployeeController model = new EmployeeController(mock.Object);

			//Act
			var result = model.Get(null, null).Result as BadRequestObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(400, result.StatusCode);
		}

		[TestMethod()]
		public void TestUpdateEmployeeSuccess()
		{
			//Arrange
			AddEmployeeModel request = new AddEmployeeModel()
			{
				Name = "John Doe",
				BirthDate = DateTime.Now,
				Phone = "1234567890",
				Address = "123 Main St",
				Email = "1234567890",
				Gender = "Male",
				Status = "Active"
			};
			Mock<IEmployeeService> mock = new Mock<IEmployeeService>();
			mock.Setup(m => m.UpdateEmployee(1, request.Name, request.BirthDate, request.Phone, request.Email, request.Gender, request.Address, request.Status))
				.Returns(Task.FromResult(true));
			EmployeeController model = new EmployeeController(mock.Object);

			//Act
			var result = model.Update(1, request).Result as OkObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(200, result.StatusCode);
		}

		[TestMethod()]
		public void TestUpdateEmployeeFail()
		{
			//Arrange
			AddEmployeeModel request = new AddEmployeeModel()
			{
				Name = "John Doe",
				BirthDate = DateTime.Now,
				Phone = "1234567890",
				Address = "123 Main St",
				Email = "1234567890",
				Gender = "Male",
				Status = "Active"
			};
			Mock<IEmployeeService> mock = new Mock<IEmployeeService>();
			mock.Setup(m => m.UpdateEmployee(1, request.Name, request.BirthDate, request.Phone, request.Email, request.Gender, request.Address, request.Status))
				.Returns(Task.FromResult(false));
			EmployeeController model = new EmployeeController(mock.Object);

			//Act
			var result = model.Update(1, request).Result as BadRequestObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(400, result.StatusCode);
		}

		[TestMethod()]
		public void TestUpdateEmployeeThrowException()
		{
			//Arrange
			Mock<IEmployeeService> mock = new Mock<IEmployeeService>();
			mock.Setup(m => m.UpdateEmployee(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Throws(new Exception("Test exception"));
			EmployeeController model = new EmployeeController(mock.Object);
			AddEmployeeModel request = new AddEmployeeModel()
			{
				Name = "John Doe",
				BirthDate = DateTime.Now,
				Phone = "1234567890",
				Address = "123 Main St",
				Email = "1234567890",
				Gender = "Male",
				Status = "Active"
			};

			//Act
			var result = model.Update(1, request).Result as BadRequestObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(400, result.StatusCode);
		}

		[TestMethod()]
		public void TestRemoveEmployeeSuccess()
		{
			//Arrange
			Mock<IEmployeeService> mock = new Mock<IEmployeeService>();
			mock.Setup(m => m.RemoveEmployee(It.IsAny<int>()))
				.Returns(Task.FromResult(true));
			EmployeeController model = new EmployeeController(mock.Object);

			//Act
			var result = model.Remove(1).Result as OkObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(200, result.StatusCode);
		}

		[TestMethod()]
		public void TestRemoveEmployeeThrowException()
		{
			//Arrange
			Mock<IEmployeeService> mock = new Mock<IEmployeeService>();
			mock.Setup(m => m.RemoveEmployee(It.IsAny<int>()))
				.Throws(new Exception("Test exception"));
			EmployeeController model = new EmployeeController(mock.Object);

			//Act
			var result = model.Remove(1).Result as BadRequestObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(400, result.StatusCode);
		}

		[TestMethod()]
		public void TestSearchEmployeeSuccess()
		{
			//Arrange
			EmployeeSearchCriteriaModel criteria = new EmployeeSearchCriteriaModel()
			{
				name = "John Doe",
				StartDate = null,
				EndDate = null,
				Status = string.Empty,
				CurrentPage = 1,
				PerPage = 10,
				Department = null,
				Gender = string.Empty
			};
			Mock<IEmployeeService> mock = new Mock<IEmployeeService>();
			mock.Setup(m => m.SearchEmployee(criteria.name, criteria.Department, criteria.Status,criteria.Gender, criteria.PerPage, criteria.CurrentPage, criteria.StartDate, criteria.EndDate))
				.Returns(Task.FromResult(new GetEmployeeResponse() { Employees = new List<Employee>(), TotalRecord = 1 }));
			EmployeeController model = new EmployeeController(mock.Object);

			//Act
			var result = model.Search(criteria).Result as OkObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(200, result.StatusCode);
		}

		[TestMethod()]
		public void TestSearchEmployeeThrowException()
		{
			//Arrange
			EmployeeSearchCriteriaModel criteria = new EmployeeSearchCriteriaModel()
			{
				name = "John Doe",
				StartDate = null,
				EndDate = null,
				Status = string.Empty,
				CurrentPage = 1,
				PerPage = 10,
				Department = null,
				Gender = string.Empty
			};
			Mock<IEmployeeService> mock = new Mock<IEmployeeService>();
			mock.Setup(m => m.SearchEmployee(criteria.name, criteria.Department, criteria.Status, criteria.Gender, criteria.PerPage, criteria.CurrentPage, criteria.StartDate, criteria.EndDate))
				.Throws(new NullReferenceException("Test Exception"));
			EmployeeController model = new EmployeeController(mock.Object);

			//Act
			var result = model.Search(criteria).Result as BadRequestObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(400, result.StatusCode);
		}
	}
}