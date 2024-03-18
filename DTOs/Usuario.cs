﻿using System.ComponentModel.DataAnnotations;

namespace SPeliculasAPI.DTOs {
    public class Usuario {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
