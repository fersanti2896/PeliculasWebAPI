using System.ComponentModel.DataAnnotations;

namespace SPeliculasAPI.DTOs {
    public class PeliculaActDTO {
        [Required]
        [StringLength(400)]
        public string Titulo { get; set; }
        public bool enCines { get; set; }
        public DateTime FechaEstreno { get; set; }
    }
}
