using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace SPeliculasAPI.Entidades {
    public class SalaCine : IId {
        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Nombre { get; set; }
        public Point Ubicacion { get; set; }
        public List<PeliculaSalaCine> PeliculaSalaCines { get; set; }
    }
}
