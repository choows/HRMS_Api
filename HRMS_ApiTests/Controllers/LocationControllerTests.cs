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
	public class LocationControllerTests
	{
		[TestMethod()]
		public void TestAddLocationSuccess()
		{
			//Arrange
			AddLocationModel request = new AddLocationModel()
			{
				Name = "Test Job",
				City = "Test City",
				Country = "Test Country"
			};
			Mock<ILocationService> mock = new Mock<ILocationService>();
			mock.Setup(m => m.AddNewLocation(request.Name, request.City, request.Country))
				.Returns(Task.FromResult(true));
			LocationController model = new LocationController(mock.Object);
			

			//Act
			var result = model.Add(request).Result as OkObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(200, result.StatusCode);
		}

		[TestMethod()]
		public void TestAddLocationFail()
		{
			//Arrange
			AddLocationModel request = new AddLocationModel()
			{
				Name = "Test Job",
				City = "Test City",
				Country = "Test Country"
			};
			Mock<ILocationService> mock = new Mock<ILocationService>();
			mock.Setup(m => m.AddNewLocation(request.Name, request.City, request.Country))
				.Returns(Task.FromResult(false));
			LocationController model = new LocationController(mock.Object);

			//Act
			var result = model.Add(request).Result as BadRequestObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(400, result.StatusCode);
		}

		[TestMethod()]
		public void TestAddLocationThrowException()
		{
			//Arrange
			AddLocationModel request = new AddLocationModel()
			{
				Name = "Test Job",
				City = "Test City",
				Country = "Test Country"
			};
			Mock<ILocationService> mock = new Mock<ILocationService>();
			mock.Setup(m => m.AddNewLocation(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Throws(new Exception("Test Exception"));
			LocationController model = new LocationController(mock.Object);

			//Act
			var result = model.Add(request).Result as BadRequestObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(400, result.StatusCode);
		}

		[TestMethod()]
		public void TestGetLocationByPage()
		{
			//Arrange
			Mock<ILocationService> mock = new Mock<ILocationService>();
			mock.Setup(m => m.GetLocationByPage(It.IsAny<int?>(), It.IsAny<int?>()))
				.Returns(Task.FromResult(new GetLocationResponse() { Locations = new List<Location>(), TotalRecord = 1 }));
			LocationController model = new LocationController(mock.Object);

			//Act
			var result = model.Get(1, 1).Result as OkObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(200, result.StatusCode);
		}

		[TestMethod()]
		public void TestGetLocationByPageAll()
		{
			//Arrange
			Mock<ILocationService> mock = new Mock<ILocationService>();
			mock.Setup(m => m.GetLocationByPage(It.IsAny<int?>(), It.IsAny<int?>()))
				.Returns(Task.FromResult(new GetLocationResponse() { Locations = new List<Location>(), TotalRecord = 1 }));
			LocationController model = new LocationController(mock.Object);

			//Act
			var result = model.Get(null, null).Result as OkObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(200, result.StatusCode);
		}

		[TestMethod()]
		public void TestGetLocationByPageThrowException()
		{
			//Arrange
			Mock<ILocationService> mock = new Mock<ILocationService>();
			mock.Setup(m => m.GetLocationByPage(It.IsAny<int?>(), It.IsAny<int?>()))
				.Throws(new Exception("Test Exception"));
			LocationController model = new LocationController(mock.Object);

			//Act
			var result = model.Get(1, 1).Result as BadRequestObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(400, result.StatusCode);
		}

		[TestMethod()]
		public void TestUpdateLocationSuccess()
		{
			//Arrange
			AddLocationModel request = new AddLocationModel()
			{
				Name = "Test Job",
				City = "Test City",
				Country = "Test Country"
			};
			Mock<ILocationService> mock = new Mock<ILocationService>();
			mock.Setup(m => m.UpdateLocation(1, request.Name, request.City, request.Country))
				.Returns(Task.FromResult(true));
			LocationController model = new LocationController(mock.Object);
			

			//Act
			var result = model.Update(1, request).Result as OkObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(200, result.StatusCode);
		}

		[TestMethod()]
		public void TestUpdateLocationFail()
		{
			//Arrange
			AddLocationModel request = new AddLocationModel()
			{
				Name = "Test Job",
				City = "Test City",
				Country = "Test Country"
			};
			Mock<ILocationService> mock = new Mock<ILocationService>();
			mock.Setup(m => m.UpdateLocation(1, request.Name, request.City, request.Country))
				.Returns(Task.FromResult(false));
			LocationController model = new LocationController(mock.Object);
			

			//Act
			var result = model.Update(1, request).Result as BadRequestObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(400, result.StatusCode);
		}

		[TestMethod()]
		public void TestUpdateLocationThrowException()
		{
			//Arrange
			AddLocationModel request = new AddLocationModel()
			{
				Name = "Test Job",
				City = "Test City",
				Country = "Test Country"
			};
			Mock<ILocationService> mock = new Mock<ILocationService>();
			mock.Setup(m => m.UpdateLocation(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Throws(new Exception("Test Exception"));	
			LocationController model = new LocationController(mock.Object);
			

			//Act
			var result = model.Update(1, request).Result as BadRequestObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(400, result.StatusCode);
		}

		[TestMethod()]
		public void TestRemoveLocationSuccess()
		{
			//Arrange
			Mock<ILocationService> mock = new Mock<ILocationService>();
			mock.Setup(m => m.RemoveLocation(It.IsAny<int>()))
				.Returns(Task.FromResult(true));
			LocationController model = new LocationController(mock.Object);

			//Act
			var result = model.Remove(1).Result as OkObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(200, result.StatusCode);
		}

		[TestMethod()]
		public void TestRemoveLocationThrowException()
		{
			//Arrange
			Mock<ILocationService> mock = new Mock<ILocationService>();
			mock.Setup(m => m.RemoveLocation(It.IsAny<int>()))
				.Throws(new Exception("Test Exception"));
			LocationController model = new LocationController(mock.Object);

			//Act
			var result = model.Remove(1).Result as BadRequestObjectResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(400, result.StatusCode);
		}
	}
}