<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="Inicial.aspx.cs" Inherits="SignalR.Vistas.Inicial" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Probando SignalR</title>
    <script>
        var renderizarEstado = function (value, metadata, record, rowIndex, colIndex, store, view) {
            switch (value) {
                default:
                    return value;
                case 'Pendiente':
                    return '<img src="' + Ext.net.ResourceMgr.getIconUrl("Hourglass") + '" width=16 height=16>';
                case 'Generando':
                    return '<div class="loading" width=16 height=16 />';
                case 'Error':
                    return '<img src="' + Ext.net.ResourceMgr.getIconUrl("Decline") + '" width=16 height=16>';
                case 'Cancelado':
                case 'Abortado':
                    return '<img src="' + Ext.net.ResourceMgr.getIconUrl("Decline") + '" width=16 height=16>';
                case 'Terminado':
                    return '<img src="' + Ext.net.ResourceMgr.getIconUrl("Tick") + '" width=16 height=16>';
            }
        };

    </script>
    <style type="text/css">
        
        .loading {
            font-size: 11px;
            background-image: url(/Recursos/Imagenes/loading.gif);
            background-repeat: no-repeat;
            background-position: top top;
            vertical-align: middle;
            display: block;
            width: 20px;
            height: 20px;
        }
    </style>
</head>

<body>
    <ext:ResourceManager runat="server" />
    <form id="form1" runat="server">
      
        <ext:Window runat="server" Title="Tareas" Layout="VBoxLayout" UI="Primary" TitleAlign="Center" >
            <CustomConfig>
                <ext:ConfigItem Name="width" Mode="Value" Value="90%"></ext:ConfigItem>
                 <ext:ConfigItem Name="height" Mode="Value" Value="90%"></ext:ConfigItem>
            </CustomConfig>
            <LayoutConfig>
                <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
            </LayoutConfig>
            <Items>
                <ext:GridPanel runat="server" Flex="1" Title="Tareas Generales" >
                   
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" Text="Tarea" DataIndex="id" Width="75"></ext:Column>
                             <ext:Column runat="server" Text="Tipo De Tarea" DataIndex="tipo" Width="150"></ext:Column>
                            <ext:Column runat="server" Text="&nbsp;" DataIndex="estado" Width="50">
                                <Renderer Fn="renderizarEstado" />
                            </ext:Column>
                          
                            <ext:ProgressBarColumn runat="server" Text="&nbsp;" DataIndex="progreso" Flex="1">
                            </ext:ProgressBarColumn>
                        </Columns>
                    </ColumnModel>
                    <Store>
                        <ext:Store runat="server" ID="StoreTareas" PageSize="9">
                            <Model>
                                <ext:Model runat="server" IDProperty="id" >
                                    <Fields>
                                        <ext:ModelField Name="id"></ext:ModelField>
                                        <ext:ModelField Name="tipo"></ext:ModelField>
                                        <ext:ModelField Name="estado"></ext:ModelField>
                                        <ext:ModelField Name="progreso"></ext:ModelField>
                                        <ext:ModelField Name="fecha_creacion"></ext:ModelField>
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters >
                                <ext:DataSorter Property="id" Direction="ASC"></ext:DataSorter>
                            </Sorters>
                        </ext:Store>
                        
                    </Store>
                    <View>
                        <ext:GridView runat="server" EmptyText="Sin Tareas" StripeRows="true">
                            
                        </ext:GridView>
                    </View>
                    <BottomBar>
                        <ext:PagingToolbar runat="server" HideRefresh="true" Weight="10" ></ext:PagingToolbar>
                    </BottomBar>
                </ext:GridPanel>
                <ext:GridPanel runat="server" Flex="1" Title="Tareas Usuario 1" >
                   
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" Text="Tarea" DataIndex="id" Width="75"></ext:Column>
                             <ext:Column runat="server" Text="Tipo De Tarea" DataIndex="tipo" Width="150"></ext:Column>
                            <ext:Column runat="server" Text="&nbsp;" DataIndex="estado" Width="50">
                                <Renderer Fn="renderizarEstado" />
                            </ext:Column>
                          
                            <ext:ProgressBarColumn runat="server" Text="&nbsp;" DataIndex="progreso" Flex="1">
                            </ext:ProgressBarColumn>
                        </Columns>
                    </ColumnModel>
                    <Store>
                        <ext:Store runat="server" ID="StoreTareasUsuario" PageSize="9">
                            <Model>
                                <ext:Model runat="server" IDProperty="id" >
                                    <Fields>
                                        <ext:ModelField Name="id"></ext:ModelField>
                                        <ext:ModelField Name="tipo"></ext:ModelField>
                                        <ext:ModelField Name="estado"></ext:ModelField>
                                        <ext:ModelField Name="progreso"></ext:ModelField>
                                        <ext:ModelField Name="fecha_creacion"></ext:ModelField>
                                    </Fields>
                                </ext:Model>
                            </Model>
                            <Sorters >
                                <ext:DataSorter Property="id" Direction="ASC"></ext:DataSorter>
                            </Sorters>
                        </ext:Store>
                        
                    </Store>
                    <View>
                        <ext:GridView runat="server" EmptyText="Sin Tareas" StripeRows="true">
                            
                        </ext:GridView>
                    </View>
                    <BottomBar>
                        <ext:PagingToolbar runat="server" HideRefresh="true" Weight="10" ></ext:PagingToolbar>
                    </BottomBar>
                </ext:GridPanel>
                <ext:Button Text="PruebaCrearTarea" runat="server">
                    <DirectEvents>
                        <Click OnEvent="Prueba" ></Click>
                    </DirectEvents>
                </ext:Button>
            </Items>
        </ext:Window>

    </form>
