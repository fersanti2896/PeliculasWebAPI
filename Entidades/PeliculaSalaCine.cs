namespace SPeliculasAPI.Entidades {
    public class PeliculaSalaCine {
        public int PeliculaId { get; set; }
        public int SalaCineId { get; set; }
        public Pelicula Pelicula { get; set; }
        public SalaCine SalaCine { get; set; }
    }
}
