﻿using Microsoft.AspNetCore.Mvc;
using SellingKoi.Services;

namespace SellingKoi.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IOrderShortenService _orderShortenService;
        //private readonly I
        public AccountController(IAccountService accountservice, IOrderShortenService orderShortenService)
        {
            _accountService = accountservice;
            _orderShortenService = orderShortenService;
        }

        [HttpGet]
        public async Task<IActionResult> AccountManagement()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            if (accounts == null)
            {
                return NotFound("No account are found !");
            }

            return View(accounts);
        }


        [HttpGet]
        public async Task<IActionResult> DetailsAccount(Guid id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null)
            {
                return NotFound($"Account with ID '{id}' not found.");
            }
            return View(account);
        }
        [HttpGet]
        public async Task<IActionResult> Profile(Guid id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null)
            {
                return NotFound($"Account with ID '{id}' not found.");
            }
            var orders = await _orderShortenService.GetOrdersByUser(account.Username);
<<<<<<< HEAD

            // Sort orders before passing to view
            ViewBag.Orders = orders.OrderByDescending(o => o.Registration_date).ToList();

=======
            ViewBag.Orders = orders;
>>>>>>> 85c932f9196067835fd31f943dac733028fd05c6
            return View(account);
        }


        public async Task<IActionResult> NegateAccount(Guid id)
        {
            try
            {
                await _accountService.NegateAccountAsync(id);
                TempData["SuccessMessage"] = "Account account has been negated successfully.";
                return RedirectToAction(nameof(AccountManagement));
            }
            catch (KeyNotFoundException)
            {
                TempData["ErrorMessage"] = $"Account with ID {id} not found.";
                return RedirectToAction(nameof(AccountManagement));
            }
            catch (Exception ex)
            {
                // Log the exception
                TempData["ErrorMessage"] = "An error occurred while updating the customer status.";
                return RedirectToAction(nameof(AccountManagement));
            }
        }



        //admin
        //sign 
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(string username, string password, string fullname)
        {
            if (await _accountService.RegisterAsync(username, password, fullname))
            {
                return RedirectToAction("Login", "Home");
            }
            ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole(string id, string role)
        {
            if (id == null)
            {
                return BadRequest("Invalid account ID.");
            }

            if (string.IsNullOrWhiteSpace(role))
            {
                return BadRequest("Role cannot be empty.");
            }

            try
            {
                await _accountService.AssignRoleToUserAsync(id, role);
                TempData["SuccessMessage"] = $"Role updated successfully to {role} for account with ID {id}.";
            }
            catch (KeyNotFoundException)
            {
                TempData["ErrorMessage"] = $"Account with ID {id} not found.";
            }
            catch (Exception ex)
            {
                // Log the exception
                TempData["ErrorMessage"] = "An error occurred while updating the role.";
            }

            return RedirectToAction(nameof(DetailsAccount), new { id = id });
        }


        //login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var accountlogin = await _accountService.LoginAsync(username, password);
            if (accountlogin != null)
            {
                var role = accountlogin.Role;
                HttpContext.Session.SetString("UserId", accountlogin.Id.ToString());
                HttpContext.Session.SetString("Username", username);
                HttpContext.Session.SetString("UserRole", role); // Lưu vai trò vào 
                if (role == "ADMIN")
                    return RedirectToAction("AdminPage", "Home");
                if (role == "Customer")
                    return RedirectToAction("Index", "Home");
                if(role == "Staff")
                    return RedirectToAction("TaskOfStaff", "Task");
                

            }
            ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}
