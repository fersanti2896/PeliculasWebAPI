using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPeliculasAPI.DTOs;
using SPeliculasAPI.Entidades;

namespace SPeliculasAPI.Controllers {
    [ApiController]
    [Route("api/v1/generos")]
    public class GenerosController : CustomBaseController {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GenerosController(
            ApplicationDbContext context,
            IMapper mapper
        ): base(context, mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        /// <summary>
        /// Listado de Género de Películas
        /// </summary>
        /// <returns></returns>
        [HttpGet("listado")]
        public async Task<ActionResult<List<GeneroDTO>>> generosAll() {
            #region Lógica anterior si no se usa el controlador CustomBaseController
            //var listado = await context.Generos.ToListAsync();
            //var resultado = mapper.Map<List<GeneroDTO>>(listado);

            //return resultado;
            #endregion

            return await Get<Genero, GeneroDTO>();
        }

        /// <summary>
        /// Obtiene un género de película por su id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "ObtenerGenero")]
        public async Task<ActionResult<GeneroDTO>> obtieneGeneroById(int id) {
            #region Lógica anterior si no se usa el controlador CustomBaseController
            //var genero = await context.Generos.FirstOrDefaultAsync(g => g.Id == id);

            //if (genero is null) { return NotFound("Género no encontrado"); }

            //var generoDTO = mapper.Map<GeneroDTO>(genero);

            //return generoDTO;
            #endregion

            return await GetById<Genero, GeneroDTO>(id);
        }

        /// <summary>
        /// Crea un género de película.
        /// </summary>
        /// <param name="generoCreacionDTO"></param>
        /// <returns></returns>
        [HttpPost("crear")]
        public async Task<ActionResult> generoCreate([FromBody] GeneroCreacionDTO generoCreacionDTO) {
            #region Lógica anterior si no se usa el controlador CustomBaseController
            //var genero = mapper.Map<Genero>(generoCreacionDTO);
            //context.Add(genero);

            //await context.SaveChangesAsync();

            //var generoDTO = mapper.Map<GeneroDTO>(genero);

            //return new CreatedAtRouteResult("ObtenerGenero", new { Id = generoDTO.Id }, generoDTO);
            #endregion

            return await Post<GeneroCreacionDTO, Genero, GeneroDTO>(generoCreacionDTO, "ObtenerGenero");
        }

        /// <summary>
        /// Actualiza un género de película por su id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="generoCreacionDTO"></param>
        /// <returns></returns>
        [HttpPut("actualizar/{id:int}")]
        public async Task<ActionResult> generoUpdate(int id, [FromBody] GeneroCreacionDTO generoCreacionDTO) {
            #region Lógica anterior sino se usa el controlador CustomBaseController
            //var generoExiste = await context.Generos.AnyAsync(g => g.Id == id);

            //if (!generoExiste) { return NotFound("El género no existe."); }

            //var genero = mapper.Map<Genero>(generoCreacionDTO);
            //genero.Id = id;

            //context.Entry(genero).State = EntityState.Modified;
            //await context.SaveChangesAsync();

            //return NoContent();
            #endregion

            return await Put<GeneroCreacionDTO, Genero>(id, generoCreacionDTO);
        }

        /// <summary>
        /// Elimina un género de película por su id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("eliminar/{id:int}")]
        public async Task<ActionResult> generoDelete(int id) {
            #region Lógica anterior sino se usa el controlador CustomBaseController
            //var generoExiste = await context.Generos.AnyAsync(g => g.Id == id);

            //if (!generoExiste) { return NotFound("El género no existe."); }

            //context.Remove(new Genero() { Id = id });

            //await context.SaveChangesAsync();

            //return NoContent();
            #endregion

            return await Delete<Genero>(id);
        }
    }
}
