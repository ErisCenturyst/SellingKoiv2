using Microsoft.AspNetCore.Mvc;
using SellingKoi.Models;
using SellingKoi.Services;
using Microsoft.EntityFrameworkCore;
using SellingKoi.Data;

namespace SellingKoi.Controllers
{
    public class TripController : Controller
    {
        private readonly ITripService _service;
        private readonly IAccountService _accountService;
        private readonly DataContext _context;

        public TripController(ITripService service, IAccountService accountService, DataContext context)
        {
            _service = service;
            _accountService = accountService;
            _context = context;
        }



        [HttpGet]
        public IActionResult TripManagement()
        {
            
            return View();
        }

        [HttpGet]
        public IActionResult QuickCreateTrip()
        {
            // Lấy danh sách nhân viên có role là Staff
            var staffList = _context.Accounts.Where(a => a.Role == "Staff").ToList();
            ViewBag.StaffList = staffList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> QuickCreateTrip(Trip trip)
        {
                // Kiểm tra xem staff có được chọn không
                if (trip.staff != null && trip.staff.Id != null)
                {
                    // Lấy thông tin đầy đủ của nhân viên từ database
                    trip.staff = await _context.Accounts.FindAsync(trip.staff.Id);
                }
                else
                {
                    ModelState.AddModelError("staff", "Vui lòng chọn nhân viên.");
                    // Load lại danh sách nhân viên
                    var staffList = _context.Accounts.Where(a => a.Role == "Staff").ToList();
                    ViewBag.StaffList = staffList;
                    return View(trip);
                }
                await _service.CreateTripAsync(trip);
                return RedirectToAction("OrderShortenedManagement", "OrderShorten");
            
            
        }
    }
}
