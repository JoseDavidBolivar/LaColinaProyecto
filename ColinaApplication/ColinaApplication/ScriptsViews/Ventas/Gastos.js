$(document).ready(function () {
    $(".TablaList").DataTable({
        "url": "//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json",
    });
});

function AsignarDia(idnomina) {
    $.alert({
        theme: 'Modern',
        icon: 'fa fa-calendar',
        boxWidth: '500px',
        useBootstrap: false,
        type: 'orange',
        title: 'Fecha trabajada !',
        content: 'Por favor seleccione una fecha de trabajo: <br/><label><input id="FechaTrabajada" type="date" class="form-control input-sm"></label><br>',
        buttons: {
            Continuar: {
                btnClass: 'btn btn-warning',
                action: function () {
                    if ($("#FechaTrabajada").val() != "") {
                        $.ajax({
                            type: "POST",
                            url: urlAsignarDiaTrabajo,
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify({ IdNomina: idnomina, FechaTrabajada: $("#FechaTrabajada").val() }),
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
                                        title: 'Exitoso !',
                                        content: 'Trabajador asignado a fecha seleccionada',
                                        buttons: {
                                            Continuar: {
                                                btnClass: 'btn btn-success',
                                                action: function () {
                                                    location.reload();
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
                                        title: 'Error Actualización !',
                                        content: 'La fecha seleccionada puede que ya se haya ingresado',
                                        buttons: {
                                            Si: {
                                                btnClass: 'btn btn-danger',
                                                action: function () {
                                                    location.reload();
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
                    else {
                        AsignarDia(idnomina);
                    }
                }
            }
        }
    });

}

function CalcularSueldo(idUsuarioNomina) {
    $.ajax({
        type: "POST",
        url: urlCalcularPago,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ IdNomina: idUsuarioNomina }),
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
                    title: 'Exitoso !',
                    content: 'Calculo exitoso',
                    buttons: {
                        Continuar: {
                            btnClass: 'btn btn-success',
                            action: function () {
                                location.reload();
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
                    title: 'Error Calculando !',
                    content: 'Hubo un error inesperado, por favor intentelo nuevamente',
                    buttons: {
                        Si: {
                            btnClass: 'btn btn-danger',
                            action: function () {
                                location.reload();
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
function Liquidar(idUsuarioNomina) {
    $.ajax({
        type: "POST",
        url: urlLiquidarUsuario,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ IdNomina: idUsuarioNomina }),
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
                    title: 'Exitoso !',
                    content: 'Trabajador Liquidado',
                    buttons: {
                        Continuar: {
                            btnClass: 'btn btn-success',
                            action: function () {
                                location.reload();
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
                    title: 'Error Liquidando !',
                    content: 'Hubo un error inesperado, por favor intentelo nuevamente',
                    buttons: {
                        Si: {
                            btnClass: 'btn btn-danger',
                            action: function () {
                                location.reload();
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