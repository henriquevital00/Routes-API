using Routes.Models.Dto;
using Routes.Models.Entities;

namespace Routes.Models.Interfaces
{
    public interface IRoutesService
    {
        Task CreateRoute(RouteModel route);
        Task DeleteRoute(RouteModel route);
        Task<RouteModel> GetRouteById(int id);
        Task<IList<RouteModel>> GetAllRoutes();
        Task UpdateRoute(RouteModel route);
        Task<(string route, int cost)> GetBestRoute(BestRoute bestRoute);
    }
}
