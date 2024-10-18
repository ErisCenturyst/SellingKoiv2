using Microsoft.EntityFrameworkCore;
using SellingKoi.Data;
using SellingKoi.Models;

namespace SellingKoi.Services
{
    public class TripService : ITripService
    {
        private readonly DataContext _dataContext;

        public TripService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task CreateTripAsync(Trip trip)
        {
            _dataContext.Trips.Add(trip);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Trip>> GetAllTripsAsync()
        {
            return await _dataContext.Trips.ToListAsync();
        }

        public async Task<Trip> GetTripByIdAsync(string id)
        {
            return await _dataContext.Trips.FirstOrDefaultAsync();
        }

        public Task NegateTripAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateTripAsync(Trip trip)
        {
            _dataContext.Entry(trip).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();
        }
    }
}
