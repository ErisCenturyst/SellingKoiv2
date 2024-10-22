using Microsoft.AspNetCore.Mvc;
using SellingKoi.Models;
using SellingKoi.Services;

namespace SellingKoi.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;   
        }

        [HttpGet]
        public async Task<IActionResult> ShowOrderHaveCreated(string orderid) //by customer
        {
            var order = await _orderService.GetOrderByIdAsync(orderid);
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
            var routeItem = cartItems.FirstOrDefault(item => item.Price != null);
            if (routeItem == null)
            {
                var errorMessage = "không tìm thấy lộ trình trong giỏ hàng.";
                return RedirectToAction("Error", "Home", new { errorMessage });
            }
            //tìm cá trong giả hàng.
            var koiItem = cartItems.ToList().Where(item => item.Price == null);
            if (koiItem == null)
            {
                var errorMessage = "Không tìm thấy cá Koi trong giỏ hàng.";
                return RedirectToAction("Error", "Home", new { errorMessage });

            }
            return View(cartItems);
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateOrder()
        //{
        //    // Lấy thông tin giỏ hàng từ session
        //    var cartItems = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");

        //    if (cartItems == null || !cartItems.Any())
        //    {
        //        ModelState.AddModelError("", "Giỏ hàng trống. Không thể tạo đơn hàng.");
        //        return View(); // Trả về view hiện tại với thông báo lỗi
        //    }

        //    // Tìm lộ trình trong giỏ hàng (giả sử chỉ có một lộ trình)
        //    var routeItem = cartItems.FirstOrDefault(item => item.Price != null);
        //    if (routeItem == null)
        //    {
        //        ModelState.AddModelError("", "Không tìm thấy lộ trình trong giỏ hàng.");
        //        return View(); // Trả về view hiện tại với thông báo lỗi
        //    }

        //    //tìm cá trong giả hàng.
        //    var koiItem = cartItems.ToList().Where(item => item.Price == null);
        //    if(koiItem == null)
        //    {
        //        ModelState.AddModelError("", "Không tìm thấy cá Koi trong giỏ hàng.");
        //        return View(); // Trả về view hiện tại với thông báo lỗi

        //    }

        //    // Tạo đơn hàng mới từ thông tin giỏ hàng
        //    var order = new Order
        //    {
        //        //RouteId = Guid.Parse(routeItem.Id), // Lưu ID của lộ trình
        //        Route =(Models.Route)routeItem,
        //        Kois = (List<KOI>)koiItem
        //    };

        //    if (ModelState.IsValid)
        //    {
        //        await _orderService.CreateOrderAsync(order);
        //        return RedirectToAction(nameof(ShowOrderHaveCreated), new { orderid = order.Id });
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "Có vấn đề với dữ liệu được cung cấp. Vui lòng kiểm tra lại thông tin.");
        //        return View(); // Trả về view hiện tại với thông báo lỗi
        //    }
        //}

    }
}
