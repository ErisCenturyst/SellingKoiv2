using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SellingKoi.Models
{
    public enum KoiType
    {
        Kohaku, Sanke, Showa, Utsuri, Ogon, Chagoi,Butterfly
    }

    public class KOI
    {
        public Guid Id { get; set; }
        [StringLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }
        public KoiType Type { get; set; }
        
        public int Age { get; set; }
        [StringLength(250)]
        [Column(TypeName = "nvarchar(250)")]
        public string Description { get; set; }
        public bool Status { get; set; } = true;
        public DateTime Registration_date { get; set; } = DateTime.Now;


        //luu tru hinh anh
        [StringLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string AvatarUrl { get; set; } // Thêm trường lưu hình ảnh avatar

        [NotMapped]
        public List<string> AlbumUrl { get; set; } = new List<string>(); // Thêm trường lưu các hình ảnh liên quan

        //FK
        public Guid FarmID { get; set; }
        //thuộc tính điều hướng
        public Farm Farm { get; set; }
        //nhieu koi - nhieu cart
        public virtual List<Cart> Carts { get; set; } = new List<Cart>();

    }
}
