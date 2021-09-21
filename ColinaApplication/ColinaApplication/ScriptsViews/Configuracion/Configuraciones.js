
$(document).ready(function () {
    $("#TablaList").DataTable({
        "language": {
            "info": "",
            "lengthMenu": "Mostrar _MENU_ registros",
            "paginate": {
                "next": ">>",
                "previous": "<<"
            },
        },
        lengthMenu: [[5, 10, 20, 25, 50, -1], [5, 10, 20, 25, 50, "Todos"]],
        
    });
});


function EditarCategoria(id, categoria, estado) {
    $("#IdCategoria").val(id);
    $("#NomCategoria").val(categoria);
    $("#EstadoCategoria").val(estado);
}
function LimpiaCamposCategoria() {
    $("#IdCategoria").val("");
    $("#NomCategoria").val("");
    $("#EstadoCategoria").val("");
}
