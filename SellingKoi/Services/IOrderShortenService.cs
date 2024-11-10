using SellingKoi.Models;

namespace SellingKoi.Services
{
    public interface IOrderShortenService
    {
<<<<<<< HEAD
        Task<IEnumerable<OrtherShorten>> GetAllOrderPaid();
        Task<IEnumerable<OrtherShorten>> GetAllOrderBeingTrip();
        Task<IEnumerable<OrtherShorten>> GetAllOrderWaitToShip();
        Task<IEnumerable<OrtherShorten>> GetAllOrderDone();
        Task<IEnumerable<OrtherShorten>> GetAllOrder();
        Task<IEnumerable<OrtherShorten>> SearchOrderList(List<string> listorderid);

        Task<OrtherShorten> GetOrderByIdAsync(string id);
        Task CreateOrderAsync(OrtherShorten order);
        Task UpdatOrderAsync(OrtherShorten order);
        Task<List<OrtherShorten>> GetOrdersByUser(string username);
        Task CancelOrderAsync(string id);
        Task PayOrder(string id);
        Task<IEnumerable<OrtherShorten>> GetListOrderBelongToStrip(string tripid);
        Task Confirm(string id);
        Task<IEnumerable<OrtherShorten>> GetOrdersByTrip(string tripid);
=======
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
        Task Confirm(string id);
        Task<IEnumerable<OrderShorten>> GetOrdersByTrip(string tripid);
>>>>>>> 85c932f9196067835fd31f943dac733028fd05c6
    }
}
