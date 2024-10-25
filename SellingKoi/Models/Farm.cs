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
<<<<<<< HEAD
=======
         [StringLength(200)]
        public string Location { get; set; } // Thuộc tính mới

        [Range(0, int.MaxValue, ErrorMessage = "Diện tích phải là một số dương.")]
        public int Size { get; set; } // Thuộc tính mới

>>>>>>> 5b5388ad21c23fb2001d1b054751ac52623bd0d7

        // Navigation Property(1 Farm có nhiều Koi)
        public List<KOI> KOIs { get; set; } = new List<KOI>();
        
        //nhiều Farm có nhiều Route
        public List<Route> Routes { get; set; } = new List<Route>();

    }
}
