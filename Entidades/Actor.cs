using System.ComponentModel.DataAnnotations;

namespace SPeliculasAPI.Entidades {
    public class Actor {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string FotoURL { get; set; }
        public List<PeliculasActores> PeliculasActores { get; set; }
    }
}
