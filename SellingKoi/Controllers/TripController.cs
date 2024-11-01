using Microsoft.AspNetCore.Mvc;
using SellingKoi.Models;
using SellingKoi.Services;

namespace SellingKoi.Controllers
{
    public class TripController : Controller
    {
        private readonly ITripService _tripService;
        private readonly IAccountService _accountService;
        private readonly IOrderShortenService _orderShortenService;
        //private readonly DataContext _context;

        public TripController(IAccountService accountService, ITripService tripService, IOrderShortenService orderShortenService)
        {
            _tripService = tripService;
            _accountService = accountService;
            _orderShortenService = orderShortenService;
            //_context = context;
        }

        [HttpGet]
        public async Task<IActionResult> TripManagement()
        {
            var trips = await _tripService.GetAllTripAsync();
            var staffList = await _accountService.GetStaffAccountAsync();
            ViewBag.stafflist = staffList;
            return View(trips.ToList());
        }
        [HttpGet]
        public async Task<IActionResult> CreateTrip()
        {
            var staffList = await _accountService.GetStaffAccountAsync();
            var salestaffList = await _accountService.GetSaleStaffAccountAsync();

            ViewBag.StaffList = staffList;
            ViewBag.SaleStaffList = salestaffList;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateTrip(Trip trip, List<string> FollowAccountsID, string SaleStaffID)
        {
            // Lấy thông tin đầy đủ của nhân viên từ database để kiểm tra
            var stafflist = await _accountService.SearchlistStaffbylistId(FollowAccountsID);
            var salestaff = await _accountService.GetAccountByIdAsync(Guid.Parse(SaleStaffID));

            if (stafflist == null || salestaff == null)
            {
                return BadRequest();
            }
            trip.SaleStaff = salestaff;
            trip.FollowAccountsID = FollowAccountsID;
            await _tripService.CreateTripAsync(trip);
            return RedirectToAction("TripManagement", "Trip");
        }

        [HttpGet]
        public async Task<IActionResult> DetailsTrip(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var trip = await _tripService.GetTripByIdAsync(id);
            if (trip == null)
            {
                return NotFound();
            }
            var staffList = await _accountService.GetStaffAccountAsync();
            var salestaffList = await _accountService.GetSaleStaffAccountAsync();
            var orderlist = await _orderShortenService.GetListOrderBelongToStrip(id);

            ViewBag.StaffList = staffList;
            ViewBag.SaleStaffList = salestaffList;
            ViewBag.OrderList = orderlist;
            return View(trip);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTrip(string tripid, string Status, List<string> FollowAccountsID, string SaleStaffID, DateTime Date_of_Departure)
        {
            if (string.IsNullOrEmpty(tripid))
            {
                return BadRequest("Trip ID is required");
            }

            var trip = await _tripService.GetTripByIdAsync(tripid);
            if (trip == null)
            {
                return NotFound();
            }

            // Hủy assign sale staff nếu SaleStaffID là null
            if (string.IsNullOrEmpty(SaleStaffID))
            {
                await _tripService.UnasignSaleStaffAsync(tripid);
            }
            else
            {
                var salestaff = await _accountService.GetAccountByIdAsync(Guid.Parse(SaleStaffID));
                if (salestaff != null)
                {
                    trip.SaleStaff = salestaff;
                }
            }

            // Hủy assign list staffs nếu FollowAccountsID là null hoặc rỗng
            if (FollowAccountsID == null || !FollowAccountsID.Any())
            {
                await _tripService.UnasignListStaffsAsync(tripid);
            }
            else
            {
                var stafflist = await _accountService.SearchlistStaffbylistId(FollowAccountsID);
                if (stafflist != null)
                {
                    trip.FollowAccountsID = FollowAccountsID;
                }
            }

            // Cập nhật các thông tin khác
            trip.status = Status;
            trip.Date_of_departure = Date_of_Departure;

            try
            {
                await _tripService.UpdateTripAsync(trip);
                return RedirectToAction("TripManagement", "Trip");
            }
            catch (Exception ex)
            {
                // Log the error
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                return View(trip);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTrip([FromBody] DeleteTripViewModel model)
        {
            if (string.IsNullOrEmpty(model.TripId))
            {
                return Json(new { success = false, message = "ID chuyến đi không hợp lệ" });
            }


            var trip = await _tripService.GetTripByIdAsync(model.TripId);
            if (trip == null)
            {
                return Json(new { success = false, message = "Không tìm thấy chuyến đi" });
            }

            // Kiểm tra xem chuyến đi có nhân viên không
            if (trip.SaleStaff != null || (trip.FollowAccountsID != null && trip.FollowAccountsID.Any()))
            {
                return Json(new { success = false, message = "Bạn phải hủy chỉ định các nhân viên trước khi xóa chuyến đi" });
            }

            try
            {
                await _tripService.DeleteTripAsync(model.TripId);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log exception
                return Json(new { success = false, message = "Có lỗi xảy ra khi xóa chuyến đi" });
            }
        }
        //[HttpGet]
        //public IActionResult QuickCreateTrip()
        //{
        //    // Lấy danh sách nhân viên có role là Staff
        //    var staffList = _context.Accounts.Where(a => a.Role == "Staff").ToList();
        //    ViewBag.StaffList = staffList;

        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> QuickCreateTrip(Trip trip)
        //{
        //        // Kiểm tra xem staff có được chọn không
        //        if (trip.staff != null && trip.staff.Id != null)
        //        {
        //            // Lấy thông tin đầy đủ của nhân viên từ database
        //            trip.staff = await _context.Accounts.FindAsync(trip.staff.Id);
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("staff", "Vui lòng chọn nhân viên.");
        //            // Load lại danh sách nhân viên
        //            var staffList = _context.Accounts.Where(a => a.Role == "Staff").ToList();
        //            ViewBag.StaffList = staffList;
        //            return View(trip);
        //        }
        //        await _tripService.CreateTripAsync(trip);
        //    return RedirectToAction("OrderShortenedManagement", "OrderShorten");   
        //}
        public class DeleteTripViewModel
        {
            public string TripId { get; set; }
        }

    }
}
