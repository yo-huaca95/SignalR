<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="Inicial.aspx.cs" Inherits="SignalR.Vistas.Inicial" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Probando SignalR</title>
</head>

<body>
    <ext:ResourceManager runat="server" />
    <form id="form1" runat="server">
      
        <ext:Window runat="server" Title="Ventana de Acciones" Layout="VBoxLayout" Height="500" Width="500" UI="Primary">
            <Items> 
                <ext:Button Text="Prueba" runat="server">
                    <DirectEvents>
                        <Click OnEvent="Prueba" ></Click>
                    </DirectEvents>
                </ext:Button>
                <ext:ProgressBar
                    ID="ProgressBar1"
                    runat="server"
                    Width="400"
                    Value="0" />
            </Items>
        </ext:Window>

    </form>
<script src="/Scripts/jquery-3.7.0.min.js"></script>
<script src="/Scripts/jquery.signalR-2.4.3.min.js"></script>
<script src="/signalr/hubs"></script>

<script>
    $(function () {

        var chat = $.connection.chatHub;
    
        chat.client.recibirProgreso = function (valor) {

            var progress = App.ProgressBar1;

            progress.updateProgress(valor / 100, valor + "%");

        };
        chat.client.recibirMensaje = function (mensaje) {
            Ext.Msg.alert("Estado", mensaje);
        };

        $.connection.hub.start().done(function () {
            console.log("Conectado");
        });

    });
</script>
</body>
</html>
