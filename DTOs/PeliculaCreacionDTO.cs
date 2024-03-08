using SPeliculasAPI.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace SPeliculasAPI.DTOs {
    public class PeliculaCreacionDTO : PeliculaActDTO {
        [ArchivoValidacion(pesoMax: 4)]
        [TipoArchivoValidacion(tipoArchivo: TipoArchivo.Imagen)]
        public IFormFile Poster { get; set; }
        public List<int> GenerosIds { get; set; }
    }
}
