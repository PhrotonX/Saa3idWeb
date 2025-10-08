using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Saa3idWeb.Controllers;
using Saa3idWeb.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saa3idWeb.Controllers.Tests
{
	[TestClass()]
	public class EmergenciesControllerTests
	{
		[TestMethod()]
		public void IndexAPITest()
		{
			//Arrange
			var mockContext = new Mock<ApplicationDbContext>();
			var controller = new EmergenciesController(mockContext.Object);
			//Act
			var result = controller.IndexAPI();
			//Assert
			Assert.IsTrue(result.IsCompleted);
			//Assert.Fail();
		}

		//[TestMethod()]
		//public void DeleteConfirmedApiTest()
		//{
		//	//Arrange
		//	var mockContext = new Mock<ApplicationDbContext>();
		//	var controller = new EmergenciesController(mockContext.Object);
		//	//Act
		//	controller.DeleteConfirmedApi(1);
		//	//Assert
		//	Assert.Fail();
		//}
	}
}