using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace SellingKoi.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime Registration_date { get; set; } = DateTime.Now;

        [StringLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        [Display(Name = "Trạng thái")]
        public string Status { get; set; }= "ReadyToTrip";
        //navigator attribute
        public Guid RouteId { get; set; } // Khóa ngoại đến Route
        public Route Route { get; set; }
        public List<KOI> Kois { get; set; } = new List<KOI>();

    }
}
