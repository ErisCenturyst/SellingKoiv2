using SellingKoi.Models;

namespace SellingKoi.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrderRDToStripAsync();
        Task<IEnumerable<Order>> GetAllOrder();
        Task<IEnumerable<Order>> GetAllOrderPaid();
        Task<Order> GetOrderByIdAsync(string id);
        Task CreateOrderAsync(Order farm);
        Task UpdatOrderAsync(Order farm);

    }
}
