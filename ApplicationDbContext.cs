using Microsoft.EntityFrameworkCore;
using SPeliculasAPI.Entidades;

namespace SPeliculasAPI {
    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
    }
}
