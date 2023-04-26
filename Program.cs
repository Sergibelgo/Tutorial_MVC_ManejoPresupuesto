using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
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
            builder.Services.AddTransient<IUserStore<Usuario>, UsuarioStore>();
            //Añadir y configurar automapper mirar el servicio AutoMapperProfiles
            builder.Services.AddAutoMapper(typeof(Program));
            //Añadir el servicio de login hay que añadir tanto el transient de SignInManager<T> y AddHttpContextAccessor()
            builder.Services.AddTransient<SignInManager<Usuario>>(); //Añadir servicio de login
            builder.Services.AddHttpContextAccessor();

            //añadir entity y para configurar identity usar opciones=>{}
            builder.Services.AddIdentityCore<Usuario>(opciones =>
            {
                opciones.Password.RequireDigit = false;
                opciones.Password.RequireLowercase = false;
                opciones.Password.RequireUppercase = false;
                opciones.Password.RequireNonAlphanumeric = false;
            }).AddErrorDescriber<MensajesDeErrorIdentity>(); //Esto sirve para añadir los mensajes personalizados de error creados en el servicio MensajesDeErrorIdentity
            //Añadir el sistema de autentificacion al programa
            builder.Services.AddAuthentication(options =>
            {
                //Añade el esquema por defecto de autentificacion de identity
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignOutScheme = IdentityConstants.ApplicationScheme;
            }).AddCookie(IdentityConstants.ApplicationScheme); //Esto creara la cookie

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

            //AÑADIR ESTO PARA USAR AUTENTIFICACION
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Transacciones}/{action=Index}/{id?}");

            app.Run();
        }
    }
}