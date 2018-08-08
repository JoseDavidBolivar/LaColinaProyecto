$(function PedidoSignalR() {
    var connectPSR = $.connection.solicitudhub;

    Llama_MetodosPSR(connectPSR);

    $.connection.hub.start().done(function () {
        Registra_EventosPSR(connectPSR);
    });
});

function Registra_EventosPSR(connectpsr) {

    //$('#NotificaOfertaComercialCliente').click(function () {
    //    connect.server.consultaNotificacion(Usuario);
    //});
    connectpsr.server.consultaMesaAbierta($('#ID').val());
}

function Llama_MetodosPSR(connectpsr) {

    connectpsr.client.ListaDetallesMesa = function (data) {
        //alert(data);
        if(data != null)
        {

        }
    }
}
