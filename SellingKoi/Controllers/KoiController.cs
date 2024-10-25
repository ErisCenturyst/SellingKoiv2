<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Mvc;
=======
﻿
using Microsoft.AspNetCore.Mvc;
>>>>>>> 5b5388ad21c23fb2001d1b054751ac52623bd0d7
using Microsoft.EntityFrameworkCore;
using SellingKoi.Data;
using SellingKoi.Models;
using SellingKoi.Services;

namespace SellingKoi.Controllers
{
    public class KoiController : Controller
    {
<<<<<<< HEAD
        private readonly DataContext _context;
=======
>>>>>>> 5b5388ad21c23fb2001d1b054751ac52623bd0d7
        private readonly IKoiService _koiService;
        private readonly IFarmService _farmService;
       

<<<<<<< HEAD
        public KoiController(DataContext context, IKoiService koiService,IFarmService farmService)
        {
            _context = context;
            _koiService = koiService;
            _farmService = farmService;
=======
        public KoiController(IKoiService koiService,IFarmService farmService,DataContext dataContext)
        {
            _koiService = koiService;
            _farmService = farmService;
 
>>>>>>> 5b5388ad21c23fb2001d1b054751ac52623bd0d7
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
<<<<<<< HEAD
            if (ModelState.IsValid)
            {
                // Set a default avatar URL if none is provided
                if (string.IsNullOrEmpty(koi.AvatarUrl))
                {
                    koi.AvatarUrl = "/default-koi-avatar.jpg"; // Replace with your default avatar path
                }

                await _koiService.CreateKoiAsync(koi);
                return RedirectToAction(nameof(Index));
            }
            return View(koi);
=======
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

>>>>>>> 5b5388ad21c23fb2001d1b054751ac52623bd0d7
        }

        [HttpGet]
        public async Task<IActionResult> EditKoi(Guid id)
        {
<<<<<<< HEAD
            var koi = await _koiService.GetKoiByIdAsync(id);
=======
            var koi= await _koiService.GetKoiByIdAsync(id);
>>>>>>> 5b5388ad21c23fb2001d1b054751ac52623bd0d7
            if (koi == null)
            {
                return NotFound();
            }   
<<<<<<< HEAD
            return View(koi);  // Add this line
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
=======
            return View(koi);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
>>>>>>> 5b5388ad21c23fb2001d1b054751ac52623bd0d7
        public async Task<IActionResult> EditKoi(Guid id, [Bind("Id,Name,Type,Age,Description,FarmID")] KOI koi)
        {
            if (id != koi.Id)
            {
                return NotFound();
            }
<<<<<<< HEAD

            if (ModelState.IsValid)
            {
                try
                {
                    await _koiService.UpdateKoiAsync(koi);
                    return RedirectToAction(nameof(KoiManagement));
                }
                catch (DbUpdateConcurrencyException)
                {
                    var exists = await _koiService.GetKoiByIdAsync(koi.Id) != null;
                    if (!exists)
                    {
                        return NotFound();
                    }
                    throw;
                }
            }
            return View(koi);
=======
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
>>>>>>> 5b5388ad21c23fb2001d1b054751ac52623bd0d7
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
<<<<<<< HEAD

        private async Task<bool> KOIExists(Guid id)
        {
            return await _context.KOIs.AnyAsync(e => e.Id == id);
        }
=======
>>>>>>> 5b5388ad21c23fb2001d1b054751ac52623bd0d7
    }
}
