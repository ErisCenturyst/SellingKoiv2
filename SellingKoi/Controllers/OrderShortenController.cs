using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SellingKoi.Data;
using SellingKoi.Models;
using SellingKoi.Services;

namespace SellingKoi.Controllers
{
    public class OrderShortenController : Controller
    {
        private readonly IOrderShortenService _orderShortenService;
        private readonly DataContext _datacontext;
        private readonly ITripService _tripService;
        private readonly IRouteService _routeService;
        private readonly IAccountService _accountService;
        public OrderShortenController(IOrderShortenService orderShortenService, ITripService tripService, DataContext datacontext, IRouteService routeService,IAccountService accountService)
        {
            _orderShortenService = orderShortenService;
            _datacontext = datacontext;
            _tripService = tripService;
            _routeService = routeService;
            _accountService = accountService;
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
        public async Task<IActionResult> UnasignOrderFromTrip(string orderid, string tripid)
        {

            var order = await _orderShortenService.GetOrderByIdAsync(orderid);
            string sampletripid = "0";
            if (order == null)
            {
                return BadRequest();
            }
            if (!sampletripid.Equals("0"))
            {
                var trip = await _tripService.GetTripByIdAsync(tripid);
                order.TripId = trip.Id.ToString();
                order.TripNum = trip.TripNum.ToString();

            }
            else
            {
                order.Status = "CancleByAdmin";
                order.TripId = null;
                order.TripNum = null;
            }                                           

            await _orderShortenService.UpdatOrderAsync(order);
                                           
            return RedirectToAction("DetailsTrip", "Trip", new { id = tripid });

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
                order.TripNum = null;
            }
            await _orderShortenService.UpdatOrderAsync(order);
            return RedirectToAction("OrderShortenedManagement");

        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderShorten(string participantName, string phoneNumber, DateTime departureFrom, DateTime departureTo)
        {
            // Lấy thông tin giỏ hàng từ session
            string participantname = participantName;
            string phoneNum = phoneNumber;
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
            //if (!koiItem.Any()) // Kiểm tra xem danh sách có rỗng không
            //{
            //    var errorMessage = "Không tìm thấy cá Koi trong giỏ hàng.";
            //    return RedirectToAction("Error", "Home", new { errorMessage });
            //}
            // Tạo đơn hàng mới từ thông tin giỏ hàng
            var order = new OrderShorten
            {
                routeid = routeItem.Id,
                routename = routeItem.Name,
                koisid = koiItem.Select(koi => koi.Id).ToList(), // Lưu danh sách id Koi
                koisname = koiItem.Select(koi => koi.Name).ToList(), // Lưu danh sách tên Koi
                Price = routeItem.Price,
                buyer = HttpContext.Session.GetString("Username"),
                participants = participantname,
                participantsPhone = phoneNum,
                DepartureFrom = departureFrom, // Gán giá trị cho DepartureFrom
                DepartureTo = departureTo // Gán giá trị cho DepartureTo

            };

            if (ModelState.IsValid)
            {
                await _orderShortenService.CreateOrderAsync(order);
                HttpContext.Session.Remove("Cart"); // Hoặc tên session mà bạn đã sử dụng để lưu giỏ hàng
                HttpContext.Session.Remove("FarmShouldInclude"); // Xóa FarmShouldInclude


                return View(order);
            }
            else
            {
                var errorMessage = "Có lỗi xảy ra trong lúc tọa Order";
                return RedirectToAction("Error", "Home", new { errorMessage });
            }
        }


        [HttpGet]
        public async Task<IActionResult> OrderShortenedManagementSort(string sortOrder)
        {
            ViewBag.RegistrationDateSortParm = String.IsNullOrEmpty(sortOrder) ? "registration_date_desc" : "";
            ViewBag.RouteNameSortParm = sortOrder == "route_name" ? "route_name_desc" : "route_name";
            ViewBag.StatusSortParm = sortOrder == "status" ? "status_desc" : "status";
            ViewBag.DepartureFromSortParm = sortOrder == "departure_from" ? "departure_from_desc" : "departure_from";

            var orders = from o in _datacontext.OrtherShortens
                         select o;

            switch (sortOrder)
            {
                case "registration_date_desc":
                    orders = orders.OrderByDescending(o => o.Registration_date);
                    break;
                case "route_name":
                    orders = orders.OrderBy(o => o.routename);
                    break;
                case "route_name_desc":
                    orders = orders.OrderByDescending(o => o.routename);
                    break;
                case "status":
                    orders = orders.OrderBy(o => o.Status);
                    break;
                case "status_desc":
                    orders = orders.OrderByDescending(o => o.Status);
                    break;
                case "departure_from_desc":
                    orders = orders.OrderByDescending(o => o.DepartureFrom);
                    break;
                default:
                    orders = orders.OrderBy(o => o.Registration_date);
                    break;
            }

            return View("OrderShortenedManagement", await orders.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> DetailsOrder(string id)
        {   
            var order = await _orderShortenService.GetOrderByIdAsync(id);
            if (order.TripId != null)
            {
                var trip = await _tripService.GetTripByIdAsync(order.TripId);
                ViewBag.TripDepartureTime = trip.Date_of_departure;
            } else
            {
                ViewBag.TripDepartureTime = null;
            }
            if (order != null)
            {
                var route = await _routeService.GetRouteByIdAsync(order.routeid.ToString().ToUpper());
                if(route != null)
                {
                    ViewBag.RouteName = route.Name;
                    return View(order);
                } else return RedirectToAction("Profile", "Account");
            } return RedirectToAction("Profile", "Account" );
        }
        [HttpPost]
        public async Task<IActionResult> CancelOrder(string orderId)
        {
            await _orderShortenService.CancelOrderAsync(orderId);
            //var account = _accountService.GetAccountByIdAsync(Guid.Parse(HttpContext.Session.GetString("UserId")));
            return RedirectToAction("DetailsOrder", "OrderShorten", new { id = orderId });

        }
        [HttpPost]
        public async Task<IActionResult> Payment(string orderId)
        {
            await _orderShortenService.PayOrder(orderId);
            //var account = _accountService.GetAccountByIdAsync(Guid.Parse(HttpContext.Session.GetString("UserId")));
            return RedirectToAction("DetailsOrder", "OrderShorten", new { id = orderId });
        }
        [HttpPost]
        public async Task<IActionResult> Confirm(string orderId)
        {
            await _orderShortenService.Confirm(orderId);
            //var account = _accountService.GetAccountByIdAsync(Guid.Parse(HttpContext.Session.GetString("UserId")));
            return RedirectToAction("DetailsOrder", "OrderShorten", new { id = orderId });

        }
        [HttpGet]
        public async Task<IActionResult> SearchOrderByID(string searchId)
        {
            var orders = await _orderShortenService.GetAllOrder();

            if (!string.IsNullOrEmpty(searchId))
            {
                orders = orders.Where(o => o.Id.ToString().Contains(searchId)).ToList();
            }

            ViewBag.TripList = await _tripService.GetAllTripAsync();
            return View("OrderShortenedManagement", orders); // Trả về view OrderShortenedManagement với danh sách đã lọc
        }


    }
}
