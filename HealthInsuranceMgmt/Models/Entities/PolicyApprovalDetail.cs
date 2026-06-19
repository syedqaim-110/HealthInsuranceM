// Models/Entities/PolicyApprovalDetail.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthInsuranceMgmt.Models.Entities
{
    public class PolicyApprovalDetail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PolicyRequestDetailId { get; set; } // FK to the request

        [ForeignKey("PolicyRequestDetailId")]
        public PolicyRequestDetail? PolicyRequest { get; set; }

        [Required]
        public string ApprovedById { get; set; } = string.Empty; // FK to Admin (EmpRegister)

        [ForeignKey("ApprovedById")]
        public EmpRegister? ApprovedBy { get; set; }

        [Required]
        public DateTime ApprovalDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty; // Approved, Rejected

        public string? Remarks { get; set; }
    }
}