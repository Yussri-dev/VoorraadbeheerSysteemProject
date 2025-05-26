﻿using System.ComponentModel.DataAnnotations;

namespace VoorraadbeheerSysteemProject.Wpf.Models
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
