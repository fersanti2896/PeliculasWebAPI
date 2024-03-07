using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SPeliculasAPI.Services;
using System.Reflection;

namespace SPeliculasAPI {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Inyectando AutoMapper
            services.AddAutoMapper(typeof(Startup));

            // Almacenador de Archivos
            services.AddTransient<IAlmacenadorArchivoService, AlmacenadorArchivoService>();
            services.AddHttpContextAccessor();

            // Add services to the container.
            services.AddDbContext<ApplicationDbContext>(opc => opc.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers().AddNewtonsoftJson();
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Peliculas API", Version = "v1", Description = "Web API de Películas", Contact = new OpenApiContact { Email = "fersanti2896@gmail.com" } });

                //c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //{
                //    Name = "Authorization",
                //    Type = SecuritySchemeType.ApiKey,
                //    Scheme = "Bearer",
                //    BearerFormat = "JWT",
                //    In = ParameterLocation.Header
                //});

                //c.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            Reference = new OpenApiReference
                //            {
                //                Type = ReferenceType.SecurityScheme,
                //                Id = "Bearer"
                //            }
                //        },
                //        new string[]{}
                //    }
                //});

                var fileXML = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var routeXML = Path.Combine(AppContext.BaseDirectory, fileXML);
                c.IncludeXmlComments(routeXML);
            });           
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger) {

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Peliculas API V1");
                });
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors();
            app.UseAuthorization();

            app.UseEndpoints(end => {
                end.MapControllers();
            });
        }
    }
}
