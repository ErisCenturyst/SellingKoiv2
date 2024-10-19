using Microsoft.EntityFrameworkCore;
using SellingKoi.Data;
using SellingKoi.Models;

namespace SellingKoi.Services
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _dataContext;

        public OrderService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task CreateOrderAsync(Order order)
        {
            _dataContext.Orders.Add(order);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrder()
        {
            return await _dataContext.Orders.ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrderPaid()
        {
            return await _dataContext.Orders.Where(o => o.Status.Equals("Paid")).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrderRDToStripAsync()
        {
            return await _dataContext.Orders.Where(o => o.Status.Equals("ReadyToTrip")).ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(string id)
        {
            return await _dataContext.Orders.FirstOrDefaultAsync(o => o.Id.ToString().ToUpper().Equals(id));
        }

        public async Task UpdatOrderAsync(Order order)
        {
            _dataContext.Entry(order).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();
        }
    }
}
