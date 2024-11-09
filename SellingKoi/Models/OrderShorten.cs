using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SellingKoi.Models
{
    public class OrderShorten
    {
        public Guid Id { get; set; }
        public DateTime Registration_date { get; set; } = DateTime.Now;

        [StringLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        [Display(Name = "Trạng thái")]
        public string Status { get; set; } = "booked";
        //booked,paid = readytotrip, beingstrip,waittoship,done
        public string routeid { get; set; }
        public string routename { get; set; }
        public List<string>? koisid { get; set; }
        public List<string>? koisname { get; set; }
        public double Price { get; set; }
        public string? buyer { get; set; }
        public string? TripId { get; set; }
        public string? TripNum { get; set; }
        public string? participants { get; set; }
        public string? participantsPhone { get;set; }
        public DateTime? DepartureFrom { get; set; }
        public DateTime? DepartureTo { get; set; }
        //FK
    }
}
