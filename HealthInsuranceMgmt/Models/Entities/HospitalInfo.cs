// Models/Entities/HospitalInfo.cs
using System.ComponentModel.DataAnnotations;

namespace HealthInsuranceMgmt.Models.Entities
{
    public class HospitalInfo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string HospitalName { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [StringLength(15)]
        public string ContactNumber { get; set; } = string.Empty;

        public string? Specialities { get; set; } // e.g., Cardiology, Ortho
    }
}