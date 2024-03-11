using Microsoft.AspNetCore.Mvc;
using SPeliculasAPI.Helpers;
using SPeliculasAPI.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace SPeliculasAPI.DTOs {
    public class PeliculaCreacionDTO : PeliculaActDTO {
        [ArchivoValidacion(pesoMax: 4)]
        [TipoArchivoValidacion(tipoArchivo: TipoArchivo.Imagen)]
        public IFormFile Poster { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> GenerosIds { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder<List<ActorPeliculaCreacionDTO>>))]
        public List<ActorPeliculaCreacionDTO> Actores { get; set; }
    }
}
