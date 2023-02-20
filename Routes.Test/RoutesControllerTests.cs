using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using NuGet.ContentModel;
using NUnit.Framework;
using Routes.Controllers;
using Routes.Models.Dto;
using Routes.Models.Entities;
using Routes.Models.Interfaces;

namespace Routes.Test
{
    [TestFixture]
    internal class RoutesControllerTests
    {
        private RoutesController _routesController;
        private Mock<IRoutesService> _routesServiceMock;

        [SetUp]
        public void Setup()
        {
            _routesServiceMock = new Mock<IRoutesService>();
            _routesController = new RoutesController(_routesServiceMock.Object);
        }

        [Test]
        public async Task GetAllRoutes_ReturnsOkResult_WhenRoutesExist()
        {
            // Arrange
            var routes = new List<RouteModel>
            {
                new RouteModel { Id = 1, From = "GRU", To = "BRC", Value = 10 },
                new RouteModel { Id = 2, From = "BRC", To = "SCL", Value = 5 }
            };
            var task = Task.FromResult<IList<RouteModel>>(routes);
            _routesServiceMock.Setup(s => s.GetAllRoutes()).Returns(task);

            // Act
            var result = await _routesController.GetAllRoutes();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOf<List<RouteModel>>(okResult.Value);
            var returnedRoutes = okResult.Value as List<RouteModel>;
            Assert.That(returnedRoutes.Count, Is.EqualTo(2));
        }


        [Test]
        public async Task GetAllRoutes_ReturnsNotFound_WhenNoRoutesExist()
        {
            // Arrange
            var emptyRoutesList = new List<RouteModel>();
            var task = Task.FromResult<IList<RouteModel>>(emptyRoutesList);
            _routesServiceMock.Setup(s => s.GetAllRoutes()).Returns(task);

            // Act
            var result = await _routesController.GetAllRoutes();

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetRouteById_ReturnsOkResult_WhenRouteExists()
        {
            // Arrange
            var route = new RouteModel { Id = 1, From = "GRU", To = "BRC", Value = 10 };
            _routesServiceMock.Setup(x => x.GetRouteById(route.Id)).ReturnsAsync(route);

            // Act
            var result = await _routesController.GetRouteById(route.Id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(route));
        }

        [Test]
        public async Task GetRouteById_ReturnsNotFound_WhenRouteDoesNotExist()
        {
            // Arrange
            var nonExistentId = 1;
            _routesServiceMock.Setup(x => x.GetRouteById(nonExistentId)).ReturnsAsync((RouteModel)null);

            // Act
            var result = await _routesController.GetRouteById(nonExistentId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetBestRoute_ReturnsOkResult_WhenRouteExists()
        {
            // Arrange
            var from = "GRU";
            var to = "BRC";
            var test = new BestRoute { From = from, To = to };
            var route = "GRU - BRC";
            var cost = 10;
            _routesServiceMock.Setup(x => x.GetBestRoute(test)).ReturnsAsync((route, cost));

            // Act
            var result = await _routesController.GetBestRoute(from, to);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task CreateRoute_Returns_CreatedAtActionResult_With_Route()
        {
            // Arrange
            var newRoute = new RouteDTO { From = "A", To = "B", Value = 10 };
            var route = new RouteModel { Id = 1, From = newRoute.From, To = newRoute.To, Value = newRoute.Value };
            _routesServiceMock.Setup(s => s.CreateRoute(It.IsAny<RouteModel>())).Callback<RouteModel>(r => r.Id = route.Id);
            _routesServiceMock.Setup(s => s.GetRouteById(route.Id)).ReturnsAsync(route);

            // Act
            var result = await _routesController.CreateRoute(newRoute) as CreatedAtActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.ActionName, Is.EqualTo(nameof(_routesController.GetRouteById)));
            Assert.That(result.RouteValues["id"], Is.EqualTo(route.Id));

            var returnedRoute = result.Value as RouteModel;
            Assert.IsNotNull(returnedRoute);
            Assert.That(returnedRoute.Id, Is.EqualTo(route.Id));
            Assert.That(returnedRoute.From, Is.EqualTo(route.From));
            Assert.That(returnedRoute.To, Is.EqualTo(route.To));
            Assert.That(returnedRoute.Value, Is.EqualTo(route.Value));
        }


        [Test]
        public async Task UpdateRoute_With_Valid_Route_Returns_NoContent()
        {
            // Arrange
            var route = new RouteModel { Id = 1, From = "A", To = "B", Value = 10 };
            _routesServiceMock.Setup(s => s.GetRouteById(route.Id)).ReturnsAsync(route);

            // Act
            NoContentResult? result = await _routesController.UpdateRoute(route) as NoContentResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task UpdateRoute_With_Invalid_Route_Returns_NotFound()
        {
            // Arrange
            var route = new RouteModel { Id = 1, From = "A", To = "B", Value = 10 };
            _routesServiceMock.Setup(s => s.GetRouteById(route.Id)).ReturnsAsync((RouteModel)null);

            // Act
            var result = await _routesController.UpdateRoute(route) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public async Task DeleteRoute_With_Valid_Id_Returns_NoContentAsync()
        {
            // Arrange
            var id = 1;
            var route = new RouteModel { Id = id, From = "A", To = "B", Value = 10 };
            _routesServiceMock.Setup(s => s.GetRouteById(id)).ReturnsAsync(route);

            // Act
            var result = await _routesController.DeleteRoute(id) as NoContentResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task DeleteRoute_With_Invalid_Id_Returns_NotFound()
        {
            // Arrange
            var id = 1;
            _routesServiceMock.Setup(s => s.GetRouteById(id)).ReturnsAsync((RouteModel)null);

            // Act
            var result = await _routesController.DeleteRoute(id) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}