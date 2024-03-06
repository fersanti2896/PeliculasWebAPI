using SPeliculasAPI.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace SPeliculasAPI.DTOs {
    public class ActorCreacionDTO {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        [ArchivoValidacion(pesoMax: 4)]
        [TipoArchivoValidacion(tipoArchivo: TipoArchivo.Imagen)]
        public IFormFile FotoURL { get; set; }
    }
}
