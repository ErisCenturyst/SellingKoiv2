using Microsoft.EntityFrameworkCore;
using SellingKoi.Data;
using SellingKoi.Models;

namespace SellingKoi.Services
{
    public class OrderShortenService : IOrderShortenService
    {
        private readonly DataContext _context;

        public OrderShortenService(DataContext context)
        {
            _context = context;
        }

        public async Task CreateOrderAsync(OrtherShorten order)
        {
            _context.OrtherShortens.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrtherShorten>> GetAllOrder()
        {
            return await _context.OrtherShortens.ToListAsync();
        }
        public async Task<IEnumerable<OrtherShorten>> SearchOrderList(List<string> listorderid)
        {
            return await _context.OrtherShortens
                .Where(order => listorderid.Contains(order.Id.ToString()))
                .ToListAsync();
        }
        public async Task<IEnumerable<OrtherShorten>> GetAllOrderBeingTrip()
        {
            return await _context.OrtherShortens.Where(o => o.Status.Equals("beingtrip")).ToListAsync();
        }

        public async Task<IEnumerable<OrtherShorten>> GetAllOrderDone()
        {
            return await _context.OrtherShortens.Where(o => o.Status.Equals("done")).ToListAsync();
        }

        public async Task<IEnumerable<OrtherShorten>> GetAllOrderPaid()
        {
            return await _context.OrtherShortens.Where(o => o.Status.Equals("paid")).ToListAsync();
        }

        public async Task<IEnumerable<OrtherShorten>> GetAllOrderWaitToShip()
        {
            return await _context.OrtherShortens.Where(o => o.Status.Equals("ongoing")).ToListAsync();
        }

        public async Task<OrtherShorten> GetOrderByIdAsync(string id)
        {
            return await _context.OrtherShortens.FirstOrDefaultAsync(o => o.Id.ToString().ToUpper().Equals(id));
        }

        public async Task UpdatOrderAsync(OrtherShorten order)
        {
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<List<OrtherShorten>> GetOrdersByUser(string username)
        { 
            return await _context.OrtherShortens.Where(o => o.buyer.Equals(username)).ToListAsync();
        }
        public async Task CancelOrderAsync(string id)
        {
            var order = await _context.OrtherShortens.FirstOrDefaultAsync(o => o.Id.ToString().ToUpper().Equals(id));
            order.Status = "cancle";
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task PayOrder(string id)
        {
            var order = await _context.OrtherShortens.FirstOrDefaultAsync(o => o.Id.ToString().ToUpper().Equals(id));
            order.Status = "paid";
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<OrtherShorten>> GetListOrderBelongToStrip(string tripid)
        {
            var orders = await _context.OrtherShortens.Where(o=> o.TripId.Equals(tripid.ToUpper())).ToListAsync();
            return orders;
        }
        public async Task Confirm(string id)
        {
            var order = await _context.OrtherShortens.FirstOrDefaultAsync(o => o.Id.ToString().ToUpper().Equals(id));
            order.Status = "confirmed";
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<OrtherShorten>> GetOrdersByTrip(string tripid)
        {
            return await _context.OrtherShortens.Where(o => o.TripId.Equals(tripid.ToUpper())).ToListAsync();
        }
    }
}