<script src="/Scripts/jquery-3.7.0.min.js"></script>
<script src="/Scripts/jquery.signalR-2.4.3.min.js"></script>
<script src="/signalr/hubs"></script>

<script type="text/javascript">
    let usuarioActual = '<%= UsuarioActual %>';
    let rolS = '<%= RolActual %>';

    let actualizarVisual = {

        NuevoRegistroStore: function (store, datos) {
            store.add({
                id: datos.id,
                tipo: datos.tipo,
                estado: datos.estado,
                progreso: 0
            });
        },
        ActualizarFilaDeStore: function (store, fila, columna, valor) {

            var record = store.getById(fila);
            
            if (record) {
                if (columna == 'progreso') {
                    valor = valor / 100;
                }
                record.set(columna, valor);
                record.commit();
            }
        }
    }

    $(function () {


        $.connection.hub.qs = {
            "usuario": usuarioActual,
            "rol": rolS  // ADMIN o USER
        };



        // Pasar el usuario al hub via query string
        /*$.connection.hub.qs = { "usuario": usuarioActual };*/


        var concentrador = $.connection.concentradorDeTareas;

        concentrador.client.nuevaTarea = function (tarea) {
            var store = App.StoreTareas;

            if (rolS == 'ADMIN') {
                actualizarVisual.NuevoRegistroStore(App.StoreTareas,tarea);
            }

            // Usuario específico
            actualizarVisual.NuevoRegistroStore(App.StoreTareasUsuario, tarea);
            


        }

        concentrador.client.recibirProgreso = function (tareaId,valor) {

            //var progress = App["Progress_" + jobId];
            if (rolS == 'ADMIN') {
                actualizarVisual.ActualizarFilaDeStore(App.StoreTareas, tareaId,'progreso', valor);
            }
            // Usuario específico
            actualizarVisual.ActualizarFilaDeStore(App.StoreTareasUsuario, tareaId, 'progreso', valor);
        };

        concentrador.client.actualizarEstado = function (tareaId, estado) {
            
            if (rolS == 'ADMIN') {
                actualizarVisual.ActualizarFilaDeStore(App.StoreTareas, tareaId, 'estado', estado);
            }
            // Usuario específico
            actualizarVisual.ActualizarFilaDeStore(App.StoreTareasUsuario, tareaId, 'estado', estado);
        }

        concentrador.client.recibirMensaje = function (mensaje) {
            Ext.Msg.alert("Estado", mensaje);
        };

        $.connection.hub.start().done(function () {
            console.log("Conectado con el usuario: " + usuarioActual);
        });

    });
</script>
</body>
</html>
