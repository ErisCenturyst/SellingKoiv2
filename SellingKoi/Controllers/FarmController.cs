using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SellingKoi.Data;
using SellingKoi.Models;
using SellingKoi.Services;

namespace SellingKoi.Controllers
{
    public class FarmController : Controller
    {
        private readonly IFarmService _farmService;
        private readonly DataContext _context;
        public FarmController(IFarmService farmService, DataContext context)
        {
            _farmService = farmService;
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> FarmManagement()
        {
            var farms = await _farmService.GetAllFarmsAsync();
            if (farms == null)
            {
                return NotFound("No farm are found !");
            }
            return View(farms);
        }

        [HttpGet]
        public async Task<IActionResult> DetailsFarm(Guid id)
        {
            var farm = await _farmService.GetFarmByIdAsync(id.ToString());
            if (farm == null)
            {
                return NotFound($"Farm with ID '{id}' not found.");
            }
            return View(farm);
        }

        [HttpGet]
        public async Task<IActionResult> FarmShopping()
        {
            var farms = await _farmService.GetAllFarmsAsync();
            if (farms == null)
            {
                return NotFound("No farm are found !");
            }
            return View(farms);
        }

        [HttpGet]
        public async Task<IActionResult> DetailsShoppingFarm(Guid id)
        {

            if (id == null)
            {
                return NotFound($"Farm with id '{id}' not found.");
            }

            var farm = await _farmService.GetFarmByIdAsync(id.ToString());
            if (farm == null)
            {
                return NotFound($"Farm with ID '{id}' not found.");
            }
            return View(farm);
        }

        [HttpGet]
        public IActionResult CreateFarm()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateFarm(Farm farm, IFormFile AvatarUpload)
        {

            // Xử lý URL ảnh đại diện nếu có
            if (!string.IsNullOrEmpty(farm.AvatarUrl))
            {
                // AvatarUrl đã được set bởi JavaScript khi tải lên Firebase
                // Không cần xử lý thêm ở đây
                //koidb.AvatarUrl = koi.AvatarUrl; // Cập nhật trường Avatar (chỉnh cần khi update, tạo mới ko cần dòng này)

            }
            else if (AvatarUpload != null)
            {
                // Nếu có file được tải lên nhưng chưa được xử lý bởi Firebase
                // Bạn có thể xử lý tải lên ở đây nếu cần
                // Ví dụ: tải lên Firebase và lấy URL
                //koi.AvatarUrl = await UploadFileToFirebase(AvatarUpload);
            }


            // Kiểm tra và xử lý các thuộc tính Location và Size nếu cần
            if (string.IsNullOrWhiteSpace(farm.Location))
            {
                ModelState.AddModelError("Location", "Vị trí farm không được để trống.");
            }

            if (farm.Size <= 0)
            {
                ModelState.AddModelError("Size", "Diện tích phải lớn hơn 0.");
            }

            await _farmService.CreateFarmAsync(farm);
            return RedirectToAction(nameof(FarmManagement));
        }


        [HttpGet]
        public async Task<IActionResult> UpdateFarm(Guid id)
        {
            string idUpperCase = id.ToString().ToUpper();

            var farm = await _farmService.GetFarmByIdAsync(idUpperCase);
            if (farm == null)
            {
                return NotFound();
            }
            return View(farm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFarm(Guid id, Farm farm/*, IFormFile AvatarUpload*/)
        {
            var existedfarm = await _farmService.GetFarmByIdAsync(id.ToString());
            farm.KOIs = existedfarm.KOIs;
            // Xử lý URL ảnh đại diện nếu có
            if (string.IsNullOrEmpty(farm.AvatarUrl))
            {
                farm.AvatarUrl = existedfarm.AvatarUrl;
                // AvatarUrl đã được set bởi JavaScript khi tải lên Firebase
                // Không cần xử lý thêm ở đây
                //farmdb.AvatarUrl = farm.AvatarUrl; // Cập nhật trường Avatar (chỉnh cần khi update, tạo mới ko cần dòng này)
            }
            //else if (AvatarUpload != null)
            //{
            //    // Nếu có file được tải lên nhưng chưa được xử lý bởi Firebase
            //    // Bạn có thể xử lý tải lên ở đây nếu cần
            //    // Ví dụ: tải lên Firebase và lấy URL
            //    //koi.AvatarUrl = await UploadFileToFirebase(AvatarUpload);
            //}
            // Kiểm tra và xử lý các thuộc tính Location và Size nếu cần
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(farm.Location))
                {
                    ModelState.AddModelError("Location", "Vị trí farm không được để trống.");
                }

                if (farm.Size <= 0)
                {
                    ModelState.AddModelError("Size", "Diện tích phải lớn hơn 0.");
                }
                try
                {
                    
                    _context.Entry(existedfarm).State = EntityState.Detached; // Detach thực thể cũ
                    await _farmService.UpdateFarmAsync(farm);
                    return RedirectToAction("FarmManagement", "Farm");
                }
                catch (Exception ex)
                {
                    // Log the error
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(farm);
        }

        public async Task<IActionResult> NegateFarm(Guid id)
        {
            try
            {
                await _farmService.NegateFarmAsync(id);
                TempData["SuccessMessage"] = "Customer account has been negated successfully.";
                return RedirectToAction(nameof(FarmManagement));
            }
            catch (KeyNotFoundException)
            {
                TempData["ErrorMessage"] = $"Customer with ID {id} not found.";
                return RedirectToAction(nameof(FarmManagement));
            }
            catch (Exception ex)
            {
                // Log the exception
                TempData["ErrorMessage"] = "An error occurred while updating the customer status.";
                return RedirectToAction(nameof(FarmManagement));
            }
        }

    }
}
