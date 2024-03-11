using System.ComponentModel.DataAnnotations;

namespace SPeliculasAPI.Entidades {
    public class Pelicula {
        public int Id { get; set; }
        [Required]
        [StringLength(400)]
        public string Titulo { get; set; }
        public bool enCines { get; set; }
        public DateTime FechaEstreno { get; set; }
        public string Poster { get; set; }
        public List<PeliculasActores> PeliculasActores { get; set; }
        public List<PeliculasGeneros> PeliculasGeneros { get; set; }
    }
}
