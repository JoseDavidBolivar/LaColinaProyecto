var connectSM = "";
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
        for (var i = 0; i < data.length; i++) {
            switch (data[i].ESTADO) {
                case "LIBRE":
                    $("#ListaMesas").append('<div id=' + data[i].ID + ' onclick="alerta(this.id, \'OCUPADO\');" class="card text-white bg-success estilo" style="width: 150px; text-align: center; float: left; margin: 5px; cursor: pointer; ">' +
                        '<div class="card-header">' +
                        '<h2 style="font-size: 25px; overflow: hidden; text-overflow: ellipsis; white-space: pre;">' + data[i].NOMBRE_MESA + '</h2>' +
                        '</div>' +
                        '<br/><i class="fa fa-3x fa-check"></i><br/>' +
                        '</div>');
                    break;
                case "OCUPADO":
                    $("#ListaMesas").append('<div id=' + data[i].ID + ' onclick="alerta(this.id, \'NO\');"  class="card text-white bg-danger estilo" style="width: 150px; text-align: center; float: left; margin: 5px; cursor: pointer; ">' +
                        '<div class="card-header">' +
                        '<h2 style="font-size: 25px; overflow: hidden; text-overflow: ellipsis; white-space: pre;">' + data[i].NOMBRE_MESA + '</h2>' +
                        '</div>' +
                        '<br/><i class="fa fa-3x fa-arrow-down"></i><br/>' +
                        '</div>');
                    break;
                case "ESPERA":
                    $("#ListaMesas").append('<div id=' + data[i].ID + ' onclick="alerta(this.id, \'NO\');"  class="card text-dark bg-warning estilo" style="width: 150px; text-align: center; float: left; margin: 5px; cursor: pointer; ">' +
                        '<div class="card-header">' +
                        '<h2 style="font-size: 25px; overflow: hidden; text-overflow: ellipsis; white-space: pre;">' + data[i].NOMBRE_MESA + '</h2>' +
                        '</div>' +
                        '<br/><i class="fa fa-3x fa-clock-o"></i><br/>' +
                        '</div>');
                    break;
                case "NO DISPONIBLE":
                    $("#ListaMesas").append('<div id=' + data[i].ID + ' class="card text-white bg-secondary" style="width: 150px; text-align: center; float: left; margin: 5px; cursor: not-allowed; ">' +
                        '<div class="card-header">' +
                        '<h2 style="font-size: 25px; overflow: hidden; text-overflow: ellipsis; white-space: pre;">' + data[i].NOMBRE_MESA + '</h2>' +
                        '</div>' +
                        '<br/><i class="fa fa-3x fa-times-circle"></i><br/>' +
                        '</div>');
                    break;
                case "CERRADO":
                    $("#ListaMesas").append('<div id=' + data[i].ID + ' class="card text-dark bg-light" style="width: 150px; text-align: center; float: left; margin: 5px; cursor: not-allowed; ">' +
                        '<div class="card-header">' +
                        '<h2 style="font-size: 25px; overflow: hidden; text-overflow: ellipsis; white-space: pre;">' + data[i].NOMBRE_MESA + '</h2>' +
                        '</div>' +
                        '<br/><i class="fa fa-3x fa-times text-muted"></i><br/>' +
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

function alerta(id, Estado) {
    if (Estado != "NO") {
        Encriptar(id).then(r => {
            connectSM.server.actualizaMesa(id, Estado, Iduser, "SI", "../Solicitud/Pedido?Id=" + encodeURIComponent(r));
            Id = id;
        }).catch(() => {
            console.log('error');
        });
        
    }
    else {
        Encriptar(id).then(r => {
            window.location.href = '../Solicitud/Pedido?Id=' + encodeURIComponent(r);
        }).catch(() => {
            console.log('error');
        });
    }
}

function Encriptar(texto) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: urlEncriptar,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ Texto: texto }),
            dataType: "JSON",
            success: function (result) {
                var json = JSON.parse(result);
                if (json != "") {
                    resolve(json);
                }
                else {
                    reject();
                }

            },
            error: function (request, status, error) {
                console.log(error);
            }

        });
    })
}

