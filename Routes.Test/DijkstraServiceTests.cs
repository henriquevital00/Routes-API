using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Routes.Models.Interfaces;
using Routes.Services;

namespace Routes.Test
{
    internal class DijkstraServiceTests
    {
        private IDijkstraService _dijkstraService;

        [SetUp]
        public void Setup()
        {
            _dijkstraService = new DijkstraService();
        }

        [Test]
        [TestCase("A", "D", "A - B - C - D", 4)]
        [TestCase("A", "B", "A - B", 2)]
        [TestCase("A", "C", "A - B - C", 3)]
        public async Task Dijkstra_ReturnsCorrectRouteAndCost_WhenGivenValidInput(string start, string end, string expectedRoute, int expectedCost)
        {
            // Arrange
            var routes = new Dictionary<string, Dictionary<string, int>>
            {
                { "A", new Dictionary<string, int> { { "B", 2 }, { "C", 4 } } },
                { "B", new Dictionary<string, int> { { "C", 1 }, { "D", 5 } } },
                { "C", new Dictionary<string, int> { { "D", 1 } } },
                { "D", new Dictionary<string, int>() }
            };

            // Act
            var result = await _dijkstraService.Dijkstra(start, end, routes);

            // Assert
            Assert.That(result.route, Is.EqualTo(expectedRoute));
            Assert.That(result.cost, Is.EqualTo(expectedCost));
        }
    }
}
