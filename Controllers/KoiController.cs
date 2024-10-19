
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SellingKoi.Data;
using SellingKoi.Models;
using SellingKoi.Services;

namespace SellingKoi.Controllers
{
    public class KoiController : Controller
    {
        private readonly IKoiService _koiService;
        private readonly IFarmService _farmService;
       

        public KoiController(IKoiService koiService,IFarmService farmService,DataContext dataContext)
        {
            _koiService = koiService;
            _farmService = farmService;
 
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedKOIs(int page = 1, int limit = 10)
        {
            var paginatedKOIs = await _koiService.GetKOIsPaged(page, limit);
            return Ok(paginatedKOIs);
        }


        public async Task<IActionResult> KoiManagement()
        {
            var kois = await _koiService.GetAllKoisAsync();
            if(kois == null)
            {
                return NotFound("No Koi are found !"); 
            }
            return View(kois);
        }
        [HttpGet]
        public async Task<IActionResult> KoiShopping()
        {
            var kois = await _koiService.GetAllKoisAsync();
          

            if (kois == null)
            {
                return NotFound("No Koi are found !");
            }
            return View(kois);
        }

        [HttpGet]
        public async Task<IActionResult> DetailsKoi(Guid id)
        {
  
            if (id == null)
            {
                return NotFound($"Koi with id '{id}' not found.");
            }

            var koi = await _koiService.GetKoiByIdAsync(id);
            if (koi == null)
            {
                return NotFound($"Koi with ID '{id}' not found.");
            }
            return View(koi);
        }
        [HttpGet]
        public async Task <IActionResult> CreateKoi(Guid farmId)
        {
            //string farmidUpper = farmId.ToString().ToUpper();
            //var farm = await _dataContext.Farms.FindAsync(farmidUpper);
            //var farm = await _farmService.GetFarmByIdAsync(farmidUpper);

            var koi = new KOI
            {
                FarmID = farmId, // Gán FarmID cho Koi mới
                //Farm = farm
            };
            return View(koi);
        }

        [HttpPost]
        public async Task<IActionResult> CreateKoi(KOI koi)
        {
            var farm = await _farmService.GetFarmByIdAsync(koi.FarmID.ToString().ToUpper());
            koi.Farm = farm;
            if (koi != null)
            {
                await _koiService.CreateKoiAsync(koi);
                //return RedirectToAction(nameof(KoiManagement));
                return RedirectToAction("DetailsFarm", "Farm", new { id = koi.FarmID }); // Chuyển hướng về trang chi tiết farm
            }
            else
            {
                ModelState.AddModelError("", "There was an issue with the data provided. Please check your inputs.");
                return View(koi);
            }

        }

        [HttpGet]
        public async Task<IActionResult> EditKoi(Guid id)
        {
            var koi= await _koiService.GetKoiByIdAsync(id);
            if (koi == null)
            {
                return NotFound();
            }   
            return View(koi);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> EditKoi(Guid id, [Bind("Id,Name,Type,Age,Description,FarmID")] KOI koi)
        {
            if (id != koi.Id)
            {
                return NotFound();
            }
             try
             {
                 await _koiService.UpdateKoiAsync(koi);
             //return RedirectToAction(nameof(KoiManagement));
             return RedirectToAction("DetailsFarm", "Farm", new { id = koi.FarmID });
             }   
             catch (Exception ex)
             {
             // Log the error
             ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
             return NotFound();
             }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> NegateKoi(Guid id)
        {
            try
            {
                var koi = await _koiService.GetKoiByIdAsync(id);
                await _koiService.NegateKoiAsync(id);
                TempData["SuccessMessage"] = "Koi has been negated successfully.";
                return RedirectToAction("DetailsFarm", "Farm", new { id = koi.FarmID });
            }
            catch (KeyNotFoundException)    
            {
                TempData["ErrorMessage"] = $"Koi with ID {id} not found.";
                return NotFound();
            }
            catch (Exception ex)
            {
                // Log the exception
                TempData["ErrorMessage"] = "An error occurred while updating the customer status.";
                return NotFound();
            }
        }
    }
}
