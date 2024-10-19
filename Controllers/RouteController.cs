using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SellingKoi.Data;
using SellingKoi.Models;
using SellingKoi.Services;
using Route = SellingKoi.Models.Route;

namespace SellingKoi.Controllers
{
    public class RouteController : Controller
    {
        private readonly IRouteService _routeService;
        private readonly IFarmService _farmService; // Dịch vụ để lấy Farm
        private readonly DataContext _dataContext;

        public RouteController(IRouteService routeService, IFarmService farmService,DataContext dataContext )
        {
            _routeService = routeService;
            _farmService = farmService;
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<IActionResult> RouteShopping()
        {
            var routes = await _routeService.GetAllRoutesAsync();

            if (routes == null)
            {
                return NotFound("No route are found !");
            }
            return View(routes);
        }



        [HttpGet]
        public async Task<IActionResult> RouteManagement()
        {
            var routes = await _routeService.GetAllRoutesAsync();
            if (routes == null)
            {
                return NotFound("No farm are found !");
            }
            return View(routes);
        }
        [HttpGet]
        public async Task<IActionResult> DetailsRoute(Guid id)
        {

            if (id == null)
            {
                return NotFound($"Route with id '{id}' not found.");
            }

            var route = await _routeService.GetRouteByIdAsync(id.ToString());
            if (route == null)
            {
                return NotFound($"Route with ID '{id}' not found.");
            }
            return View(route);
        }
        [HttpGet]
        public async Task<IActionResult> CreateRoute()
        {
            var farms = await _farmService.GetAllFarmsAsync(); // Lấy danh sách Farm
            ViewBag.Farms = farms; // Truyền danh sách Farm vào View
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoute(Route route, Guid[] Farms) // Nhận mảng ID farm
        {
            route.Farms = new List<Farm>(); // Khởi tạo danh sách Farm
            if (Farms != null) // Kiểm tra nếu Farms không null
            {
                foreach (var farmId in Farms)
                {
                    var farm = await _dataContext.Farms.FindAsync(farmId); // Tìm farm theo ID
                    if (farm != null)
                    {
                        route.Farms.Add(farm); // Thêm farm vào danh sách
                    }
                }
            }

            if (ModelState.IsValid)
            {
                await _routeService.CreateRouteAsync(route);
                return RedirectToAction(nameof(RouteManagement));
            }
            else
            {
                ModelState.AddModelError("", "There was an issue with the data provided. Please check your inputs.");
                return View(route);
            }

        }
        [HttpGet]
        public async Task<IActionResult> UpdateRoute(Guid id)
        {
            
            //return View();

            string idUpperCase = id.ToString().ToUpper();

            var route = await _routeService.GetRouteByIdAsync(idUpperCase);
            if (route == null)
            {
                return NotFound();
            }
            var farms = await _farmService.GetAllFarmsAsync(); // Lấy danh sách Farm
            ViewBag.Farms = farms; // Truyền danh sách Farm vào View
            return View(route);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRoute(Guid id, Route route, Guid[] Farms)
        {
            //if (id != route.Id)
            //{
            //    return NotFound();
            //}

            // Tìm route hiện tại từ cơ sở dữ liệu
            var existingRoute = await _routeService.GetRouteByIdAsync(id.ToString());
            if (existingRoute == null)
            {
                return NotFound();
            }

            // Xóa mọi farm hiện có trong route
            existingRoute.Farms.Clear(); // Xóa sạch danh sách farm hiện tại

            // Khởi tạo danh sách Farm mới
            //route.Farms = new List<Farm>();

            if (Farms != null) // Kiểm tra nếu Farms không null
            {
                foreach (var farmId in Farms)
                {
                    var farm = await _dataContext.Farms.FindAsync(farmId); // Tìm farm theo ID
                    if (farm != null)
                    {
                        existingRoute.Farms.Add(farm); // Thêm farm vào danh sách
                    }
                }
            }
            existingRoute.Name = route.Name; // Cập nhật tên

            if (ModelState.IsValid)
            {
                try
                {
                    // Cập nhật route với danh sách farm mới
                    await _routeService.UpdateRouteAsync(existingRoute);
                    return RedirectToAction("RouteManagement");
                }
                catch (Exception ex)
                {
                    // Log the error
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(route);
        }
        public async Task<IActionResult> NegateRoute(Guid id)
        {
            try
            {
                await _routeService.NegateRouteAsync(id);
                TempData["SuccessMessage"] = "Route account has been negated successfully.";
                return RedirectToAction(nameof(RouteManagement));
            }
            catch (KeyNotFoundException)
            {
                TempData["ErrorMessage"] = $"Customer with ID {id} not found.";
                return RedirectToAction(nameof(RouteManagement));
            }
            catch (Exception ex)
            {
                // Log the exception
                TempData["ErrorMessage"] = "An error occurred while updating the customer status.";
                return RedirectToAction(nameof(RouteManagement));
            }
        }
        


    }
}
