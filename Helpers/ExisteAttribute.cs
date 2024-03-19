using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace SPeliculasAPI.Helpers {
    public class ExisteAttribute : Attribute, IAsyncResourceFilter {
        private readonly ApplicationDbContext dbContext;

        public ExisteAttribute(ApplicationDbContext context) {
            this.dbContext = context;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next) {
            var peliculaIdObject = context.HttpContext.Request.RouteValues["peliculaId"];

            if (peliculaIdObject == null) { return; }

            var peliculaId = int.Parse(peliculaIdObject.ToString());
            
            var existePelicula = await dbContext.Peliculas.AnyAsync(x => x.Id == peliculaId);
            
            if (!existePelicula) {
                context.Result = new NotFoundResult(); 
            } else { 
                await next();
            }
        }
    }
}
