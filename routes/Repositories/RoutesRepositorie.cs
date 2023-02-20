using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using Routes.Models.Data;
using Routes.Models.Entities;
using Routes.Models.Interfaces;

namespace Routes.Repositories
{
    public class RoutesRepositorie : IRoutesRepositorie
    {
        private readonly RoutesContext _dbcontext;

        public RoutesRepositorie(RoutesContext _context)
        {
            _dbcontext = _context;
        }

        public async Task CreateRoute(RouteModel route)
        {
            _dbcontext.Add(route);
            _dbcontext.SaveChangesAsync();
        }

        public async Task DeleteRoute(RouteModel route)
        {
            _dbcontext.Remove(route);
            _dbcontext.SaveChangesAsync();
        }

        public Task<RouteModel> GetRouteById(int id)
        {
            return _dbcontext.Routes.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IList<RouteModel>> GetAllRoutes()
        {
            return await _dbcontext.Routes.ToListAsync();
        }

        public async Task UpdateRoute(RouteModel route)
        {
            var existingRoute = _dbcontext.Set<RouteModel>().Find(route.Id);
            if (existingRoute != null)
            {
                _dbcontext.Entry(existingRoute).State = EntityState.Detached;
            }
            _dbcontext.Entry(route).State = EntityState.Modified;
            _dbcontext.SaveChangesAsync();
        }
    }
}
