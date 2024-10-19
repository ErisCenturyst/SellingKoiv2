namespace SellingKoi.Models
{
    public class CartItem
    {
        
        public string Id { get; set; } // ID của cá Koi hoặc lộ trình
        public string Name { get; set; } // Tên cá Koi hoặc lộ trình
        //public int Quantity { get; set; } // Số lượng
        //public int? RouteId { get; set; } // ID của lộ trình nếu có
        //public string RouteName { get; set; } // Tên lộ trình nếu có
        public double Price { get; set; } = 0; // Giá lộ trình
    }
}

