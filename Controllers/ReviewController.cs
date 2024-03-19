using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPeliculasAPI.DTOs;
using SPeliculasAPI.Entidades;
using SPeliculasAPI.Helpers;
using System.Security.Claims;

namespace SPeliculasAPI.Controllers {
    [ApiController]
    [Route("api/v1/peliculas/{peliculaId:int}/review")]
    [ServiceFilter(typeof(ExisteAttribute))]
    public class ReviewController : CustomBaseController {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ReviewController(
            ApplicationDbContext context,
            IMapper mapper
        ) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        /// <summary>
        /// Listado de reviews de una película.
        /// </summary>
        /// <param name="peliculaId"></param>
        /// <param name="paginacionDTO"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<ReviewDTO>>> reviewsAll(int peliculaId, [FromQuery] PaginacionDTO paginacionDTO) {
            var query = context.Reviews.Include(x => x.Usuario).AsQueryable();
            query = query.Where(x => x.PeliculaId == peliculaId);

            return await GetPagination<Review, ReviewDTO>(paginacionDTO, query);
        }

        /// <summary>
        /// Creando un review para una película.
        /// </summary>
        /// <param name="peliculaId"></param>
        /// <param name="reviewCreacion"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> reviewCreate(int peliculaId, [FromBody] ReviewCreacionDTO reviewCreacion) {
            var usuarioId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
        
            var existeReview = await context.Reviews.AnyAsync(x => x.PeliculaId == peliculaId && x.UsuarioId == usuarioId);
            if (existeReview) { return BadRequest("El usuario ya ha escrito un review de esta película"); }

            var review = mapper.Map<Review>(reviewCreacion);
            review.PeliculaId = peliculaId;
            review.UsuarioId = usuarioId;

            context.Add(review);
            await context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Actualizando un review de una película.
        /// </summary>
        /// <param name="peliculaId"></param>
        /// <param name="reviewId"></param>
        /// <param name="reviewCreacion"></param>
        /// <returns></returns>
        [HttpPut("actualizar/{reviewId:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> reviewUpdate(int peliculaId, int reviewId, [FromBody] ReviewCreacionDTO reviewCreacion) {
            var reviewDB = await context.Reviews.FirstOrDefaultAsync(x => x.Id == reviewId);
            if (reviewDB is null) { return NotFound($"No existe el review con el id: {reviewId}"); }

            var usuarioId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            if (reviewDB.UsuarioId != usuarioId) { return Forbid(); }

            reviewDB = mapper.Map(reviewCreacion, reviewDB);

            await context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Elimina un review de una pelicula.
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        [HttpDelete("eliminar/{reviewId:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> reviewDelete(int reviewId) {
            var review = await context.Reviews.FirstOrDefaultAsync(x => x.Id == reviewId);
            if(review is null) { return NotFound($"No existe el review con el id: {reviewId}"); }

            var usuarioId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            if (review.UsuarioId != usuarioId) { return Forbid(); }

            context.Remove(review);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
