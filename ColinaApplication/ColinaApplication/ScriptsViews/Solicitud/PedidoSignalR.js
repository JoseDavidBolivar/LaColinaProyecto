$(function PedidoSignalR() {
    var connectPSR = $.connection.solicitudhub;

    Llama_MetodosPSR(connectPSR);

    $.connection.hub.start().done(function () {
        Registra_EventosPSR(connectPSR);
    });


});

function Registra_EventosPSR(connectpsr) {

    $('#Subrpoductos').change(function () {
        connectpsr.server.consultaComposicionSubProducto($("#Subrpoductos").val());
    });

    connectpsr.server.consultaMesaAbierta($('#ID').val());
}

function Llama_MetodosPSR(connectpsr) {

    connectpsr.client.ListaDetallesMesa = function (data) {
        console.log(data);
        if (data.length > 0) {
            ActualizaInfoMesa(data);
            ActualizaInfoPrecios(data);
            ActualizaInfoProductos(data);
            
        }
        else
        {
            $("#InfoMesa").empty();
            $("#InfoMesa").append('<div class="col-lg-12">' +
                    '<div class="small-box bg-green">' +
                        '<div class="inner">' +
                            '<h3>' +
                                '#' + $('#ID').val() +
                            '</h3>' +
                            '<p>' +  + '</p>' +
                            '<p><b>Mesero:<b/> ' + + '</p>' +
                            '<p><b>C.C Cliente: </b><input type="text" class="form-control input-sm" name="CCCliente" val=""/></p>' +
                            '<p><b>Nombre Cliente: </b><input type="text" class="form-control input-sm" name="NombreCliente"  val=""/></p>' +
                        '</div>' +
                        '<div class="icon">' +
                            '<i class="fa fa-check"></i>' +
                        '</div>' +
                    '</div>' +
                '</div>');
            $("#InfoPrecios").empty();
            $("#InfoPrecios").append('<table class="table table-hover">' +
                    '<tbody>'+
                        '<tr>'+
                            '<td>'+
                                '<small>Otros Cobros: </small>' +
                                '<input type="text" class="form-control input-sm" name="OtrosCobros"/>' +
                            '</td>'+
                        '</tr>'+
                        '<tr>'+
                            '<td>'+
                                '<small>Descuentos: </small>' +
                                '<input type="text" class="form-control input-sm" name="Descuentos"/>' +
                            '</td>'+
                        '</tr>'+
                        '<tr>'+
                            '<td>'+
                                '<small>Total: </small>' +
                                '<input type="text" class="form-control input-sm" name="Total"/>' +
                            '</td>'+
                        '</tr>'+
                    '</tbody>'+
                '</table>');
            $("#InfoProductos").empty();
            $("#InfoProductos").append('<table id="Tabla2" class="table table-bordered table-hover">' +
                    '<thead>' +
                        '<tr>' +
                            '<th>Id</th>' +
                            '<th>Producto</th>' +
                            '<th>Composicion</th>' +
                            '<th>Cantidad</th>' +
                            '<th>Precio</th>' +
                        '</tr>' +
                    '</thead>' +
                    '<tbody id="BodyProductos">' +

                    '</tbody>' +
                    '</table>');

            $('#Tabla2').DataTable();
            
            
        }
    }
    connectpsr.client.ListaCompoSubProdu = function (data) {
        console.log(data);
        if (data.length > 0)
        {
            if (data[0].ID_SUBPRODUCTO == $("#Subrpoductos").val()) {
                TraeComposicionSeleccion(data);
            }
        }
    }
}

