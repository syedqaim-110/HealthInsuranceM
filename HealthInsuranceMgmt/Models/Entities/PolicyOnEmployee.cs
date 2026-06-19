// Models/Entities/PolicyOnEmployee.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthInsuranceMgmt.Models.Entities
{
    public class PolicyOnEmployee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string EmpRegisterId { get; set; } = string.Empty; // FK to Employee

        [ForeignKey("EmpRegisterId")]
        public EmpRegister? Employee { get; set; }

        [Required]
        public int PolicyId { get; set; } // FK to Policy

        [ForeignKey("PolicyId")]
        public Policy? Policy { get; set; }

        [Required]
        public DateTime AssignedDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Active"; // Active, Expired, Claimed

        // Finance tracking
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ClaimAmountCredited { get; set; }

        public DateTime? ClaimSettledDate { get; set; }
    }
}