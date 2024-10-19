
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SellingKoi.Data;
using SellingKoi.Models;

namespace SellingKoi.Services
{
    public class RouteService : IRouteService
    {
        private readonly DataContext _dataContext;
        public RouteService(DataContext dataContext) { _dataContext = dataContext; }
        public async Task CreateRouteAsync(Models.Route route)
        {
            _dataContext.Routes.Add(route);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Models.Route>> GetAllRoutesAsync()
        {
            return await _dataContext.Routes.Include(r => r.Farms).Where(r => r.Status).ToListAsync();
            
        }

        public async Task<Models.Route> GetRouteByIdAsync(string id)
        {
            return await _dataContext.Routes.Include(r => r.Farms).Where(f => f.Status).FirstOrDefaultAsync(f => f.Id.ToString() == id);
        }

        public async Task NegateRouteAsync(Guid id)
        {
            var route = await _dataContext.Routes.FindAsync(id);
            if (route != null)
            {
                try
                {
                    route.Status = false;
                    _dataContext.Routes.Update(route);
                    await _dataContext.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    // Log the exception
                    throw new Exception("An error occurred while deactivating the route.", ex);
                }
            }
            else
            {
                throw new KeyNotFoundException($"Route with ID {id} not found.");
            }
        }


        public async Task UpdateRouteAsync(Models.Route route)
        {
            _dataContext.Entry(route).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();
        }
    }
}
