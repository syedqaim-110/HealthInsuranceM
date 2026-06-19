// Models/ViewModels/AdminEmployeeViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace HealthInsuranceMgmt.Models.ViewModels
{
    // Yeh class sirf Admin ki form se data lene ke liye hai
    public class AdminEmployeeViewModel
    {
        public string? Id { get; set; } // Edit ke time kaam aayega

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty; // Yehi username banega

        [Required]
        [StringLength(20)]
        public string Department { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Designation { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty; // Sirf Create ke time chalega
    }
}