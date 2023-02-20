using Microsoft.AspNetCore.Mvc;
using Routes.Models.Interfaces;
using Routes.Models.Dto;
using Routes.Models.Entities;

namespace Routes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly IRoutesService _routesService;

        public RoutesController(IRoutesService routesService)
        {
            _routesService = routesService;
        }



        [HttpGet]
        [ProducesResponseType(typeof(IList<RouteModel>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAllRoutes()
        {
            try
            {
                IList<RouteModel> routes = await _routesService.GetAllRoutes();
                if (routes != null && routes.Count > 0)
                {
                    return Ok(routes);
                }
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RouteModel), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetRouteById(int id)
        {
            var route = await _routesService.GetRouteById(id);
            if (route == null)
            {
                return NotFound();
            }
            return Ok(route);
        }

        [HttpGet("best-route/{from}/{to}")]
        [ProducesResponseType(typeof((string, int)), 200)]
        public async Task<IActionResult> GetBestRoute(string from, string to)
        {
            try
            {
                var test = new BestRoute { From = from, To = to };
                (string route, int cost) = await _routesService.GetBestRoute(test);
                string result = $"{route} ao custo de ${cost}";
                return Ok(result);
            } catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(RouteModel), 201)]
        public async Task<IActionResult> CreateRoute([FromBody] RouteDTO newRoute)
        {
            var route = new RouteModel
            {
                From = newRoute.From,
                To = newRoute.To,
                Value = newRoute.Value
            };
            await _routesService.CreateRoute(route);
            return CreatedAtAction(nameof(GetRouteById), new { id = route.Id }, route);
        }

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateRoute([FromBody] RouteModel route)
        {
            var existingRoute = await _routesService.GetRouteById(route.Id);
            if (existingRoute == null)
            {
                return NotFound();
            }
            await _routesService.UpdateRoute(route);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteRoute(int id)
        {
            var route = await _routesService.GetRouteById(id);
            if (route == null)
            {
                return NotFound();
            }
            await _routesService.DeleteRoute(route);
            return NoContent();
        }

    }
}
