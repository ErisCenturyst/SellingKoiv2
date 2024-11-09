using Microsoft.AspNetCore.Mvc;
using SellingKoi.Services;

namespace SellingKoi.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITripService _tripService;
        private readonly IAccountService _accountService;
        private readonly IOrderShortenService _orderShortenService;
        private readonly ITaskService _taskService;

        public TaskController(ITripService tripService, IAccountService accountService, 
            IOrderShortenService orderShortenService, ITaskService taskService)
        {
            _tripService = tripService;
            _accountService = accountService;
            _orderShortenService = orderShortenService;
            _taskService = taskService;
        }
        [HttpGet]
        public async Task<IActionResult> TaskOfStaff(string staffid) 
        {
            if (staffid == null)
            {
                return View();
            }
            var tasks = await _taskService.GetAllTaskOfStaff(staffid);
            if (tasks.Any()) { 
                return View(tasks);
            }else
            {
                ViewBag.ErrorMessage = "Chưa được phân công công việc";
                return View();
            }
        }



    }
}
