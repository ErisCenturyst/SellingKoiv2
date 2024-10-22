using SellingKoi.Models;

namespace SellingKoi.Services
{
    public interface IOrderShortenService
    {
        Task<IEnumerable<OrderShorten>> GetAllOrderPaid();
        Task<IEnumerable<OrderShorten>> GetAllOrderBeingTrip();
        Task<IEnumerable<OrderShorten>> GetAllOrderWaitToShip();
        Task<IEnumerable<OrderShorten>> GetAllOrderDone();
        Task<IEnumerable<OrderShorten>> GetAllOrder();
        
        Task<OrderShorten> GetOrderByIdAsync(string id);
        Task CreateOrderAsync(OrderShorten order);
        Task UpdatOrderAsync(OrderShorten order);
    }
}
