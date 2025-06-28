using System;
using System.ComponentModel.DataAnnotations;

namespace LifeStreamBDMS.Models
{
    public enum RequestStatus
    {
        Pending,
        Accepted,
        Rejected
    }

    public class BloodRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Donor name is required")]
        public string DonorName { get; set; }

        [Required(ErrorMessage = "Blood type is required")]
        public string BloodType { get; set; }

        [Required(ErrorMessage = "Contact number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Request date is required")]
        [DataType(DataType.Date)]
        public DateTime RequestDate { get; set; }

        [Required(ErrorMessage = "Hospital name is required")]
        public string HospitalName { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }

        public RequestStatus Status { get; set; } = RequestStatus.Pending;
    }
}
