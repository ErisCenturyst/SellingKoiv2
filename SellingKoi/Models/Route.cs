using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SellingKoi.Models
{
    public class Route
    {
        public Guid Id { get; set; }
        [StringLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "Lộ trình")]
        public string Name { get; set; }
        //public List<Farm> Farms { get; set; }
        public bool Status { get; set; } = true;
        public DateTime Registration_date { get; set; } = DateTime.Now;
        [Display(Name = "Giá tiền")]
        [Column(TypeName = "decimal(12, 2)")]
        public double Price { get; set; }
        //luu tru hinh anh
        //[StringLength(500)]
        //[Column(TypeName = "nvarchar(500)")]
        //public string? AvatarUrl { get; set; } // Thêm trường lưu hình ảnh avatar

        //[NotMapped]
        //public List<string> AlbumUrl { get; set; } = new List<string>(); // Thêm trường lưu các hình ảnh liên quan

        //Navigation Property
        public List<Farm> Farms { get; set; } = new List<Farm>();
        public List<Cart>? Carts { get; set; }

    }
}
