﻿using System.ComponentModel.DataAnnotations;

namespace LifeStreamBDMS.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } // "Admin" or "User"
    }
}
