using System.ComponentModel.DataAnnotations;

namespace SPeliculasAPI.DTOs {
    public class RolDTO {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
