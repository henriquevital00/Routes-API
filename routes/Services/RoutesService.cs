using Routes.Models.Dto;
using Routes.Models.Entities;
using Routes.Models.Interfaces;

namespace Routes.Services
{
    public class RoutesService : IRoutesService
    {
        private readonly IRoutesRepositorie _routesRepositorie;
        public readonly IDijkstraService _dijikstraService;

        public RoutesService(IRoutesRepositorie routesRepositorie, IDijkstraService dijikstraService)
        {
            _routesRepositorie = routesRepositorie;
            _dijikstraService = dijikstraService;
        }


        public async Task CreateRoute(RouteModel route)
        {
            await _routesRepositorie.CreateRoute(route);
        }

        public async Task DeleteRoute(RouteModel route)
        {
            await _routesRepositorie.DeleteRoute(route);
        }

        public async Task<RouteModel> GetRouteById(int id)
        {
            return await _routesRepositorie.GetRouteById(id);
        }

        public async Task<IList<RouteModel>> GetAllRoutes()
        {
            return await _routesRepositorie.GetAllRoutes();
        }

        public async Task UpdateRoute(RouteModel route)
        {
            await _routesRepositorie.UpdateRoute(route);
        }

        public async Task<(string route, int cost)> GetBestRoute(BestRoute bestRoute)
        {
            Dictionary<string, Dictionary<string, int>> routes = new Dictionary<string, Dictionary<string, int>>();

            try
            {
                IList<RouteModel> allRoutes = await GetAllRoutes();

                foreach (var route in allRoutes)
                {
                    if (!routes.ContainsKey(route.From))
                    {
                        routes[route.From] = new Dictionary<string, int>();
                    }
                    routes[route.From][route.To] = route.Value;
                }

                foreach (var route in allRoutes)
                {
                    if (!routes.ContainsKey(route.To))
                    {
                        routes[route.To] = new Dictionary<string, int>();
                    }
                }

                if (!routes.ContainsKey(bestRoute.From))
                {
                    throw new Exception($"The source '{bestRoute.From}' does not exist.");
                }

                if (!routes.ContainsKey(bestRoute.To))
                {
                    throw new Exception($"The destination '{bestRoute.To}' does not exist.");
                }


                return await _dijikstraService.Dijkstra(bestRoute.From, bestRoute.To, routes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
