namespace SPeliculasAPI.DTOs {
    public class FiltroPeliculaDTO {
        public int Pagina { get; set; } = 1;
        public int Elementos { get; set; } = 10;
        public PaginacionDTO Paginacion { get { return new PaginacionDTO() { Pagina = Pagina, Elementos = Elementos }; } }
        public string Titulo { get; set; }
        public int GeneroId { get; set; }
        public bool enCines { get; set; }
        public bool ProximosEstrenos { get; set; }
        public string OrdenarCampo { get; set; }
        public bool Ordenacion { get; set; }
    }
}
