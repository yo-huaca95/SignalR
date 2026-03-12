using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace SignalR.TiempoReal.Concentradores
{
    public class ConcentradorDeTareas:Hub
    {
        public override Task OnConnected()
        {
            string usuario = Context.QueryString["usuario"];
            string rol = Context.QueryString["rol"];

            Groups.Add(Context.ConnectionId, usuario);

            // Si es administrador → unirse al grupo de administradores
            if (rol == "ADMIN")
            {
                Groups.Add(Context.ConnectionId, "Administrador");
            }


            return base.OnConnected();
        }

    }
}