using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPeliculasAPI.DTOs;
using SPeliculasAPI.Entidades;

namespace SPeliculasAPI.Controllers {
    [ApiController]
    [Route("api/v1/generos")]
    public class GenerosController : ControllerBase {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GenerosController(
            ApplicationDbContext context,
            IMapper mapper
        ) {
            this.context = context;
            this.mapper = mapper;
        }

        /// <summary>
        /// Listado de Género de Películas
        /// </summary>
        /// <returns></returns>
        [HttpGet("listado")]
        public async Task<ActionResult<List<GeneroDTO>>> generosAll() {
            var listado = await context.Generos.ToListAsync();
            var resultado = mapper.Map<List<GeneroDTO>>(listado);

            return resultado;
        }

        /// <summary>
        /// Obtiene un género de película por su id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "ObtenerGenero")]
        public async Task<ActionResult<GeneroDTO>> obtieneGeneroById(int id) {
            var genero = await context.Generos.FirstOrDefaultAsync(g => g.Id == id);

            if(genero is null) { return NotFound("Género no encontrado"); }

            var generoDTO = mapper.Map<GeneroDTO>(genero);

            return generoDTO;
        }

        /// <summary>
        /// Crea un género de película.
        /// </summary>
        /// <param name="generoCreacionDTO"></param>
        /// <returns></returns>
        [HttpPost("crear")]
        public async Task<ActionResult> generoCreate([FromBody] GeneroCreacionDTO generoCreacionDTO) { 
            var genero = mapper.Map<Genero>(generoCreacionDTO);
            context.Add(genero);

            await context.SaveChangesAsync();

            var generoDTO = mapper.Map<GeneroDTO>(genero);

            return new CreatedAtRouteResult("ObtenerGenero", new { Id = generoDTO.Id }, generoDTO);
        }

        /// <summary>
        /// Actualiza un género de película por su id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="generoCreacionDTO"></param>
        /// <returns></returns>
        [HttpPut("actualizar/{id:int}")]
        public async Task<ActionResult> generoUpdate(int id, [FromBody] GeneroCreacionDTO generoCreacionDTO) {
            var generoExiste = await context.Generos.AnyAsync(g => g.Id == id);

            if (!generoExiste) { return NotFound("El género no existe."); }

            var genero = mapper.Map<Genero>(generoCreacionDTO);
            genero.Id = id;

            context.Entry(genero).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Elimina un género de película por su id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("eliminar/{id:int}")]
        public async Task<ActionResult> generoDelete(int id) { 
            var generoExiste = await context.Generos.AnyAsync(g => g.Id == id);

            if(!generoExiste) { return NotFound("El género no existe."); }

            context.Remove(new Genero() { Id = id });

            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
