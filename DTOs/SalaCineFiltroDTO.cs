using System.ComponentModel.DataAnnotations;

namespace SPeliculasAPI.DTOs {
    public class SalaCineFiltroDTO {
        [Range(-90, 90)]
        public double Latitud { get; set; }
        [Range(-180, 180)]
        public double Longitud { get; set; }
        private int distanciaKm = 10;
        private int distanciaMaxKm = 50;
        public int DistanciaKM { get { return distanciaKm; } set { distanciaKm = (value > distanciaMaxKm) ? distanciaMaxKm : value; } }
    }
}
