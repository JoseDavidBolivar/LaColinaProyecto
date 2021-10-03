$(document).ready(function () {  
    $(".TablaList").DataTable({
        "url": "//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json",
    });
    ValidaResultados();
});

function ValidaResultados() {
    switch (CierreCaja) {
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