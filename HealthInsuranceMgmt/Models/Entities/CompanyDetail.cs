// Models/Entities/CompanyDetail.cs
using System.ComponentModel.DataAnnotations;

namespace HealthInsuranceMgmt.Models.Entities
{
    public class CompanyDetail
    {
        [Key] // Marks this as the Primary Key
        public int Id { get; set; }

        [Required(ErrorMessage = "Company Name is required")]
        [StringLength(100)]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [StringLength(15)]
        public string ContactNumber { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        // Navigation Property: One company can have MANY policies
        public ICollection<Policy>? Policies { get; set; }
    }
}