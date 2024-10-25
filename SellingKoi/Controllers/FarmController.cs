using Microsoft.AspNetCore.Mvc;
using SellingKoi.Models;
using SellingKoi.Services;

namespace SellingKoi.Controllers
{
    public class FarmController : Controller
    {
        private readonly IFarmService _farmService;
        public FarmController(IFarmService farmService)
        {
            _farmService = farmService;
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
        public async Task<IActionResult> CreateFarm(Farm farm)
        {

            if (ModelState.IsValid)
            {
                await _farmService.CreateFarmAsync(farm);
                return RedirectToAction(nameof(FarmManagement));
            }
            else
            {
                ModelState.AddModelError("", "There was an issue with the data provided. Please check your inputs.");
                return View(farm);
            }

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
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateFarm(Guid id, Farm farm)
        {
            if (id != farm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _farmService.UpdateFarmAsync(farm);
                    //return RedirectToAction(nameof(FarmManagement));
                    return RedirectToAction("DetailsFarm", "Farm", new { id = farm.Id });
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
