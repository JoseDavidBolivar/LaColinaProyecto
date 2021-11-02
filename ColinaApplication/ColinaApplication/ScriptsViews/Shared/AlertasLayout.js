var idleTime = 0;

$(function Alertas() {

    //Increment the idle time counter every minute.
    var idleInterval = setInterval(timerIncrement, 60000); // 1 minute
    //Zero the idle timer on mouse movement.
    $(this).mousemove(function (e) {
        idleTime = 0;
    });
    $(this).keypress(function (e) {
        idleTime = 0;
    });

    //HUB GLOBAL
    var connect = $.connection.sharedhub;
    Llama_Metodos(connect);

    $.connection.hub.start().done(function () {
        Registra_Eventos(connect);
    });
});

//FUNCION PARA CERRAR SESION DESPUES DE 5 MINUTOS DE INACTIVIDAD
function timerIncrement() {
    idleTime = idleTime + 1;
    if (idleTime > 4) { // 5 minutes
        document.getElementById("CerrarSesion").click();
    }
}

function Registra_Eventos(connect) {
    $('#botonCierre').click(function () {
        connect.server.refrescarMesas();
    });
}

function Llama_Metodos(connect) {

    connect.client.Mesas = function (data) {
        location.reload();
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