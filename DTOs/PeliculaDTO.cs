namespace SPeliculasAPI.DTOs {
    public class PeliculaDTO {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public bool enCines { get; set; }
        public DateTime FechaEstreno { get; set; }
        public string Poster { get; set; }
    }
}
