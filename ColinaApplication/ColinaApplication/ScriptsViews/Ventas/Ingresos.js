reImprimir$(document).ready(function () {  
    $(".TablaList").DataTable({
        "url": "//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json",
    });
    ValidaResultados();    
});

function ValidaResultados() {
    switch (CierreCaja) {
        case "True":
            if (ValueCajaText == "CERRAR CAJA") {
                $.alert({
                    theme: 'Modern',
                    icon: 'fa fa-check',
                    boxWidth: '500px',
                    useBootstrap: false,
                    type: 'green',
                    title: 'Caja abierta !',
                    content: 'La caja se encuentra abierta para registrar pedidos',
                    buttons: {
                        Continuar: {
                            btnClass: 'btn btn-success',
                            action: function () {

                            }
                        }
                    }
                });
            }
            else {
                $.alert({
                    theme: 'Modern',
                    icon: 'fa fa-times',
                    boxWidth: '500px',
                    useBootstrap: false,
                    type: 'default',
                    title: 'Caja cerrada !',
                    content: 'La caja se encuentra cerrada. No pueden registrar pedidos a sus mesas asignadas',
                    buttons: {
                        Continuar: {
                            btnClass: 'btn btn-default',
                            action: function () {

                            }
                        }
                    }
                });
            }
            break;
        case "False":
            $.alert({
                theme: 'Modern',
                icon: 'fa fa-times',
                boxWidth: '500px',
                useBootstrap: false,
                type: 'red',
                title: 'Error !',
                content: 'Ha ocurrido un error inesperado. Por favor intentelo de nuevo ',
                buttons: {
                    Continuar: {
                        btnClass: 'btn btn-danger',
                        action: function () {

                        }
                    }
                }
            });
            break;
        case "Mesas Abiertas":
            $.alert({
                theme: 'Modern',
                icon: 'fa fa-times',
                boxWidth: '500px',
                useBootstrap: false,
                type: 'red',
                title: 'Mesas abiertas !',
                content: 'Existen mesas abiertas, debe asignarlas a otro usuario o finalizarlas para poder cerrar caja',
                buttons: {
                    Continuar: {
                        btnClass: 'btn btn-danger',
                        action: function () {

                        }
                    }
                }
            });
            break;

        default:
    }
}

function reImprimir() {
    $.ajax({
        type: "POST",
        url: urlreImprimir,
        contentType: "application/json; charset=utf-8",
        dataType: "JSON",
        success: function (result) {
            var json = JSON.parse(result);
            if (json == true) {
                $.alert({
                    theme: 'Modern',
                    icon: 'fa fa-check',
                    boxWidth: '500px',
                    useBootstrap: false,
                    type: 'green',
                    title: 'Cierre Impreso !',
                    content: 'Se imprimio el ticket de CIERRE',
                    buttons: {
                        Continuar: {
                            btnClass: 'btn btn-success',
                            action: function () {

                            }
                        }
                    }
                });
            }
            else {
                $.alert({
                    theme: 'Modern',
                    icon: 'fa fa-times',
                    boxWidth: '500px',
                    useBootstrap: false,
                    type: 'red',
                    title: 'Cierre Impreso !',
                    content: 'Error al imprimir el ticket de CIERRE. Intente nuevamente',
                    buttons: {
                        Continuar: {
                            btnClass: 'btn btn-danger',
                            action: function () {

                            }
                        }
                    }
                });
            }
            
        },
        error: function (request, status, error) {
            console.log(error);
        }

    });
}

function ImprimirParcial() {
    $.ajax({
        type: "POST",
        url: urlImprimirParcial,
        contentType: "application/json; charset=utf-8",
        dataType: "JSON",
        success: function (result) {
            var json = JSON.parse(result);
            if (json == true) {
                $.alert({
                    theme: 'Modern',
                    icon: 'fa fa-check',
                    boxWidth: '500px',
                    useBootstrap: false,
                    type: 'green',
                    title: 'Cierre Parcial Impreso !',
                    content: 'Se imprimio el ticket de CIERRE PARCIAL',
                    buttons: {
                        Continuar: {
                            btnClass: 'btn btn-success',
                            action: function () {

                            }
                        }
                    }
                });
            }
            else {
                $.alert({
                    theme: 'Modern',
                    icon: 'fa fa-times',
                    boxWidth: '500px',
                    useBootstrap: false,
                    type: 'red',
                    title: 'Cierre Parcial Impreso !',
                    content: 'Error al imprimir el ticket de CIERRE PARCIAL. Intente nuevamente',
                    buttons: {
                        Continuar: {
                            btnClass: 'btn btn-danger',
                            action: function () {

                            }
                        }
                    }
                });
            }

        },
        error: function (request, status, error) {
            console.log(error);
        }

    });
}

