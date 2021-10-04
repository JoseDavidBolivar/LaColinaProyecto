
$(document).ready(function () {
    $(".TablaList").DataTable({
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
    ValidaResultado();
});

function ValidaResultado() {
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
                content: 'Se guardo/actualizó exitosamente el registro',
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
                content: 'Hubo un error al guardar/actualizar el registro. Intentelo nuevamente',
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
function EditarProducto(id, idCategoria, producto, precio, cantidad, descripcion) {
    $("#IdProducto").val(id);
    $("#IdCategoriaProducto").val(idCategoria);
    $("#NomProducto").val(producto);
    $("#PrecioProducto").val(precio);
    $("#CantidadProducto").val(cantidad);
    $("#DescripcionProducto").val(descripcion);
}
function LimpiaCamposProductos() {
    $("#IdProducto").val("");
    $("#IdCategoriaProducto").val("");
    $("#NomProducto").val("");
    $("#PrecioProducto").val("");
    $("#CantidadProducto").val("");
    $("#DescripcionProducto").val("");
}
function EditarMesa(id, numeromesa, idusuario, mesa, descripcion, estado) {
    $("#IdMesa").val(id);
    $("#NumeroMesa").val(numeromesa);
    $("#IdUsuarioMesa").val(idusuario);
    $("#NombreMesa").val(mesa);
    $("#DescripcionMesa").val(descripcion);
    $("#EstadoMesa").val(estado);
}
function LimpiaCamposMesa() {
    $("#IdMesa").val("");
    $("#NumeroMesa").val("");
    $("#IdUsuarioMesa").val("");
    $("#NombreMesa").val("");
    $("#DescripcionMesa").val("");
    $("#EstadoMesa").val("");
}
function EditarUsuario(id, cedula, nombre, codigo, perfil) {
    $("#IdUsuario").val(id);
    $("#CedulaUsuario").val(cedula);
    $("#NombreUsuario").val(nombre);
    $("#CodigoUsuario").val(codigo);
    $("#IdPerfilUsuario").val(perfil);
    
}
function LimpiaCamposUsuario() {
    $("#IdUsuario").val("");
    $("#CedulaUsuario").val("");
    $("#NombreUsuario").val("");
    $("#CodigoUsuario").val("");
    $("#IdPerfilUsuario").val("");
}
function EditarImpuesto(id, nombre, porcentaje, estado) {
    $("#IdImpuesto").val(id);
    $("#NombreImpuesto").val(nombre);
    $("#PorcentajeImpuesto").val(porcentaje);
    $("#EstadoImpuesto").val(estado);
}
function LimpiaCamposImpuesto() {
    $("#IdImpuesto").val("");
    $("#NombreImpuesto").val("");
    $("#PorcentajeImpuesto").val("");
    $("#EstadoImpuesto").val("");
}
function EditarPerfil(id, nombrePerfil) {    
    $("#IdPerfil").val(id);
    $("#NombrePerfil").val(nombrePerfil);
}
function LimpiaCamposPerfil() {
    $("#IdPerfil").val("");
    $("#NombrePerfil").val("");
}

function EditarNomina(id, idusuariosistema, idperfil, cedula, nombre, cargo, sueldodiario, fechanacimiento, direccion, telefono, estado) {
    $("#IdNomina").val(id);
    $("#IdUsuarioSistemaNomina").val(idusuariosistema);
    $("#IdPerfilNomina").val(idperfil);
    $("#CedulaNomina").val(cedula);
    $("#NombreNomina").val(nombre);
    $("#CargoNomina").val(cargo);
    $("#SueldoDiaNomina").val(sueldodiario);
    $("#FechaNacimientoDiaNomina").val(fechanacimiento);
    $("#DireccionResidenciaNomina").val(direccion);
    $("#TelefonoNomina").val(telefono);
    $("#EstadoNomina").val(estado);
}
function LimpiaCamposNomina() {
    $("#IdNomina").val("");
    $("#IdUsuarioSistemaNomina").val("");
    $("#IdPerfilNomina").val("");
    $("#CedulaNomina").val("");
    $("#NombreNomina").val("");
    $("#CargoNomina").val("");
    $("#SueldoDiaNomina").val("");
    $("#FechaNacimientoDiaNomina").val("");
    $("#DireccionResidenciaNomina").val("");
    $("#TelefonoNomina").val("");
    $("#EstadoNomina").val("");
}


