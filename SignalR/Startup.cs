using Microsoft.Owin;
using Owin;
using SignalR.TiempoReal.Trabajos;

[assembly: OwinStartup(typeof(SignalR.Startup))]
namespace SignalR
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            GestorDeTrabajadores.Iniciar();
        }
    }
}