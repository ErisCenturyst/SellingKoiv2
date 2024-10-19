using Microsoft.AspNetCore.Mvc;
using SellingKoi.Models;
using SellingKoi.Services;
using System.Collections.Immutable;

namespace SellingKoi.Controllers
{
    public class OrderShortenController : Controller
    {
        private readonly IOrderShortenService _orderShortenService;

        public OrderShortenController(IOrderShortenService orderShortenService)
        {
            _orderShortenService = orderShortenService;
        }
        [HttpGet]
        public async Task<IActionResult> ShowOrderHaveCreated(string orderid) //by customer
        {
            var order = await _orderShortenService.GetOrderByIdAsync(orderid);
            if (order == null)
            {
                return NotFound("No order are found !");
            }
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderShorten()
        {
            // Lấy thông tin giỏ hàng từ session
            var cartItems = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
            if (cartItems == null || !cartItems.Any())
            {
                var errorMessage = "Giỏ hàng trống. Không thể tạo đơn hàng.";
                return RedirectToAction("Error", "Home", new { errorMessage });
            }

            // Tìm lộ trình trong giỏ hàng (giả sử chỉ có một lộ trình)
            var routeItem = cartItems.FirstOrDefault(item => item.Price != 0);
            if (routeItem == null)
            {
                var errorMessage = "không tìm thấy lộ trình trong giỏ hàng.";
                return RedirectToAction("Error", "Home", new { errorMessage });
            }
            //tìm cá trong giỏ hàng.
            var koiItem = cartItems.Where(item => item.Price == 0).ToList(); // Chuyển đổi thành danh sách
            if (!koiItem.Any()) // Kiểm tra xem danh sách có rỗng không
            {
                var errorMessage = "Không tìm thấy cá Koi trong giỏ hàng.";
                return RedirectToAction("Error", "Home", new { errorMessage });
            }
            // Tạo đơn hàng mới từ thông tin giỏ hàng
            var order = new OrderShorten
            {
                routeid = routeItem.Id,
                routename = routeItem.Name,
                koisid = koiItem.Select(koi => koi.Id).ToList(), // Lưu danh sách id Koi
                koisname = koiItem.Select(koi => koi.Name).ToList() // Lưu danh sách tên Koi

            };
            if (ModelState.IsValid)
            {
                await _orderShortenService.CreateOrderAsync(order);
                //return RedirectToAction(nameof(ShowOrderHaveCreated), new { orderid = order.Id });
                return View(order);
            }
            else
            {
                var errorMessage = "Có lỗi xảy ra trong lúc tọa Order";
                return RedirectToAction("Error", "Home", new { errorMessage });
            }
        }

    }
}
