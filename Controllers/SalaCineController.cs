using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using SPeliculasAPI.DTOs;
using SPeliculasAPI.Entidades;

namespace SPeliculasAPI.Controllers {
    [ApiController]
    [Route("api/v1/salacine")]
    public class SalaCineController : CustomBaseController {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly GeometryFactory geometryFactory;

        public SalaCineController(
            ApplicationDbContext context, 
            IMapper mapper, 
            GeometryFactory geometryFactory
        ) : base(context, mapper) {
            this.context = context;
            this.mapper = mapper;
            this.geometryFactory = geometryFactory;
        }

        /// <summary>
        /// Listado de Salas de Cine.
        /// </summary>
        /// <returns></returns>
        [HttpGet("listado")]
        public async Task<ActionResult<List<SalaCineDTO>>> salasCinesAll() {
            return await Get<SalaCine, SalaCineDTO>();
        }

        /// <summary>
        /// Obtiene una sala de cine por su id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "ObtenerSalaCine")]
        public async Task<ActionResult<SalaCineDTO>> salaCineById(int id) {
            return await GetById<SalaCine, SalaCineDTO>(id);
        }

        /// <summary>
        /// Crea una sala de cine.
        /// </summary>
        /// <param name="salaCineCreacionDTO"></param>
        /// <returns></returns>
        [HttpPost("crear")]
        public async Task<ActionResult> salaCineCreate([FromBody] SalaCineCreacionDTO salaCineCreacionDTO) {
            return await Post<SalaCineCreacionDTO, SalaCine, SalaCineDTO>(salaCineCreacionDTO, "ObtenerSalaCine");
        }

        /// <summary>
        /// Actualiza una sala de cine por su id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="salaCineCreacionDTO"></param>
        /// <returns></returns>
        [HttpPut("actualizar/{id:int}")]
        public async Task<ActionResult> salaCineUpdate(int id, [FromBody] SalaCineCreacionDTO salaCineCreacionDTO) { 
            return await Put<SalaCineCreacionDTO, SalaCine>(id, salaCineCreacionDTO);
        }

        /// <summary>
        /// Elimina una sala de cine por su id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("eliminar/{id:int}")]
        public async Task<ActionResult> salaCineDelete(int id) {
            return await Delete<SalaCine>(id);
        }

        /// <summary>
        /// Ubica salas de cines cercanos a tu longitud y latitud
        /// </summary>
        /// <param name="salaCineFiltroDTO"></param>
        /// <returns></returns>
        [HttpGet("cercano")]
        public async Task<ActionResult<List<SalaCineCercannoDTO>>> salaCineCercano([FromQuery] SalaCineFiltroDTO salaCineFiltroDTO) {
            var ubicacion = geometryFactory.CreatePoint(new Coordinate(salaCineFiltroDTO.Longitud, salaCineFiltroDTO.Latitud));

            var salaCine = await context.SalasCines.OrderBy(x => x.Ubicacion.Distance(ubicacion))
                                                   .Where(x => x.Ubicacion.IsWithinDistance(ubicacion, salaCineFiltroDTO.DistanciaKM * 1000))
                                                   .Select(x => new SalaCineCercannoDTO { 
                                                    Id = x.Id,
                                                    Nombre = x.Nombre,
                                                    Latitud = x.Ubicacion.Y,
                                                    Longitud = x.Ubicacion.X,
                                                    DistanciaMetros = Math.Round(x.Ubicacion.Distance(ubicacion))
                                                   })
                                                   .ToListAsync();

            return salaCine;
        }
    }
}
