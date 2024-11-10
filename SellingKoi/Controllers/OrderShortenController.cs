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
        private readonly ITripService _tripService;
        private readonly IRouteService _routeService;
        private readonly DataContext _context;

        public OrderShortenController(
            IOrderShortenService orderShortenService, 
            ITripService tripService,
            IRouteService routeService,
            DataContext context)
        {
            _orderShortenService = orderShortenService;
            _tripService = tripService;
            _routeService = routeService;
            _context = context;
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
        public async Task<IActionResult> UpdateOrder(string orderId, string tripId)
        {
            var order = await _orderShortenService.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return BadRequest();
            }
            if (!tripId.Equals("0"))
            {
                var trip = await _tripService.GetTripByIdAsync(tripId);
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
            try 
            {
                var cartItems = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
                if (cartItems == null || !cartItems.Any())
                {
                    return RedirectToAction("Index", "Cart");
                }

                var username = HttpContext.Session.GetString("Username");
                if (string.IsNullOrEmpty(username))
                {
                    return RedirectToAction("Login", "Account");
                }

                var order = new OrtherShorten
                {
                    Id = Guid.NewGuid(),
                    participants = participantName,
                    participantsPhone = phoneNumber,
                    DepartureFrom = departureFrom,
                    DepartureTo = departureTo,
                    buyer = username,
                    Status = "booked",
                    Registration_date = DateTime.Now,
                    koisid = new List<string>(),
                    koisname = new List<string>()
                };

                double totalPrice = 0;
                foreach (var item in cartItems)
                {
                    if (item.Price > 0)  // Route
                    {
                        order.routeid = item.Id;
                        order.routename = item.Name;
                        totalPrice += item.Price;
                    }
                    else  // Koi
                    {
                        order.koisid.Add(item.Id);
                        order.koisname.Add(item.Name);
                    }
                }
                order.Price = totalPrice;

                await _orderShortenService.CreateOrderAsync(order);
                HttpContext.Session.Remove("Cart");
                
                return RedirectToAction("DetailsOrder", new { id = order.Id });
            }
            catch (Exception ex)
            {
                // Log the error
                return RedirectToAction("Index", "Cart");
            }
        }

        [HttpGet]
        public async Task<IActionResult> OrderShortenedManagementSort(string sortOrder)
        {
            ViewBag.RegistrationDateSortParm = String.IsNullOrEmpty(sortOrder) ? "registration_date_desc" : "";
            ViewBag.RouteNameSortParm = sortOrder == "route_name" ? "route_name_desc" : "route_name";
            ViewBag.StatusSortParm = sortOrder == "status" ? "status_desc" : "status";
            ViewBag.DepartureFromSortParm = sortOrder == "departure_from" ? "departure_from_desc" : "departure_from";

            var orders = from o in _context.OrtherShortens
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

            var trips = await _tripService.GetAllTripAsync();
            ViewBag.TripList = trips;
            return View("OrderShortenedManagement", await orders.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> DetailsOrder(string id)
        {
            try
            {
                var order = await _orderShortenService.GetOrderByIdAsync(id);
                if (order == null)
                {
                    return NotFound();
                }
                return View(order);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
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
            if (string.IsNullOrEmpty(searchId))
            {
                return RedirectToAction("OrderShortenedManagement");
            }

            var orders = await _orderShortenService.SearchOrderList(new List<string> { searchId });
            var trips = await _tripService.GetAllTripAsync();
            ViewBag.TripList = trips;
            return View("OrderShortenedManagement", orders);
        }


    }
}
