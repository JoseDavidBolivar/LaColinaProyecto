$(function Alertas() {
    var connect = $.connection.sharedhub;

    Llama_Metodos(connect);

    $.connection.hub.start().done(function () {
        Registra_Eventos(connect);
    });
});

function Registra_Eventos(connect) {
    connect.server.hello();
}

function Llama_Metodos(connect) {

    connect.client.Hello = function (data) {
        
    }
}



function cargando() {
    $("#Cargando").css("display", "block");

}

function cerrar() {
    $("#Cargando").css("display", "none");
}
$(document).ajaxStart(function () {
    cargando();
});
$(document).ajaxStop(function () {
    cerrar();
});