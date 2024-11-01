using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SellingKoi.Models
{
    public class Farm
    {
        public Guid Id { get; set; }
        [StringLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "Tên trại")]
        public string Name { get; set; }
        [StringLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "Chủ trại")]
        public string Owner { get; set; }
        [StringLength(250)]
        [Column(TypeName = "nvarchar(250)")]
        [Display(Name = "Chú thích")]
        public string? Description { get; set; }
        public bool Status { get; set; } = true;
        [StringLength(200)]
        [Display(Name = "Vị trí")]
        public string? Location { get; set; } // Thuộc tính mới

        [Range(0, int.MaxValue, ErrorMessage = "Diện tích phải là một số dương.")]
        [Display(Name = "Diện tích (m2)")]
        public int? Size { get; set; } // Thuộc tính mới
                                       //luu tru hinh anh
        [StringLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        public string? AvatarUrl { get; set; } // Thêm trường lưu hình ảnh avatar

        [NotMapped]
        public List<string> AlbumUrl { get; set; } = new List<string>(); // Thêm trường lưu các hình ảnh liên quaneeeeeeeeee


        // Navigation Property(1 Farm có nhiều Koi)
        public List<KOI> KOIs { get; set; } = new List<KOI>();

        //nhiều Farm có nhiều Route
        public List<Route> Routes { get; set; } = new List<Route>();

    }
}
