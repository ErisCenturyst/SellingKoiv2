using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SellingKoi.Models
{
    public class OrderShorten
    {
        public Guid Id { get; set; }
        public DateTime Registration_date { get; set; } = DateTime.Now;

        [StringLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        [Display(Name = "Trạng thái")]
        public string Status { get; set; } = "paid";
        //paid = readytotrip, beingstrip,waittoship,done
        public string routeid { get; set; }
        public string routename { get; set; }
        public List<string> koisid { get; set; }
        public List<string> koisname { get; set; }
        public double Price { get; set; }
        public string buyer {  get; set; }
        //FK
        public Guid ?TripId { get; set; } // Đảm bảo rằng kiểu dữ liệu là Guid
        public Trip ?Trip { get; set; } // Mối quan hệ với Trip

    }
}
