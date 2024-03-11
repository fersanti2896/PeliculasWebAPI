using AutoMapper;
using SPeliculasAPI.DTOs;
using SPeliculasAPI.Entidades;

namespace SPeliculasAPI.Helpers {
    public class AutoMapperProfiles : Profile {
        public AutoMapperProfiles() {
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Genero>();

            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreacionDTO, Actor>().ForMember(x => x.FotoURL, opc => opc.Ignore());
            CreateMap<ActorActDTO, Actor>().ReverseMap();

            CreateMap<Pelicula, PeliculaDTO>().ReverseMap();
            CreateMap<PeliculaCreacionDTO, Pelicula>().ForMember(x => x.Poster, opc => opc.Ignore())
                                                      .ForMember(x => x.PeliculasGeneros, opc => opc.MapFrom(mapPeliculaGenero))
                                                      .ForMember(x => x.PeliculasActores, opc => opc.MapFrom(mapPeliculaActor));
            CreateMap<PeliculaActDTO, Pelicula>().ReverseMap();

        }

        private List<PeliculasGeneros> mapPeliculaGenero(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula) {
            var result = new List<PeliculasGeneros>();

            if (peliculaCreacionDTO.GenerosIds == null) { return result; }

            foreach (var id in peliculaCreacionDTO.GenerosIds) {
                result.Add(new PeliculasGeneros() { GeneroId = id });
            }

            return result;
        }

        private List<PeliculasActores> mapPeliculaActor(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula) { 
            var result = new List<PeliculasActores>();

            if(peliculaCreacionDTO.Actores == null) { return result; }

            foreach (var actor in peliculaCreacionDTO.Actores) {
                result.Add(new PeliculasActores() { ActorId = actor.ActorId, Personaje = actor.Personaje });
            }

            return result;
        }
    }
}
