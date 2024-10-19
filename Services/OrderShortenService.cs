using Microsoft.EntityFrameworkCore;
using SellingKoi.Data;
using SellingKoi.Models;

namespace SellingKoi.Services
{
    public class OrderShortenService : IOrderShortenService
    {
        private readonly DataContext _dataContext;

        public OrderShortenService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task CreateOrderAsync(OrderShorten order)
        {
            _dataContext.OrtherShortens.Add(order);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderShorten>> GetAllOrder()
        {
            return await _dataContext.OrtherShortens.ToListAsync();
        }

        public async Task<IEnumerable<OrderShorten>> GetAllOrderBeingTrip()
        {
            return await _dataContext.OrtherShortens.Where(o => o.Status.Equals("beingtrip")).ToListAsync();
        }

        public async Task<IEnumerable<OrderShorten>> GetAllOrderDone()
        {
            return await _dataContext.OrtherShortens.Where(o => o.Status.Equals("done")).ToListAsync();
        }

        public async Task<IEnumerable<OrderShorten>> GetAllOrderPaid()
        {
            return await _dataContext.OrtherShortens.Where(o => o.Status.Equals("paid")).ToListAsync();
        }

        public async Task<IEnumerable<OrderShorten>> GetAllOrderWaitToShip()
        {
            return await _dataContext.OrtherShortens.Where(o => o.Status.Equals("waittoship")).ToListAsync();
        }

        public async Task<OrderShorten> GetOrderByIdAsync(string id)
        {
            return await _dataContext.OrtherShortens.FirstOrDefaultAsync(o => o.Id.ToString().ToUpper().Equals(id));
        }

        public Task UpdatOrderAsync(OrderShorten order)
        {
            throw new NotImplementedException();
        }

    }
}
