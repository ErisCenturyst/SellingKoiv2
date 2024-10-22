using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SellingKoi.Models
{
    public class Account
    {
        public Guid Id { get; set; }

        [StringLength(30)]
        [Column(TypeName = "nvarchar(30)")]
        public required string Username { get; set; }
        [StringLength(30)]
        [Column(TypeName = "nvarchar(30)")]
        public required string Password { get; set; }
        
        [StringLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public  required string Fullname { get; set; }
        [StringLength(10)]
        [Column(TypeName = "nvarchar(10)")]

        public required string Role { get; set; }
        
        public Guid CartId { get; set; }
        public virtual Cart Cart { get; set; }


        public bool Status { get; set; } = true;
        public DateTime Registration_date { get; set; } = DateTime.Now;



    }
}
