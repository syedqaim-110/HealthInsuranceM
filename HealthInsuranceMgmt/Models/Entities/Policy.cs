// Models/Entities/Policy.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthInsuranceMgmt.Models.Entities
{
    public class Policy
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string PolicyName { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid Insurance Company")]
        public int CompanyDetailId { get; set; } // Foreign Key property

        [ForeignKey("CompanyDetailId")]
        public CompanyDetail? Company { get; set; } // Navigation Property

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PremiumAmount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SumInsured { get; set; }

        [Required]
        [StringLength(50)]
        public string PolicyType { get; set; } = string.Empty;

        public string? Description { get; set; }

        // Navigation Properties
        public ICollection<PolicyRequestDetail>? PolicyRequests { get; set; }
        public ICollection<PolicyOnEmployee>? AssignedEmployees { get; set; }
    }
}