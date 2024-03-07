using System.ComponentModel.DataAnnotations;

namespace SPeliculasAPI.DTOs {
    public class ActorActDTO {
        [Required]
        [StringLength(120)]
        public string Nombre { get; set; }
        public DateTime? FechaNacimiento { get; set; }
    }
}
