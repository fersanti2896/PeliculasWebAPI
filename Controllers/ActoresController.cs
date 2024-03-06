using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPeliculasAPI.DTOs;
using SPeliculasAPI.Entidades;

namespace SPeliculasAPI.Controllers {
    [ApiController]
    [Route("api/v1/actores")]
    public class ActoresController : ControllerBase {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ActoresController(
            ApplicationDbContext context,
            IMapper mapper
        ) {
            this.context = context;
            this.mapper = mapper;
        }

        /// <summary>
        /// Listado de actores de una película.
        /// </summary>
        /// <returns></returns>
        [HttpGet("listado")]
        public async Task<ActionResult<List<ActorDTO>>> actoresAll() {
            var actores = await context.Actores.ToListAsync();

            return mapper.Map<List<ActorDTO>>(actores);
        }

        /// <summary>
        /// Obtiene un actor por su id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "ObtenerActor")]
        public async Task<ActionResult<ActorDTO>> actorById(int id) { 
            var actor = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);

            if(actor is null) { return NotFound($"No existe el actor con el id {id}"); }

            return mapper.Map<ActorDTO>(actor);
        }

        /// <summary>
        /// Crea un actor de una película.
        /// </summary>
        /// <param name="actorCreacionDTO"></param>
        /// <returns></returns>
        [HttpPost("crear")]
        public async Task<ActionResult> actorCreate([FromForm] ActorCreacionDTO actorCreacionDTO) {
            var actor = mapper.Map<Actor>(actorCreacionDTO);
            context.Add(actor);

            //await context.SaveChangesAsync();
            var actorDTO = mapper.Map<ActorDTO>(actor);

            return new CreatedAtRouteResult("ObtenerActor", new { Id = actorDTO.Id }, actorDTO);
        }

        /// <summary>
        /// Actualiza un actor de una película.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="actorCreacionDTO"></param>
        /// <returns></returns>
        [HttpPut("actualizar/{id:int}")]
        public async Task<ActionResult> actorUpdate(int id, [FromForm] ActorCreacionDTO actorCreacionDTO) {
            var actor = await context.Actores.FirstOrDefaultAsync(a => a.Id == id);

            if (actor is null) { return NotFound("El actor no existe."); }

            actor = mapper.Map(actorCreacionDTO, actor);
            //actor.Id = id;

            await context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Elimina un actor de película por su id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("eliminar/{id:int}")]
        public async Task<ActionResult> actorDelete(int id) {
            var actorExiste = await context.Actores.AnyAsync(g => g.Id == id);

            if (!actorExiste) { return NotFound("El actor no existe."); }

            context.Remove(new Actor() { Id = id });

            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
