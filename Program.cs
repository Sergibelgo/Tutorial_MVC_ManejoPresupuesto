using Microsoft.AspNetCore.Identity;
using Tutorial2ManejoPresupuesto.Models;
using Tutorial2ManejoPresupuesto.Services;

namespace Tutorial2ManejoPresupuesto
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddTransient<ITiposCuentasService, TiposCuentasService>();
            builder.Services.AddTransient<IUsuariosService, UsuariosService>();
            builder.Services.AddTransient<ICuentasService, CuentasService>();
            builder.Services.AddTransient<ICategoriasService, CategoriasService>();
            builder.Services.AddTransient<ITransaccionesService, TransaccionesService>();
            builder.Services.AddTransient<IReporteService, ReporteService>();
            builder.Services.AddTransient<IUsuariosAuthService, UsuariosAuthService>();
            //Añadir el servicio de user store
            builder.Services.AddTransient<IUserStore<Usuario>,UsuarioStore>();
            //Añadir y configurar automapper mirar el servicio AutoMapperProfiles
            builder.Services.AddAutoMapper(typeof(Program));

            //añadir entity y para configurar identity usar opciones=>{}
            builder.Services.AddIdentityCore<Usuario>(opciones =>
            {
                opciones.Password.RequireDigit = false;
                opciones.Password.RequireLowercase = false;
                opciones.Password.RequireUppercase= false;
                opciones.Password.RequireNonAlphanumeric = false;
                opciones.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Transacciones}/{action=Index}/{id?}");

            app.Run();
        }
    }
}