using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SPeliculasAPI.DTOs;
using SPeliculasAPI.Entidades;

namespace SPeliculasAPI.Controllers {
    [ApiController]
    [Route("api/v1/salacine")]
    public class SalaCineController : CustomBaseController {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public SalaCineController(ApplicationDbContext context, IMapper mapper) : base(context, mapper) {
            this.context = context;
            this.mapper = mapper;
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
    }
}
