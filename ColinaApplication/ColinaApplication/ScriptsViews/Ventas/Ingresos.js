$(document).ready(function () {  
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
    switch (Resultado) {
        case "True":
            Resultado = "";
            $.alert({
                theme: 'Modern',
                icon: 'fa fa-check',
                boxWidth: '500px',
                useBootstrap: false,
                type: 'green',
                title: 'Exitoso !',
                content: 'Se actualizó exitosamente el registro',
                buttons: {
                    Continuar: {
                        btnClass: 'btn btn-success',
                        action: function () {
                            window.location.href = '#' + Posicion;
                        }
                    }
                }
            });
            break;
        case "False":
            Resultado = "";
            $.alert({
                theme: 'Modern',
                icon: 'fa fa-times',
                boxWidth: '500px',
                useBootstrap: false,
                type: 'red',
                title: 'Error !',
                content: 'Hubo un error al actualizar el registro o debes seleccionar una factura antes de guardar. Intentelo nuevamente',
                buttons: {
                    Continuar: {
                        btnClass: 'btn btn-danger',
                        action: function () {
                            window.location.href = '#' + Posicion;
                        }
                    }
                }
            });
            break;

        default:
            break;
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

function ReImprimirFactura(NumFact) {
    $.alert({
        theme: 'Modern',
        icon: 'fa fa-list-alt',
        boxWidth: '500px',
        useBootstrap: false,
        type: 'gray',
        title: 'Factura !',
        content: 'Desea imprimir la factura ?',
        buttons: {
            Si: {
                btnClass: 'btn btn-default',
                action: function () {
                    $.ajax({
                        type: "POST",
                        url: urlImprimirFactura,
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({ IdFactura: NumFact }),
                        dataType: "JSON",
                        success: function (result) {
                            var json = JSON.parse(result);
                            if (json == true) {

                            }
                            else {

                            }

                        },
                        error: function (request, status, error) {
                            console.log(error);
                        }

                    });
                }
            },
            Cancelar: {
                btnClass: 'btn btn-default',
                action: function () {

                }
            },
        }
    });
    
}

function EditarFactura(idFact, otrosCobros, descuentos, servicio, total, subtotal) {
    $("#IdFactura").val(idFact);
    $("#OtrosCobros").val(otrosCobros);
    $("#Descuentos").val(descuentos);
    $("#PorcentajeServicio").val(servicio);
    var servicioT = (servicio * subtotal)/100;
    $("#ServicioTotal").val(parseInt(servicioT));
    $("#TotalCalc").html(total);
    $("#Total").val(total);
    $("#Subtotal").val(subtotal);
}
function LimpiaCamposFactura() {
    $("#IdFactura").val("");
    $("#OtrosCobros").val("");
    $("#Descuentos").val("");
    $("#PorcentajeServicio").val("");
    $("#ServicioTotal").val("");
    $("#TotalCalc").html("");
    $("#Total").val("");
    $("#Subtotal").val("");
}
function CalculosFactura() {
    var TotalF = parseInt($("#OtrosCobros").val()) - parseInt($("#Descuentos").val()) + ((parseInt($("#PorcentajeServicio").val()) * parseInt($("#Subtotal").val())) / 100) + parseInt($("#Subtotal").val());
    $("#TotalCalc").html(TotalF);
    $("#Total").val(TotalF);
}


