using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SellingKoi.Models
{
    public class OrtherShorten
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string routeid { get; set; }
        
        [Required]
        public string routename { get; set; }

        public List<string> koisid { get; set; } = new List<string>();
        public List<string> koisname { get; set; } = new List<string>();

        public double Price { get; set; }

        public string buyer { get; set; }
        public string participants { get; set; }
        public string participantsPhone { get; set; }

        public DateTime Registration_date { get; set; } = DateTime.Now;
        public DateTime? DepartureFrom { get; set; }
        public DateTime? DepartureTo { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "booked";

        public string? TripId { get; set; }
        public string? TripNum { get; set; }
    }
} 