using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LifeStreamBDMS.Models
{
    [Table("BloodInventories")] // ✅ Ensures explicit table mapping in SQL
    public class BloodInventory
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Blood Type")]
        [StringLength(5)] // ✅ Prevents invalid blood types
        public string BloodType { get; set; }

        [Required]
        [Display(Name = "Available Units")]
        [Range(0, 1000, ErrorMessage = "Quantity must be between 0 and 1000.")]
        public int AvailableUnits { get; set; } = 0;

        

        [Display(Name = "Last Updated")]
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Status")]
        [StringLength(20)]
        public string Status { get; set; } = "Adequate";

        // ✅ Determines appropriate Bootstrap class for status display
        public string GetStatusClass()
        {
            return Status switch
            {
                "Critical" => "danger",
                "Low" => "warning",
                "Moderate" => "info",
                _ => "success"
            };
        }

        // ✅ Updates inventory status dynamically based on Available Units
        public void UpdateStatus()
        {
            Status = AvailableUnits switch
            {
                <= 2 => "Critical",
                <= 5 => "Low",
                <= 10 => "Moderate",
                _ => "Adequate"
            };

            LastUpdated = DateTime.Now;
        }
    }
}
