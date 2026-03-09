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
      
        <ext:Window runat="server" Title="Ventana de Acciones" Layout="VBoxLayout" UI="Primary" >
            <CustomConfig>
                <ext:ConfigItem Name="width" Mode="Value" Value="90%"></ext:ConfigItem>
                 <ext:ConfigItem Name="height" Mode="Value" Value="90%"></ext:ConfigItem>
            </CustomConfig>
            <LayoutConfig>
                <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
            </LayoutConfig>
            <Items>
                <ext:GridPanel runat="server" Title="Tareas" Flex="1">
                   
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" Text="IdTarea" DataIndex="id"></ext:Column>
                            <ext:Column runat="server" Text="Estado" DataIndex="estado"></ext:Column>
                            <ext:ProgressBarColumn runat="server" Text="Progreso" DataIndex="progreso">

                            </ext:ProgressBarColumn>
                        </Columns>
                    </ColumnModel>
                    <Store>
                        <ext:Store runat="server">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="id"></ext:ModelField>
                                        <ext:ModelField Name="tipo"></ext:ModelField>
                                        <ext:ModelField Name="estado"></ext:ModelField>
                                        <ext:ModelField Name="progreso"></ext:ModelField>
                                        <ext:ModelField Name="fecha_creacion"></ext:ModelField>
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <View>
                        <ext:GridView runat="server" EmptyText="Sin Tareas" StripeRows="true">
                            
                        </ext:GridView>
                    </View>
                </ext:GridPanel>
                <ext:Button Text="PruebaCrearTarea" runat="server">
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
    
        chat.client.recibirProgreso = function (jobId,valor) {

            var progress = App["Progress_" + jobId];

            if (progress) {
                progress.updateProgress(valor / 100, valor + "%");
            }

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
