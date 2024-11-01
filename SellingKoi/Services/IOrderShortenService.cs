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
        Task<IEnumerable<OrderShorten>> SearchOrderList(List<string> listorderid);

        Task<OrderShorten> GetOrderByIdAsync(string id);
        Task CreateOrderAsync(OrderShorten order);
        Task UpdatOrderAsync(OrderShorten order);
        Task<List<OrderShorten>> GetOrdersByUser(string username);
        Task CancelOrderAsync(string id);
        Task PayOrder(string id);
        Task<IEnumerable<OrderShorten>> GetListOrderBelongToStrip(string tripid);
    }
}
