using AutoMapper;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using SPeliculasAPI.DTOs;
using SPeliculasAPI.Entidades;

namespace SPeliculasAPI.Helpers {
    public class AutoMapperProfiles : Profile {
        public AutoMapperProfiles(GeometryFactory geometryFactory) {
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

            CreateMap<Pelicula, PeliculaDetallesDTO>().ForMember(x => x.Generos, opc => opc.MapFrom(mapPelGeneros))
                                                      .ForMember(x => x.Actores, opc => opc.MapFrom(mapPelActores));

            CreateMap<SalaCine, SalaCineDTO>().ForMember(x => x.Latitud, x => x.MapFrom(y => y.Ubicacion.Y))
                                              .ForMember(x => x.Longitud, x => x.MapFrom(y => y.Ubicacion.X));

            CreateMap<SalaCineDTO, SalaCine>().ForMember(x => x.Ubicacion, x => x.MapFrom(y => geometryFactory.CreatePoint(new Coordinate(y.Longitud, y.Latitud))));
            CreateMap<SalaCineCreacionDTO, SalaCine>().ForMember(x => x.Ubicacion, x => x.MapFrom(y => geometryFactory.CreatePoint(new Coordinate(y.Longitud, y.Latitud)))); ;
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

        private List<GeneroDTO> mapPelGeneros(Pelicula pelicula, PeliculaDetallesDTO peliculaDetallesDTO) {
            var result = new List<GeneroDTO>();

            if (pelicula.PeliculasGeneros == null) { return result; }

            foreach (var pel in pelicula.PeliculasGeneros) {
                result.Add(new GeneroDTO() { Id = pel.GeneroId, Nombre = pel.Genero.Nombre });
            }

            return result;
        }

        private List<ActorPeliculaDetalleDTO> mapPelActores(Pelicula pelicula, PeliculaDetallesDTO peliculaDetallesDTO) {
            var result = new List<ActorPeliculaDetalleDTO>();

            if (pelicula.PeliculasActores == null) { return result; }

            foreach (var pel in pelicula.PeliculasActores) {
                result.Add(new ActorPeliculaDetalleDTO() { 
                    ActorId = pel.ActorId, 
                    Personaje = pel.Personaje,
                    NombrePersona = pel.Actor.Nombre
                });
            }

            return result;
        }
    }
}
