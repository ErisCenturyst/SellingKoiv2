using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
        public string Description { get; set; }
        public bool Status { get; set; } = true;

        // Navigation Property(1 Farm có nhiều Koi)
        public List<KOI> KOIs { get; set; } = new List<KOI>();
        
        //nhiều Farm có nhiều Route
        public List<Route> Routes { get; set; } = new List<Route>();

    }
}
