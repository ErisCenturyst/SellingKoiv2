using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SellingKoi.Data;
using SellingKoi.Models;
using SellingKoi.Services;
using System.Collections.Immutable;

namespace SellingKoi.Controllers
{
    public class OrderShortenController : Controller
    {
        private readonly IOrderShortenService _orderShortenService;
        private readonly DataContext _datacontext;
        private readonly ITripService _tripService;

        public OrderShortenController(IOrderShortenService orderShortenService,ITripService tripService,DataContext datacontext)
        {
            _orderShortenService = orderShortenService;
            _datacontext = datacontext;
            _tripService = tripService;
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

        [HttpGet]
        public async Task<IActionResult> OrderShortenedManagement()
        {
            var models = await _orderShortenService.GetAllOrder();
            var trips = await _tripService.GetAllTripAsync();
            if (models == null)
            {
                return NotFound("No Order are found !");
            }
            ViewBag.TripList = trips;
            return View(models);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOrder(string orderid, string tripid)
        {

            var order = await _orderShortenService.GetOrderByIdAsync(orderid);
            if (order == null)
            {
                return BadRequest();
            }
            if (!tripid.Equals("0"))
            {
                var trip = await _tripService.GetTripByIdAsync(tripid);
                order.TripId = trip.Id.ToString();
                order.TripNum = trip.TripNum.ToString();

            }
            else
            {
                order.TripId = null;
            }
            await _orderShortenService.UpdatOrderAsync(order);
            return RedirectToAction("OrderShortenedManagement");

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
                koisname = koiItem.Select(koi => koi.Name).ToList(), // Lưu danh sách tên Koi
                Price = routeItem.Price,
                buyer = HttpContext.Session.GetString("Username")

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


        [HttpGet]
        public IActionResult OrderShortenedManagementSort(string sortOrder)
        {
            ViewBag.OrderDateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewBag.RouteNameSortParm = sortOrder == "route" ? "route_desc" : "route";
            ViewBag.StatusSortParm = sortOrder == "status" ? "status_desc" : "status";

            var orders = _datacontext.OrtherShortens.AsQueryable();

            switch (sortOrder)
            {
                case "date_desc":
                    orders = orders.OrderByDescending(o => o.Registration_date);
                    break;
                case "route":
                    orders = orders.OrderBy(o => o.routename);
                    break;
                case "route_desc":
                    orders = orders.OrderByDescending(o => o.routename);
                    break;
                case "status":
                    orders = orders.OrderBy(o => o.Status);
                    break;
                case "status_desc":
                    orders = orders.OrderByDescending(o => o.Status);
                    break;
                default:
                    orders = orders.OrderBy(o => o.Registration_date);
                    break;
            }

            //return View(orders.ToList());
            return RedirectToAction("OrderShortenedManagement", "OrderShorten", new { sortedOrders = orders.ToList() });

        }

    }
}
