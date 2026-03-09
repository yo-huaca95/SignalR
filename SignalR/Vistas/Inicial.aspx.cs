using Ext.Net;
using Microsoft.AspNet.SignalR;
using SignalR.Conexiones.Mysql;
using SignalR.Hubs;
using System;

namespace SignalR.Vistas
{
    public partial class Inicial : System.Web.UI.Page
    {
        ConexionMysql conexionMysql = new ConexionMysql();
        protected void Page_Load(object sender, EventArgs e)
        {
            WorkerManager.Iniciar();
        }

        public async void Prueba(object sender, DirectEventArgs e)
        {
            //var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            //for (int i = 0; i <= 100; i += 10)
            //{
            //    await System.Threading.Tasks.Task.Delay(500);// Simula trabajo pesado

            //    context.Clients.All.recibirProgreso(i);
            //}

            //context.Clients.All.recibirMensaje("Proceso terminado ✅");

            int id = conexionMysql.InsertarJob();

            JobQueue.Enqueue(id);

        }

        //[DirectMethod]
        //public void CrearJob()
        //{
        //    int id = conexionMysql.InsertarJob();

        //    JobQueue.Enqueue(id);
        //}
    }
}