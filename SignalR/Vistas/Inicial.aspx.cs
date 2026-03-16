using Ext.Net;
using Microsoft.AspNet.SignalR;
using SignalR.Conexiones.Mysql;
using SignalR.TiempoReal.Concentradores;
using SignalR.TiempoReal.Trabajos;
using System;
using System.Data;

namespace SignalR.Vistas
{
    public partial class Inicial : System.Web.UI.Page
    {
        ConexionMysql conexionMysql = new ConexionMysql();
        protected string UsuarioActual { get; private set; }
        protected string RolActual { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            UsuarioActual = "1002936";
            RolActual = "ADMIN";
            if (!X.IsAjaxRequest)
                {
                ActializarStore(StoreTareas, conexionMysql.ObtenerTareas());
                ActializarStore(StoreTareasUsuario, conexionMysql.ObtenerTareasUsuario(UsuarioActual));
            }
        }

        public void Prueba(object sender, DirectEventArgs e)
        {
            int id = conexionMysql.InsertarJob(UsuarioActual,"");

            var context = GlobalHost.ConnectionManager.GetHubContext<ConcentradorDeTareas>();
            context.Clients.Group(UsuarioActual).nuevaTarea(new
            {
                id = id,
                tipo = "Liquidacion",
                estado = "Pendiente",
                progreso = 0,
                usuario= UsuarioActual
            });

            ColaDeTrabajos.Encolar(id);

        }

        private void ActializarStore(Store store, DataTable datos)
        {
            store.DataSource = datos;
            store.DataBind();
        }

    }
}