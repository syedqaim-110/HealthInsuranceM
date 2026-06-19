// Models/Entities/EmpRegister.cs
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HealthInsuranceMgmt.Models.Entities
{
    // Inheriting from IdentityUser gives us Id, UserName, Email, PasswordHash, etc.
    // We just add the extra fields an Employee needs.
    public class EmpRegister : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Department { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Designation { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        public DateTime DateOfJoining { get; set; } = DateTime.Now;

        // Navigation Properties
        public ICollection<PolicyRequestDetail>? PolicyRequests { get; set; }
        public ICollection<PolicyOnEmployee>? AssignedPolicies { get; set; }
    }
}