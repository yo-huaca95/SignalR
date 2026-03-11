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
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
                {
                ActializarStore(StoreTareas, conexionMysql.ObtenerTareas());
                }
        }

        public void Prueba(object sender, DirectEventArgs e)
        {
            int id = conexionMysql.InsertarJob();

            var context = GlobalHost.ConnectionManager.GetHubContext<ConcentradorDeTareas>();
            context.Clients.All.nuevaTarea(new
            {
                id = id,
                tipo = "Liquidacion",
                estado = "Pendiente",
                progreso = 0
            });

            ColaDeTrabajos.Enqueue(id);

        }

        private void ActializarStore(Store store, DataTable datos)
        {
            store.DataSource = datos;
            store.DataBind();
        }

    }
}