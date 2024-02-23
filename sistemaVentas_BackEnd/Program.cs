using DataBaseHelper;
using Microsoft.Extensions.FileProviders;
using sistemaVentas_BackEnd.repository;

namespace sistemaVentas_BackEnd
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("appsettings.json");
            var environment = builder.Environment;
            var contentRootPath = environment.ContentRootPath;

            //acceder a IConfiguration
            var configuration = builder.Configuration;

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            //inyeccion de dependencias
            builder.Services.AddSingleton<IDataBase, BD>();
            builder.Services.AddSingleton<IClientes, ClientesRepository>();
            builder.Services.AddSingleton<ITienda, TiendaRepository>();
            builder.Services.AddSingleton<IArticulo, ArticulosRepository>();

           

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(contentRootPath, "Content")),
                RequestPath = "/Content"
            });

            app.UseHttpsRedirection();

            app.UseCors();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}