using SellingKoi.Models;

namespace SellingKoi.Services
{
    public interface ITripService
    {
        Task<IEnumerable<Trip>> GetAllTripsAsync();
        Task<Trip> GetTripByIdAsync(string id);
        //Task<Guid?> GetIdByNameAsync(string name);
        Task CreateTripAsync(Trip trip);
        Task UpdateTripAsync(Trip trip);
        Task NegateTripAsync(Guid id);
    }              
}
