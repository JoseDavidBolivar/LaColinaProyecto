﻿var connectSM = "";
$(function SeleccionarMesa() {
    connectSM = $.connection.solicitudhub;

    Llama_MetodosSeMe(connectSM);

    $.connection.hub.start().done(function () {
        Registra_EventosSeMe(connectSM);
    });
});

function Registra_EventosSeMe(connectsm) {
    connectsm.server.listarEstadoMesas("NO", 0, "");
}
function Llama_MetodosSeMe(connectsm) {

    connectsm.client.ListaMesas = function (data, Redirecciona, idmesa, ruta) {
        $("#ListaMesas").empty();
        for (var i = 0; i < data.length; i++)
        {
            switch (data[i].ESTADO) {
                case "LIBRE":
                    $("#ListaMesas").append('<div id=' + data[i].ID + ' onclick="alerta(this.id, \'OCUPADO\');" class="clic panel panel-success estilo" style="width: 100px; text-align: center; float: left; margin: 5px; cursor: pointer; ">' +
                        '<div class="panel-heading">' +
                        '<h2 class="panel-title">' + data[i].NOMBRE_MESA + '</h2>' +
                        '</div>' +
                        '<i class="fa fa-3x fa-check text-success"></i>' +
                        '</div>');                    
                    break;
                case "OCUPADO":
                    $("#ListaMesas").append('<div id=' + data[i].ID + ' onclick="alerta(this.id, \'NO\');"  class="panel panel-danger estilo" style="width: 100px; text-align: center; float: left; margin: 5px; cursor: pointer; ">' +
                        '<div class="panel-heading">' +
                        '<h2 class="panel-title">' + data[i].NOMBRE_MESA + '</h2>' +
                        '</div>' +
                        '<i class="fa fa-3x fa-arrow-down text-danger"></i>' +
                        '</div>');
                    break;
                case "ESPERA":
                    $("#ListaMesas").append('<div id=' + data[i].ID + ' onclick="alerta(this.id, \'NO\');"  class="panel panel-warning estilo" style="width: 100px; text-align: center; float: left; margin: 5px; cursor: pointer; ">' +
                        '<div class="panel-heading">' +
                        '<h2 class="panel-title">' + data[i].NOMBRE_MESA + '</h2>' +
                        '</div>' +
                        '<i class="fa fa-3x fa-clock-o text-warning"></i>' +
                        '</div>');
                    break;
                case "NO DISPONIBLE":
                    $("#ListaMesas").append('<div id=' + data[i].ID + ' class="panel panel-default" style="width: 100px; text-align: center; float: left; margin: 5px; cursor: not-allowed; ">' +
                        '<div class="panel-heading">' +
                        '<h2 class="panel-title">' + data[i].NOMBRE_MESA + '</h2>' +
                        '</div>' +
                        '<i class="fa fa-3x fa-times-circle text-primary"></i>' +
                        '</div>');
                    break;

                default:
                    break;
            }
        }
        // SE REALIZA EL REDIRECCIONAMIENTO EN ESTE PUNTO YA QUE SI SE DEJA SOBRE ONCLICK DE CADA MESA, NO ACTUALIZA LA MESA
        if (Redirecciona == "SI" && idmesa == Id) {
            window.location.href = ruta;
        }
    }
}

var Id = 0;

function alerta(id, Estado)
{
    if (Estado != "NO")
    {
        connectSM.server.actualizaMesa(id, Estado, Iduser, "SI", "../Solicitud/Pedido?Id="+id);
        Id = id;
    }
    else
    {
        window.location.href = '../Solicitud/Pedido?Id=' + id;
    }
    
}