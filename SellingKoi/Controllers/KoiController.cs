using Microsoft.AspNetCore.Mvc;
using SellingKoi.Data;
using SellingKoi.Models;
using SellingKoi.Services;

namespace SellingKoi.Controllers
{
    public class KoiController : Controller
    {

        private readonly IKoiService _koiService;
        private readonly IFarmService _farmService;

        public KoiController(IKoiService koiService, IFarmService farmService, DataContext dataContext)
        {
            _koiService = koiService;
            _farmService = farmService;

        }

        //upload imgage
        //[HttpPost]
        //public async Task<IActionResult> Index(FileUploadViewModel model)
        //{
        //    var file = model.File;
        //    FileStream fs;
        //    FileStream ms;
        //    if (file.Length > 0)
        //    {
        //        string folderName = "KoiImage";
        //        string path = Path.Combine(_env.WebRootPath, $"images/{folderName}");
        //        ms = new FileStream(Path.Combine(path, file.FileName), FileMode.Open);
        //        var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
        //        var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

        //        // you can use CancellationTokenSource to cancel the upload midway
        //        var cancellation = new CancellationTokenSource();

        //        var task = new FirebaseStorage(
        //            Bucket,
        //            new FirebaseStorageOptions
        //            {
        //                AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
        //                ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
        //            })
        //            .Child("receipts")
        //            .Child("test")
        //            .Child($"aspcore.png")
        //            .PutAsync(ms, cancellation.Token);

        //        task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");


        //        try
        //        {
        //            ViewBag.link = await task;
        //            return Ok();
        //        }
        //        catch (Exception ex)
        //        {
        //            ViewBag.error = $"Exception was thrown: {ex}";
        //        }

        //    }
        //    return BadRequest();
        //}

        //

        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedKOIs(int page = 1, int limit = 10)
        {
            var paginatedKOIs = await _koiService.GetKOIsPaged(page, limit);
            return Ok(paginatedKOIs);
        }


        public async Task<IActionResult> KoiManagement()
        {
            var kois = await _koiService.GetAllKoisAsync();
            if (kois == null)
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
        public async Task<IActionResult> DetailsShoppingKoi(Guid id)
        {
            var koi = await _koiService.GetKoiByIdAsync(id);
            if (koi == null)
            {
                return NotFound($"Koi with ID '{id}' not found.");
            }
            var relatedKois = await _koiService.GetKoisWithType(koi.Type.ToString());
            ViewBag.RelatedKois = relatedKois;
            return View(koi);
        }


        [HttpGet]
        public async Task<IActionResult> CreateKoi(Guid farmId)
        {

            var koi = new KOI
            {
                FarmID = farmId, // Gán FarmID cho Koi mới
                //Farm = farm
            };
            return View(koi);
        }

        [HttpPost]
        public async Task<IActionResult> CreateKoi(KOI koi, IFormFile AvatarUpload)
        {
            var farm = await _farmService.GetFarmByIdAsync(koi.FarmID.ToString().ToUpper());
            koi.Farm = farm;
            // Xử lý URL ảnh đại diện nếu có
            if (!string.IsNullOrEmpty(koi.AvatarUrl))
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
            var koi = await _koiService.GetKoiByIdAsync(id);
            if (koi == null)
            {
                return NotFound();
            }
            return View(koi);
        }

        [HttpPost]
        public async Task<IActionResult> EditKoi(Guid id, KOI koi, IFormFile AvatarUpload)
        {
            try
            {

                var koidb = await _koiService.GetKoiByIdAsync(id);
                if (koidb == null)
                {
                    return NotFound();
                }

                // Xử lý URL ảnh đại diện nếu có
                if (!string.IsNullOrEmpty(koi.AvatarUrl))
                {
                    // AvatarUrl đã được set bởi JavaScript khi tải lên Firebase
                    // Không cần xử lý thêm ở đây
                    koidb.AvatarUrl = koi.AvatarUrl; // Cập nhật trường Avatar (chỉnh cần khi update, tạo mới ko cần dòng này)

                }
                else if (AvatarUpload != null)
                {
                    // Nếu có file được tải lên nhưng chưa được xử lý bởi Firebase
                    // Bạn có thể xử lý tải lên ở đây nếu cần
                    // Ví dụ: tải lên Firebase và lấy URL
                    //koi.AvatarUrl = await UploadFileToFirebase(AvatarUpload);
                }


                // Cập nhật các thuộc tính của thực thể đã được theo dõi
                koidb.Status = koi.Status;
                koidb.Name = koi.Name;
                koidb.Description = koi.Description;
                koidb.AlbumUrl = koi.AlbumUrl;
                koidb.Age = koi.Age;
                koidb.Type = koi.Type;
                await _koiService.UpdateKoiAsync(koidb); // Cập nhật thực thể đã được theo dõi
                return RedirectToAction("KoiManagement", "Koi");
            }
            catch (Exception ex)
            {
                // Log the error
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                return View(koi); // Trả về view với model để hiển thị lỗi
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
                return RedirectToAction("KoiManagement", "Koi");
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
