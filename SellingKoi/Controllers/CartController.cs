using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SellingKoi.Models;

namespace SellingKoi.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            return View(cart); // Đảm bảo rằng bạn truyền một danh sách không null
        }
        private static List<CartItem> _cartItems = new List<CartItem>();

        // Hàm để lấy các mục trong giỏ hàng
        [HttpGet]
        public IActionResult GetCartItems()
        {
            
            return Json(_cartItems); // Trả về danh sách dưới dạng JSON
        }


        [HttpPost]
        public IActionResult AddToCart([FromBody] CartItem item)
        {
            // Lấy danh sách FarmID từ session
            var farmListJson = HttpContext.Session.GetString("FarmShouldInclude");
            List<string> farmIds;

            if (!string.IsNullOrEmpty(farmListJson))
            {
                // Nếu đã có danh sách, chuyển đổi từ JSON về List<string>
                farmIds = JsonConvert.DeserializeObject<List<string>>(farmListJson);
            }
            else
            {
                // Nếu chưa có, khởi tạo danh sách mới
                farmIds = new List<string>();
            }

            // Thêm FarmID mới vào danh sách nếu chưa có
            if (!farmIds.Contains(item.FarmID))
            {
                farmIds.Add(item.FarmID);
            }

            // Lưu lại danh sách FarmID vào session
            HttpContext.Session.SetString("FarmShouldInclude", JsonConvert.SerializeObject(farmIds));


            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            cart.Add(new CartItem { Id = item.Id, Name = item.Name, Price = item.Price,FarmID = item.FarmID });

            // Lưu giỏ hàng vào session
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return Ok();
        }

        [HttpPost]
        public IActionResult RemoveFromCart([FromBody] RemoveFromCartRequest request)
        {
            string itemId = request.ItemId; // Lấy itemId từ request
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            // Tìm item trong giỏ hàng
            var itemToRemove = cart.FirstOrDefault(i => i.Id == itemId);
            if (itemToRemove != null)
            {
                // Lưu FarmID trước khi xóa item
                string farmIdToRemove = itemToRemove.FarmID; // Giả sử CartItem có thuộc tính FarmId

                cart.Remove(itemToRemove); // Xóa item

                // Cập nhật danh sách FarmID trong session
                var farmListJson = HttpContext.Session.GetString("FarmShouldInclude");
                List<string> farmIds = string.IsNullOrEmpty(farmListJson) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(farmListJson);

                // Xóa FarmID khỏi danh sách nếu nó tồn tại
                if (farmIds.Contains(farmIdToRemove))
                {
                    farmIds.Remove(farmIdToRemove);
                }

                // Lưu lại danh sách FarmID vào session
                HttpContext.Session.SetString("FarmShouldInclude", JsonConvert.SerializeObject(farmIds));
            }

            // Lưu giỏ hàng vào session
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return Ok();
        }
       
        //[HttpPost]

        //public IActionResult AddRouteToCart([FromBody] RouteCartItem routeItem)
        //{
        //    if (routeItem == null || routeItem.RouteId == Guid.Empty)
        //    {
        //        return BadRequest("Invalid route item.");
        //    }

        //    var cart = HttpContext.Session.GetObjectFromJson<List<RouteCartItem>>("RouteCart") ?? new List<RouteCartItem>();

        //    //var existingRoute = cart.FirstOrDefault(i => i.RouteId.Equals((routeItem.RouteId).ToString().ToUpper()));
        //    //if (existingRoute != null)
        //    //{
        //        cart.Add(routeItem); // Thêm mới
        //    //}

        //    HttpContext.Session.SetObjectAsJson("RouteCart", cart);
        //    return Ok();
        //}
        //[HttpPost]
        //public IActionResult AddToCartKoi(CartItem koiItem)
        //{
        //    var cartItem = new CartItem
        //    {
        //        Id = koiItem.Id,
        //        Name = koiItem.Name,
        //        IsKoi = true // Đánh dấu đây là cá Koi
        //    };
        //    AddToCart(cartItem); // Thêm vào giỏ hàng
        //    return Ok();
        //}
        //[HttpPost]
        //public IActionResult AddToCartRoute(RouteCartItem routeItem)
        //{
        //    var cartItem = new CartItem
        //    {
        //        Id = routeItem.RouteId,
        //        Name = routeItem.RouteName,
        //        Price = routeItem.Price,
        //        IsKoi = false // Đánh dấu đây là lộ trình
        //    };
        //    AddToCart(cartItem); // Thêm vào giỏ hàng
        //    return Ok();
        //}
        public class RemoveFromCartRequest
        {
            public string ItemId { get; set; }
        }
    }
}
