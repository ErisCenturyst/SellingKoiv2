using SellingKoi.Models;
using Route = SellingKoi.Models.Route;

namespace SellingKoi.Services
{
    public interface IRouteService
    {
        Task<IEnumerable<Route>> GetAllRoutesAsync();
        Task<Route> GetRouteByIdAsync(string id);
        //Task<Guid?> GetIdByNameAsync(string name);
        Task CreateRouteAsync(Route route);
        Task UpdateRouteAsync(Route route);
        Task NegateRouteAsync(Guid id);
    }
}
