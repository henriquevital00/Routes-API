namespace Routes.Models.Interfaces
{
    public interface IDijkstraService
    {
        Task<(string route, int cost)>  Dijkstra(string start, string end, Dictionary<string, Dictionary<string, int>> routes);
    }
}
