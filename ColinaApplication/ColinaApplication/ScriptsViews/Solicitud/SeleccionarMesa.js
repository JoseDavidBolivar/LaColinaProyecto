var connectSM = "";
$(function SeleccionarMesa() {
    connectSM = $.connection.solicitudhub;

    Llama_MetodosSeMe(connectSM);

    $.connection.hub.start().done(function () {
        Registra_EventosSeMe(connectSM);
    });
});

function Registra_EventosSeMe(connectsm) {


    connectsm.server.listarEstadoMesas();
}
function Llama_MetodosSeMe(connectsm) {

    connectsm.client.ListaMesas = function (data) {
        $("#ListaMesas").empty();
        var contador = 0;
        for (var i = 0; i < data.length; i++)
        {
            if (data[i].ESTADO == "LIBRE") {
                $("#ListaMesas").append('<div id=' + data[i].ID + ' onclick="alerta(this.id, \'OCUPADO\');" class="clic panel panel-success estilo" style="width: 100px; text-align: center; float: left; margin: 5px; cursor: pointer; ">' +
                                            '<div class="panel-heading">' +
                                                '<h2 class="panel-title">' + data[i].NOMBRE_MESA + '</h2>' +
                                            '</div>' +
                                            '<i class="fa fa-3x fa-check text-success"></i>' +
                                        '</div>');
            }
            if (data[i].ESTADO == "OCUPADO") {
                $("#ListaMesas").append('<div id=' + data[i].ID + ' onclick="alerta(this.id, \'NO\');"  class="panel panel-danger estilo" style="width: 100px; text-align: center; float: left; margin: 5px; cursor: pointer; ">' +
                                            '<div class="panel-heading">' +
                                                '<h2 class="panel-title">' + data[i].NOMBRE_MESA + '</h2>' +
                                            '</div>' +
                                            '<i class="fa fa-3x fa-arrow-down text-danger"></i>' +
                                        '</div>');
            }
            if (data[i].ESTADO == "ESPERA") {
                $("#ListaMesas").append('<div id=' + data[i].ID + ' onclick="alerta(this.id, \'ESPERA\');"  class="panel panel-warning estilo" style="width: 100px; text-align: center; float: left; margin: 5px; cursor: pointer; ">' +
                                            '<div class="panel-heading">' +
                                                '<h2 class="panel-title">' + data[i].NOMBRE_MESA + '</h2>' +
                                            '</div>' +
                                            '<i class="fa fa-3x fa-clock-o text-warning"></i>' +
                                        '</div>');
            }
            if (data[i].ESTADO == "NO DISPONIBLE") {
                $("#ListaMesas").append('<div id=' + data[i].ID + ' class="panel panel-default" style="width: 100px; text-align: center; float: left; margin: 5px; cursor: not-allowed; ">' +
                                            '<div class="panel-heading">' +
                                                '<h2 class="panel-title">' + data[i].NOMBRE_MESA + '</h2>' +
                                            '</div>' +
                                            '<i class="fa fa-3x fa-times-circle text-primary"></i>' +
                                        '</div>');
            }
            contador++;
            if (contador == 8) { $("#ListaMesas").append('<br/><br/><br/>'); $("#ListaMesas").css("min-height", "360px"); }
            if (contador == 16) { $("#ListaMesas").append('<br/><br/><br/>'); $("#ListaMesas").css("min-height", "460px"); }
            if (contador == 32) { $("#ListaMesas").append('<br/><br/><br/>'); $("#ListaMesas").css("min-height", "560px"); }
            if (contador == 40) { $("#ListaMesas").append('<br/><br/><br/>'); $("#ListaMesas").css("min-height", "660px"); }
            if (contador == 48) { $("#ListaMesas").append('<br/><br/><br/>'); $("#ListaMesas").css("min-height", "760px"); }
            if (contador == 56) { $("#ListaMesas").append('<br/><br/><br/>'); $("#ListaMesas").css("min-height", "860px"); }
            if (contador == 64) { $("#ListaMesas").append('<br/><br/><br/>'); $("#ListaMesas").css("min-height", "960px"); }
            if (contador == 72) { $("#ListaMesas").append('<br/><br/><br/>'); $("#ListaMesas").css("min-height", "1060px"); }
            if (contador == 80) { $("#ListaMesas").append('<br/><br/><br/>'); $("#ListaMesas").css("min-height", "1160px"); }
            if (contador == 88) { $("#ListaMesas").append('<br/><br/><br/>'); $("#ListaMesas").css("min-height", "1260px"); }
            if (contador == 96) { $("#ListaMesas").append('<br/><br/><br/>'); $("#ListaMesas").css("min-height", "1360px"); }
        }
    }
}


function alerta(id, Estado)
{
    console.log(Estado);
    if (Estado != "NO")
    {
        connectSM.server.actualizaMesa(id, Estado);
        if (Estado == "OCUPADO")
        {
            connectSM.server.insertaSolicitud(id, Estado, Iduser);
        }
    }
    window.location.href = '../Solicitud/Pedido?Id='+id;
}