﻿using System.ComponentModel.DataAnnotations;

namespace APIProject.DTO
{
    public class AuthenticateRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
