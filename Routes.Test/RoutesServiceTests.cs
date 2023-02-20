using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Routes.Models.Dto;
using Routes.Models.Entities;
using Routes.Models.Interfaces;
using Routes.Services;

namespace Routes.Test
{
    [TestFixture]
    internal class RoutesServiceTests
    {
        private Mock<IRoutesRepositorie> _mockRoutesRepo;
        private Mock<IDijkstraService> _mockDijkstraService;
        private RoutesService _routesService;

        [SetUp]
        public void SetUp()
        {
            _mockRoutesRepo = new Mock<IRoutesRepositorie>();
            _mockDijkstraService = new Mock<IDijkstraService>();
            _routesService = new RoutesService(_mockRoutesRepo.Object, _mockDijkstraService.Object);
        }

        [Test]
        public void CreateRoute_Should_Call_RoutesRepositorie_CreateRoute()
        {
            // Arrange
            var route = new RouteModel { From = "A", To = "B", Value = 10 };

            // Act
            _routesService.CreateRoute(route);

            // Assert
            _mockRoutesRepo.Verify(x => x.CreateRoute(route), Times.Once);
        }

        [Test]
        public void DeleteRoute_Should_Call_RoutesRepositorie_DeleteRoute()
        {
            // Arrange
            var route = new RouteModel { Id = 1, From = "A", To = "B", Value = 10 };

            // Act
            _routesService.DeleteRoute(route);

            // Assert
            _mockRoutesRepo.Verify(x => x.DeleteRoute(route), Times.Once);
        }

        [Test]
        public async Task GetRouteById_Should_Return_RouteModel_With_Correct_Id()
        {
            // Arrange
            var route = new RouteModel { Id = 1, From = "A", To = "B", Value = 10 };
            _mockRoutesRepo.Setup(x => x.GetRouteById(1)).ReturnsAsync(route);

            // Act
            var result = await _routesService.GetRouteById(1);

            // Assert
            Assert.That(result, Is.EqualTo(route));
        }

        [Test]
        public async Task GetAllRoutes_Should_Return_List_Of_RouteModels()
        {
            // Arrange
            var routes = new List<RouteModel>
            {
                new RouteModel { Id = 1, From = "A", To = "B", Value = 10 },
                new RouteModel { Id = 2, From = "B", To = "C", Value = 5 },
                new RouteModel { Id = 3, From = "A", To = "C", Value = 15 }
            };
            _mockRoutesRepo.Setup(x => x.GetAllRoutes()).ReturnsAsync(routes);

            // Act
            var result = await _routesService.GetAllRoutes();

            // Assert
            Assert.That(result, Is.EqualTo(routes));
        }

        [Test]
        public void UpdateRoute_Should_Call_RoutesRepositorie_UpdateRoute()
        {
            // Arrange
            var route = new RouteModel { Id = 1, From = "A", To = "B", Value = 10 };

            // Act
            _routesService.UpdateRoute(route);

            // Assert
            _mockRoutesRepo.Verify(x => x.UpdateRoute(route), Times.Once);
        }

        [Test]
        public async Task GetBestRoute_Returns_CorrectResult()
        {
            // Arrange
            var dijkstraServiceMock = new Mock<IDijkstraService>();
            dijkstraServiceMock.Setup(ds => ds.Dijkstra("A", "B", It.IsAny<Dictionary<string, Dictionary<string, int>>>()))
                .ReturnsAsync(("A -> B", 10));
            var routesRepositorieMock = new Mock<IRoutesRepositorie>();
            routesRepositorieMock.Setup(rr => rr.GetAllRoutes())
                .ReturnsAsync(new List<RouteModel>
                {
                    new RouteModel { Id = 1, From = "A", To = "B", Value = 10 },
                    new RouteModel { Id = 2, From = "B", To = "C", Value = 5 },
                    new RouteModel { Id = 3, From = "C", To = "D", Value = 3 },
                });
            var routesService = new RoutesService(routesRepositorieMock.Object, dijkstraServiceMock.Object);

            // Act
            var result = await routesService.GetBestRoute(new BestRoute { From = "A", To = "B" });

            // Assert
            Assert.That(result.route, Is.EqualTo("A -> B"));
            Assert.That(result.cost, Is.EqualTo(10));
        }

    }
}
