using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using SellingKoi.Data;
using SellingKoi.Models;
using SellingKoi.Services;

namespace SellingKoi.Controllers
{
    public class TripController : Controller
    {
        private readonly ITripService _tripService;
        private readonly IAccountService _accountService;
        private readonly IOrderShortenService _orderShortenService;
        private readonly DataContext _context;

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
            //check staff, salestaff ko co tripid (account ko co tripid)
            var salestaffList = await _accountService.GetSaleStaffWithNoTrip();
            var staffList = await _accountService.GetStaffWithNoTrip();
            var orderlist = await _orderShortenService.GetListOrderBelongToStrip(id);

            //danh sach sale staff thuoc trip
            var salestaffBelongToTrip = await _accountService.GetSaleStaffByTripIdAsync(id);

            //danh sach staff thuoc trip
            var ListStaffBelongToTrip = await _accountService.GetStaffByTripIdAsync(id);

            var combinedSaleStaffList = salestaffList;
            var combinedStaffList = staffList.Concat(ListStaffBelongToTrip).Distinct().ToList(); // Kết hợp và loại bỏ trùng lặp


            // Kết hợp salestaffList và salestaffBelongToTrip
            // Kiểm tra nếu salestaffBelongToTrip không phải là null và không có trong danh sách
            if (salestaffBelongToTrip != null && !salestaffList.Any(s => s.Id == salestaffBelongToTrip.Id))
            {
                combinedSaleStaffList.Add(salestaffBelongToTrip); // Thêm vào danh sách
            }

            ViewBag.StaffList = combinedStaffList; 
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
            //var salestaff = await _accountService.GetAccountByIdAsync(Guid.Parse(SaleStaffID));

            // Hủy assign sale staff nếu SaleStaffID là null
            if (string.IsNullOrEmpty(SaleStaffID))
            {
                if (trip.SaleStaff != null)
                {
                    await _accountService.UnassignTripFromSaleStaffAsync(trip.SaleStaff.Id.ToString().ToUpper());
                }
                await _tripService.UnasignSaleStaffAsync(tripid);
                
            }
            else
            {
                var salestaff = await _accountService.GetAccountByIdAsync(Guid.Parse(SaleStaffID));
                if (salestaff != null)
                {
                    trip.SaleStaff = salestaff;
                    //Gán tripid vào các sale staff - trưởng đoàn của chuyến 
                    salestaff.TripId = trip.Id.ToString(); // Gán tripId vào account
                }


            }

            // Hủy assign list staffs nếu FollowAccountsID là null hoặc rỗng
            if (FollowAccountsID == null)
            {
                if (trip.FollowAccountsID.Any()) 
                {
                    await _accountService.UnassignTripFromFollowStaffAsync(trip.FollowAccountsID);
                }
                await _tripService.UnasignListStaffsAsync(tripid);
            }
            else
            {
                var stafflist = await _accountService.SearchlistStaffbylistId(FollowAccountsID);
                if (stafflist != null)
                {
                    trip.FollowAccountsID = FollowAccountsID;

                    // Gán tripId cho các tài khoản có id tương ứng
                    foreach (var accountId in FollowAccountsID)
                    {
                        var account = await _accountService.GetAccountByIdAsync(Guid.Parse(accountId));
                        if (account != null)
                        {
                            account.TripId = trip.Id.ToString(); // Gán tripId vào account
                        }
                    }

                    //await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
                }
            }
            // Kiểm tra trạng thái
            if (Status == "Ongoing")
            {
                var orders = await _orderShortenService.GetOrdersByTrip(tripid); // Lấy danh sách đơn hàng thuộc chuyến đi
                if (orders.Any(order => order.Status != "Confirmed"))
                {
                    //ModelState.AddModelError("", "Tất cả đơn hàng phải được khách hàng xác nhận trước khi cập nhật chuyến đi sẵn sàng sang Nhật.");
                    //return View(trip); // Trả về view với thông báo lỗi
                    TempData["ErrorMessage"] = "Tất cả đơn hàng phải được khách hàng xác nhận trước khi cập nhật chuyến đi sẵn sàng sang Nhật.";
                    return RedirectToAction("DetailsTrip", "Trip", new { id = tripid }); // Chuyển hướng đến DetailsTrip với 
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
