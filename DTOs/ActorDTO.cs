using System.ComponentModel.DataAnnotations;

namespace SPeliculasAPI.DTOs {
    public class ActorDTO {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string FotoURL { get; set; }
    }
}
