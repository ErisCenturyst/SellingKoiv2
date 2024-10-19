using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SellingKoi.Models
{
    public class Cart
    {
        
        [Key]
        public Guid Id { get; set; } // Khóa chính

        [ForeignKey("Account")]
        public Guid CustomerId { get; set; } // Khóa ngoại đến Account

        [ForeignKey("Route")]
        public Guid RouteId { get; set; } // Khóa ngoại đến Route

        public virtual Account Account { get; set; } // Mối quan hệ với Account
        public virtual Route Route { get; set; } // Mối quan hệ với Route
        //nhieu cart nhieu koi
        public virtual List<KOI> KOIs { get; set; } = new List<KOI>(); // Danh sách Koi trong Cart
    }

}
