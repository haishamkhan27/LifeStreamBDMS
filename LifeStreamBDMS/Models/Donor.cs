using System;
using System.ComponentModel.DataAnnotations;

namespace LifeStreamBDMS.Models
{
    public class Donor
    {
        [Key] // Defines Primary Key
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Blood Type is required")]
        public string BloodType { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Last Donation Date is required")]
        [DataType(DataType.Date)]
        public DateTime LastDonationDate { get; set; }

        [Required(ErrorMessage = "Contact Number is required")]
        public string ContactNumber { get; set; }
    }
}
