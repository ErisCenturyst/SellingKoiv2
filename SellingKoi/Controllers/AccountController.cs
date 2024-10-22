using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using SellingKoi.Services;

namespace SellingKoi.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountservice)
        {
            _accountService = accountservice;
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

            if (id == null)
            {
                return NotFound($"Account with id '{id}' not found.");
            }

            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null)
            {
                return NotFound($"Account with ID '{id}' not found.");
            }
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
                HttpContext.Session.SetString("Username", username);
                HttpContext.Session.SetString("UserRole", role); // Lưu vai trò vào 
                if(role == "ADMIN")
                    return RedirectToAction("AdminPage", "Home");
                if(role == "Customer")
                    return RedirectToAction("Index", "Home");

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
