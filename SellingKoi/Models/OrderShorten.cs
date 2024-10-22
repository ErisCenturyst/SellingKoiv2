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
    }
}
