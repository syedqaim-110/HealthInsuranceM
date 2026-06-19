// Models/Entities/PolicyTotalDescription.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthInsuranceMgmt.Models.Entities
{
    public class PolicyTotalDescription
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PolicyId { get; set; } // FK to Policy

        [ForeignKey("PolicyId")]
        public Policy? Policy { get; set; }

        [Required]
        [StringLength(100)]
        public string CoverageType { get; set; } = string.Empty; // e.g., Hospitalization, Surgery

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CoverageAmount { get; set; }

        public string? Description { get; set; }
    }
}