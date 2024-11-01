using Microsoft.EntityFrameworkCore;
using SellingKoi.Data;
using SellingKoi.Models;

namespace SellingKoi.Services
{
    public class TripService : ITripService
    {
        private readonly DataContext _context;
        private readonly IOrderShortenService _orderShortenService;

        public TripService(DataContext context, IOrderShortenService orderShortenService)
        {
            _context = context;
            _orderShortenService = orderShortenService;
        }

        public async Task CreateTripAsync(Trip model)
        {
            if (model.OrderShortensID != null)
            {
                var listorder = await _orderShortenService.SearchOrderList(model.OrderShortensID);
                double price_temp = 0;
                foreach (var item in listorder)
                {
                    price_temp += item.Price;
                }
                model.Price = price_temp.ToString();
            }
            _context.Trips.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Trip>> GetAllTripAsync()
        {
            return await _context.Trips.Include(t => t.SaleStaff).ToListAsync();
        }


        public async Task<Trip> GetTripByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Id cannot be null or empty", nameof(id));
            }

            if (!Guid.TryParse(id, out Guid tripId))
            {
                throw new ArgumentException("Invalid Guid format", nameof(id));
            }

            return await _context.Trips
                .Include(t => t.SaleStaff)
                .FirstOrDefaultAsync(t => t.Id == tripId);
        }

        public async Task UpdateTripAsync(Trip trip)
        {
            _context.Entry(trip).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task UnasignListStaffsAsync(string tripId)
        {
            if (string.IsNullOrEmpty(tripId))
            {
                throw new ArgumentException("Trip ID cannot be null or empty", nameof(tripId));
            }

            var trip = await _context.Trips.FindAsync(Guid.Parse(tripId));
            if (trip == null)
            {
                throw new ArgumentException("Trip not found", nameof(tripId));
            }

            trip.FollowAccountsID = null;
            await _context.SaveChangesAsync();
        }
        public async Task UnasignSaleStaffAsync(string tripId)
        {
            if (string.IsNullOrEmpty(tripId))
            {
                throw new ArgumentException("Trip ID cannot be null or empty", nameof(tripId));
            }

            var trip = await _context.Trips.FindAsync(Guid.Parse(tripId));
            if (trip == null)
            {
                throw new ArgumentException("Trip not found", nameof(tripId));
            }

            trip.SaleStaff = null;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteTripAsync(string id)
        {
            var trip = await _context.Trips.FindAsync(Guid.Parse(id));
            if (trip != null)
            {
                _context.Trips.Remove(trip);
                await _context.SaveChangesAsync();
            }
        }
    }
}
