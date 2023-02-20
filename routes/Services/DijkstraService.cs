using Routes.Models.Interfaces;

namespace Routes.Services
{
    public class DijkstraService : IDijkstraService
    {
        public async Task<(string route, int cost)> Dijkstra(string start, string end, Dictionary<string, Dictionary<string, int>> routes)
        {
            Dictionary<string, int> distances = new Dictionary<string, int>();
            Dictionary<string, string> previousNodes = new Dictionary<string, string>();
            List<string> unvisitedNodes = new List<string>(routes.Keys);

            foreach (var node in unvisitedNodes)
            {
                distances[node] = int.MaxValue;
            }

            distances[start] = 0;

            while (unvisitedNodes.Count > 0)
            {
                string currentNode = unvisitedNodes.OrderBy(node => distances[node]).First();

                if (currentNode == end)
                {
                    break;
                }

                unvisitedNodes.Remove(currentNode);

                if (routes.ContainsKey(currentNode))
                {
                    foreach (var neighbor in routes[currentNode])
                    {
                        int newCost = distances[currentNode] + neighbor.Value;

                        if (newCost < distances[neighbor.Key])
                        {
                            distances[neighbor.Key] = newCost;
                            previousNodes[neighbor.Key] = currentNode;
                        }
                    }
                }
            }

            List<string> path = new List<string>();
            string current = end;

            while (current != start)
            {
                path.Add(current);
                current = previousNodes[current];
            }

            path.Add(start);
            path.Reverse();

            return (route: string.Join(" - ", path), cost: distances[end]);
        }
    }
}

