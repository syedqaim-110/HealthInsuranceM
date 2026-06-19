// Models/Entities/PolicyRequestDetail.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthInsuranceMgmt.Models.Entities
{
    public class PolicyRequestDetail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string EmpRegisterId { get; set; } = string.Empty; // FK to AspNetUsers (our EmpRegister)

        [ForeignKey("EmpRegisterId")]
        public EmpRegister? Employee { get; set; }

        [Required]
        public int PolicyId { get; set; } // FK to Policy

        [ForeignKey("PolicyId")]
        public Policy? Policy { get; set; }

        [Required]
        public DateTime RequestDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Under Review, Approved, Rejected

        public string? Remarks { get; set; }

        // Navigation property to approval details
        public PolicyApprovalDetail? ApprovalDetail { get; set; }
    }
}