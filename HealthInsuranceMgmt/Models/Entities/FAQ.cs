// Models/Entities/FAQ.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthInsuranceMgmt.Models.Entities
{
    public class FAQ
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Question is required")]
        [StringLength(500)]
        public string Question { get; set; } = string.Empty;

        // Answer tab tak null rahega jab tak Admin reply nahi deta
        [StringLength(2000)]
        public string? Answer { get; set; }

        // Kaunse user ne poocha hai
        public string? AskedById { get; set; }
        [ForeignKey("AskedById")]
        public EmpRegister? AskedBy { get; set; }

        public DateTime AskedOn { get; set; } = DateTime.Now;
        public DateTime? AnsweredOn { get; set; }

        // Agar true hoga, tabhi public FAQ page par dikhega
        public bool IsPublished { get; set; } = false;
    }
}