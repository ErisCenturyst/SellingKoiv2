using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
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
        [HttpPost]
        public IActionResult AddToCart([FromBody] CartItem item)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            cart.Add(item);
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return Json(new { success = true, count = cart.Count });
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

        [HttpGet]
        public IActionResult GetCartCount()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            return Json(cart.Count);
        }
    }
}