function ActualizaInfoMesa(data)
{
    if (data[0].EstadoSolicitud == "ABIERTA"){
        $("#InfoMesa").empty();
        $("#InfoMesa").append('<div class="col-lg-12">' +
                '<div class="small-box bg-danger">' +
                    '<div class="inner">' +
                        '<h3>' +
                            '#' + data[0].IdMesa +
                        '</h3>' +
                        '<p>' + data[0].NombreMesa + '</p>' +
                        '<p><b>Mesero:<b/> ' + data[0].NombreMesero + '</p>' +
                        '<p><b>C.C Cliente: </b><input type="text" class="form-control input-sm" name="CCCliente" val="' + data[0].IdentificacionCliente + '"/></p>' +
                        '<p><b>Nombre Cliente: </b><input type="text" class="form-control input-sm" name="NombreCliente"  val="' + data[0].NombreCliente + '"/></p>' +
                    '</div>' +
                    '<div class="icon">' +
                        '<i class="fa fa-arrow-down"></i>' +
                    '</div>' +
                '</div>' +
            '</div>');
    }
    if (data[0].EstadoSolicitud == "FINALIZADA") {
        $("#InfoMesa").empty();
        $("#InfoMesa").append('<div class="col-lg-12">' +
                '<div class="small-box bg-green">' +
                    '<div class="inner">' +
                        '<h3>' +
                            '#' + data[0].IdMesa +
                        '</h3>' +
                        '<p>' + data[0].NombreMesa + '</p>' +
                        '<p><b>Mesero:<b/> ' +  + '</p>' +
                        '<p><b>C.C Cliente: </b><input type="text" class="form-control input-sm" name="CCCliente" val=""/></p>' +
                        '<p><b>Nombre Cliente: </b><input type="text" class="form-control input-sm" name="NombreCliente"  val=""/></p>' +
                    '</div>' +
                    '<div class="icon">' +
                        '<i class="fa fa-check"></i>' +
                    '</div>' +
                '</div>' +
            '</div>');
    }
    if (data[0].EstadoSolicitud == "LLEVAR") {
        $("#InfoMesa").empty();
        $("#InfoMesa").append('<div class="col-lg-12">' +
                '<div class="small-box bg-warning">' +
                    '<div class="inner">' +
                        '<h3>' +
                            '#' + data[0].IdMesa +
                        '</h3>' +
                        '<p>' + data[0].NombreMesa + '</p>' +
                        '<p><b>Mesero:<b/> ' + + '</p>' +
                        '<p><b>C.C Cliente: </b><input type="text" class="form-control input-sm" name="CCCliente" val=""/></p>' +
                        '<p><b>Nombre Cliente: </b><input type="text" class="form-control input-sm" name="NombreCliente"  val=""/></p>' +
                    '</div>' +
                    '<div class="icon">' +
                        '<i class="fa fa-check"></i>' +
                    '</div>' +
                '</div>' +
            '</div>');
    }
}
function ActualizaInfoPrecios(data)
{
    $("#InfoPrecios").empty();
    $("#InfoPrecios").append('<table class="table table-hover">' +
            '<tbody>' +
                '<tr>' +
                    '<td>' +
                        '<small>Otros Cobros: </small>' +
                        '<input type="text" class="form-control input-sm" name="OtrosCobros"/>' +
                    '</td>' +
                '</tr>' +
                '<tr>' +
                    '<td>' +
                        '<small>Descuentos: </small>' +
                        '<input type="text" class="form-control input-sm" name="Descuentos"/>' +
                    '</td>' +
                '</tr>' +
                '<tr>' +
                    '<td>' +
                        '<small>Total: </small>' +
                        '<input type="text" class="form-control input-sm" name="Total"/>' +
                    '</td>' +
                '</tr>' +
            '</tbody>' +
        '</table>');
}
function ActualizaInfoProductos(data)
{
    $("#InfoProductos").empty();
    $("#InfoProductos").append('<table id="Tabla2" class="table table-bordered table-hover">' +
            '<thead>' +
                '<tr>' +
                    '<th>Id</th>' +
                    '<th>Producto</th>' +
                    '<th>Composicion</th>' +
                    '<th>Cantidad</th>' +
                    '<th>Precio</th>' +
                '</tr>' +
            '</thead>' +
            '<tbody id="BodyProductos">' +

            '</tbody>' +
            '</table>');
    $("#BodyProductos").empty();
    var count = 0;

    for (var i = 0; i < data[0].ProductosSolicitud.length; i++) {
        var code = '';
        for (var j = 0; j < data[0].ProductosSolicitud[count].CompoProductSolicitud.length; j++) {
            code = code + '&nbsp;&nbsp;' + data[0].ProductosSolicitud[count].CompoProductSolicitud[j].Descripcion + '<br/>';
        }

        $("#BodyProductos").append('<tr>' +
                '<td>' +
                    data[0].ProductosSolicitud[i].IdSubProducto +
                '</<td>' +
                '<td>' +
                    data[0].ProductosSolicitud[i].NombreSubProducto +
                '</<td>' +
                '<td>' +
                    code +
                '</<td>' +
                '<td>' +
                    '1' +
                '</<td>' +
                '<td>' +
                    data[0].ProductosSolicitud[i].PrecioFinal +
                '</<td>' +
                '</tr>');
        count++;
    }
    $('#Tabla2').DataTable();
}

function TraeComposicionSeleccion(json) {
    $("#Composicion").empty();
    for (var index = 0, len = json.length; index < len; index++) {
        var code = '';
        if (index == 0) {
            code = '<td><p valor="' + json[index].VALOR_ESTIMADO + '" style="background-color: #b9d5e6;padding: 5px; min-width: 160px;' +
                ' border-radius: 5px;">' + json[index].DESCRIPCION + '</p>';
        }
        else {
            code = '<td><p valor="' + json[index].VALOR_ESTIMADO + '" style="background-color: #b9d5e6;padding: 5px; min-width: 160px; ' +
                'border-radius: 5px; cursor: w-resize;">' + json[index].DESCRIPCION + '</p>';
        }

        $("#Composicion").append(code);
    }
    ListaCantidadDisponible();
}
function ListaCantidadDisponible(id) {
    var idSubProducto = $("#Subrpoductos").val();
    $.ajax({
        type: "POST",
        url: urlDatosElementoInventario,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ Id: idSubProducto }),
        dataType: "JSON",
        success: function (result) {
            var json = JSON.parse(result);
            if (json != null) {
                $("#CantidadDisponible").val(json.CANTIDAD_EXISTENCIA);
                $("#CantidadVender").removeAttr("ReadOnly");
            }
        },
        error: function (request, status, error) {
            console.log(error);
        }

    });

}