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
            CreateMap<PeliculaCreacionDTO, Pelicula>().ForMember(x => x.Poster, opc => opc.Ignore());
            CreateMap<PeliculaActDTO, Pelicula>().ReverseMap();

        }
    }
}
