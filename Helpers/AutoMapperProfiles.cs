using AutoMapper;
using SPeliculasAPI.DTOs;
using SPeliculasAPI.Entidades;

namespace SPeliculasAPI.Helpers {
    public class AutoMapperProfiles : Profile {
        public AutoMapperProfiles() {
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Genero>();
            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreacionDTO, Actor>();
        }
    }
}
