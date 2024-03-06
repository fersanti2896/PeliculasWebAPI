using System.ComponentModel.DataAnnotations;

namespace SPeliculasAPI.DTOs {
    public class ActorCreacionDTO {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
    }
}
