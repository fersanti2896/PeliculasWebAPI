using SPeliculasAPI.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace SPeliculasAPI.DTOs {
    public class ActorCreacionDTO : ActorActDTO {
        [ArchivoValidacion(pesoMax: 4)]
        [TipoArchivoValidacion(tipoArchivo: TipoArchivo.Imagen)]
        public IFormFile FotoURL { get; set; }
    }
}
