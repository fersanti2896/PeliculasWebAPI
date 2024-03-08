using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPeliculasAPI.DTOs;
using SPeliculasAPI.Entidades;
using SPeliculasAPI.Helpers;
using SPeliculasAPI.Services;

namespace SPeliculasAPI.Controllers {
    [ApiController]
    [Route("api/v1/peliculas")]
    public class PeliculasController : ControllerBase {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivoService almacenadorArchivoService;
        private readonly string contenedor = "Peliculas";

        public PeliculasController(
            ApplicationDbContext context,
            IMapper mapper,
            IAlmacenadorArchivoService almacenadorArchivoService
        ) {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivoService = almacenadorArchivoService;
        }

        /// <summary>
        /// Listado de películas con paginación.
        /// </summary>
        /// <param name="paginacionDTO"></param>
        /// <returns></returns>
        [HttpGet("listado")]
        public async Task<ActionResult<List<PeliculaDTO>>> peliculasAll([FromQuery] PaginacionDTO paginacionDTO) {
            var query = context.Peliculas.AsQueryable();

            await HttpContext.paginacionCabecera(query);

            var peliculas = await query.OrderBy(a => a.Id)
                                     .Paginar(paginacionDTO)
                                     .ToListAsync();

            return mapper.Map<List<PeliculaDTO>>(peliculas);
        }

        /// <summary>
        /// Obtiene una película por su id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "ObtenerPelicula")]
        public async Task<ActionResult<PeliculaDTO>> peliculaById(int id) { 
            var pelicula = await context.Peliculas.FirstOrDefaultAsync(x => x.Id == id);

            if(pelicula is null) { return NotFound($"No existe la película con el id {id}"); }

            return mapper.Map<PeliculaDTO>(pelicula);
        }

        /// <summary>
        /// Crea una película.
        /// </summary>
        /// <param name="peliculaCreacionDTO"></param>
        /// <returns></returns>
        [HttpPost("crear")]
        public async Task<ActionResult> peliculaCreate([FromForm] PeliculaCreacionDTO peliculaCreacionDTO) {
            var pelicula = mapper.Map<Pelicula>(peliculaCreacionDTO);

            if (peliculaCreacionDTO.Poster != null) {
                using (var memoryStream = new MemoryStream()) { 
                    await peliculaCreacionDTO.Poster.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(peliculaCreacionDTO.Poster.FileName);

                    pelicula.Poster = await almacenadorArchivoService.GuardarArchivo(contenido, extension, contenedor, peliculaCreacionDTO.Poster.ContentType);
                }
            }

            context.Add(pelicula);

            await context.SaveChangesAsync();
            var peliculaDTO = mapper.Map<PeliculaDTO>(pelicula);

            return new CreatedAtRouteResult("ObtenerPelicula", new { Id = peliculaDTO.Id }, peliculaDTO);
        }

        /// <summary>
        /// Actualiza una película por su id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="peliculaCreacionDTO"></param>
        /// <returns></returns>
        [HttpPut("actualizar/{id:int}")]
        public async Task<ActionResult> peliculaUpdate(int id, [FromForm] PeliculaCreacionDTO peliculaCreacionDTO) {
            var pelicula = await context.Peliculas.FirstOrDefaultAsync(x => x.Id == id);

            if (pelicula is null) { return NotFound($"No existe la película con el id {id}"); }

            pelicula = mapper.Map(peliculaCreacionDTO, pelicula);
            pelicula.Id = id;

            if (peliculaCreacionDTO.Poster != null) {
                using (var memoryStream = new MemoryStream()) {
                    await peliculaCreacionDTO.Poster.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(peliculaCreacionDTO.Poster.FileName);

                    pelicula.Poster = await almacenadorArchivoService.EditarArchivo(contenido, extension, contenedor, pelicula.Poster, peliculaCreacionDTO.Poster.ContentType);
                }
            }

            await context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Elimina una película por su id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("eliminar/{id:int}")]
        public async Task<ActionResult> peliculaDelete(int id) {
            var peliculaExiste = await context.Peliculas.AnyAsync(g => g.Id == id);

            if (!peliculaExiste) { return NotFound($"La película con el id: {id} no existe."); }

            context.Remove(new Pelicula() { Id = id });

            await context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Actualización parcial de una pelicula.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [HttpPatch("parcial/{id:int}")]
        public async Task<ActionResult> peliculaPartial(int id, [FromBody] JsonPatchDocument<PeliculaActDTO> patchDocument) {
            if (patchDocument == null) { return BadRequest(); }

            var pelicula = await context.Peliculas.FirstOrDefaultAsync(x => x.Id == id);

            if(pelicula is null) { return NotFound($"No existe la película con el id {id}"); }

            var peliculaDTO = mapper.Map<PeliculaActDTO>(pelicula);
            patchDocument.ApplyTo(peliculaDTO, ModelState);

            var isValid = TryValidateModel(peliculaDTO);

            if(!isValid) { return BadRequest(ModelState); }

            mapper.Map(peliculaDTO, pelicula);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
