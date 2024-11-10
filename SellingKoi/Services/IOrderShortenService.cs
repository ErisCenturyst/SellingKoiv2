using SellingKoi.Models;

namespace SellingKoi.Services
{
    public interface IOrderShortenService
    {
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
    }
}
