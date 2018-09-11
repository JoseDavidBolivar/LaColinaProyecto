var VProductosInfo = [];
var VProductosFinal = [];
var VComposiProducF = [];

var VListaSelects = [];
var CantidadColumns = 0;

$(function PedidoSignalR() {
    
    CargaProducto();
    var connectPSR = $.connection.solicitudhub;

    Llama_MetodosPSR(connectPSR);

    $.connection.hub.start().done(function () {
        Registra_EventosPSR(connectPSR);
    });


});

function Registra_EventosPSR(connectpsr) {

    $('#AgregaSubproductos').click(function () {
        if (VComposiProducF.length > 0)
        {
            alert(1);
            var CantiDispon = parseInt($("#CantidadDisponible").val());
            var CantiVender = parseInt($("#SelectCantidadVender").val());
            if (CantiDispon >= CantiVender)
            {
                var valordescontar = CantiVender;
                connectpsr.server.insertaProductosSolicitud(VProductosFinal, VComposiProducF, valordescontar, $("#ID_MESA").val());
            }
            else
            {
                alert("Debe seleccionar menos CANTIDAD a vender .....");
            }
        }
        else {
            alert("Debe seleccionar primero los productos antes de Guardar .....");
        }
    });

    connectpsr.server.consultaMesaAbierta($('#ID_MESA').val());
}

function Llama_MetodosPSR(connectpsr) {
    connectpsr.client.ListaDetallesMesa = function (data, data2) {
        if (data[0].IdMesa == $('#ID_MESA').val())
        {
            if (data.length > 0) {
                ActualizaInfoMesa(data);
                ActualizaInfoPrecios(data);
                ActualizaInfoProductos(data);
                $("#ID").val(data[0].Id);

            }
            else {
                $("#InfoMesa").empty();
                $("#InfoMesa").append('<div class="col-lg-12">' +
                        '<div class="small-box bg-green">' +
                            '<div class="inner">' +
                                '<h3>' +
                                    '#' + $('#ID_MESA').val() +
                                '</h3>' +
                                '<p>' + + '</p>' +
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
        
    }
    
    connectpsr.client.GuardoProductos = function (data) {
        alert(data);
        var x = document.getElementById("CerrarModalAP");
        x.click();
        document.getElementById("Categoria").value = '';
        $("#Subrpoductos").empty();
        $("#Subrpoductos").append("<option value=''>--SELECCIONE--</option>");
        $("#Composicion").empty();
        $("#CantidadPlatos").empty();
        $("#CantidadDisponible").val('');
        document.getElementById("SelectCantidadVender").value = '';
        $("#SelectCantidadVender").attr("disabled", "disabled");

    }

    connectpsr.client.ActualizaCantidadProductos = function (data)
    {
        if ($("#Subrpoductos").val() == data.ID)
            $("#CantidadDisponible").val(data.CANTIDAD_EXISTENCIA);
    }
    
}

function ActualizaInfoMesa(data) {
    if (data[0].EstadoSolicitud == "ABIERTA") {
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
function ActualizaInfoPrecios(data) {
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
function ActualizaInfoProductos(data) {
    $("#InfoProductos").empty();
    $("#InfoProductos").append('<table id="Tabla2" class="table table-bordered table-hover">' +
            '<thead>' +
                '<tr>' +
                    //'<th>Editar</th>' +
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
                //'<td>' +
                //    data[0].ProductosSolicitud[i].IdSubProducto +
                //'</<td>' +
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
            code = '<td><p style="background-color: #b9d5e6;padding: 5px; min-width: 160px;' +
                ' border-radius: 5px;">' + json[index].DESCRIPCION + '</p>';
        }
        else {
            code = '<td><p style="background-color: #b9d5e6;padding: 5px; min-width: 160px; ' +
                'border-radius: 5px;">' + json[index].DESCRIPCION + '</p>';
        }
        VProductosInfo[index] = {
            IdVarios: json[index].VARIOS
        };
        $("#Composicion").append(code);
        CantidadColumns++;
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
                $("#SelectCantidadVender").removeAttr("disabled");
            }
        },
        error: function (request, status, error) {
            console.log(error);
        }

    });

}

function CargaProducto() {
    $.ajax({
        type: "POST",
        url: urlListaProductos,
        contentType: "application/json; charset=utf-8",
        dataType: "JSON",
        success: function (result) {
            var json = JSON.parse(result);
            if (json.length > 0) {
                for (var index = 0, len = json.length; index < len; index++) {
                    $('#Categoria').append($('<option>', {
                        value: json[index].ID,
                        text: json[index].PRODUCTO
                    }));
                }
            }
        },
        error: function (request, status, error) {
            console.log(error);
        }

    });
}
function CargaSubProducto() {
    var idProducto = $("#Categoria").val();
    $("#Subrpoductos").empty();
    $("#Subrpoductos").append("<option value=''>--SELECCIONE--</option>");
    $("#Composicion").empty();
    $("#CantidadPlatos").empty();    
    $("#CantidadDisponible").val('');
    VComposiProducFH = [];
    document.getElementById("SelectCantidadVender").value = '';
    $("#SelectCantidadVender").attr("disabled", "disabled");

    if (idProducto != "") {
        $.ajax({
            type: "POST",
            url: urlListaSubProductos,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ IdProducto: idProducto }),
            dataType: "JSON",
            success: function (result) {
                var json = JSON.parse(result);
                if (json.length > 0) {
                    for (var index = 0, len = json.length; index < len; index++) {
                        $('#Subrpoductos').append($('<option>', {
                            value: json[index].ID,
                            text: json[index].NOMBRE_SUBPRODUCTO,
                            valorTotal: json[index].PRECIO_UNITARIO
                        }));
                    }
                }
            },
            error: function (request, status, error) {
                console.log(error);
            }

        });
    }

}

function ListarPlatosOpciones() {
    VProductosFinal = [];
    $("#CantidadPlatos").empty();
    $("#TituloPlatos").empty();
    var CantidadPlatos = $("#SelectCantidadVender").val();
    if (CantidadPlatos != "") {
        for (var i = 0; i < CantidadPlatos; i++) {
            var title = '<th><h4><b>' + $('#Categoria option:selected').text() + ' #' + (i + 1) + '</b></h4></th>';
            var code = '';
            var VListaSelectsH = [];
            for (var j = 0; j < VProductosInfo.length; j++) {
                code = code + '<td><select id="' + i + j + '" class="form-control input-sm" onchange="GuardaPlatoVector(this)">' +
                    '</select>';
                
                if (VProductosInfo[j].IdVarios == 0) {
                    VListaSelectsH[j] = {
                        IdSelect: i + '' + j + '',
                        IdSubProducto: $("#Subrpoductos").val()
                    };
                }
                else {
                    VListaSelectsH[j] = {
                        IdSelect: i + '' + j + '',
                        IdSubProducto: VProductosInfo[i, j].IdVarios
                    };
                }
            }
            VListaSelects.push(VListaSelectsH);
            $("#CantidadPlatos").append('<tr>' + title + '</tr><tr>' + code + '</tr><tr></tr>');

        }
        //console.log(VListaSelects);
        ListarSelects();

    }
}
function ListarSelects() {
    for (var i = 0; i < VListaSelects.length; i++) {
        for (var j = 0; j < VListaSelects[i].length; j++) {
            var idBusqueda = VListaSelects[i][j].IdSubProducto;
            var Select = VListaSelects[i][j].IdSelect;
            LlenaDesplegable(idBusqueda, Select);
        }
    }
    VListaSelects = [];
    var VProductoFinalH = [];
    VProductoFinalH = {
        ID_SOLICITUD: $("#ID").val(),
        ID_SUBPRODUCTO: $("#Subrpoductos").val(),
        ID_MESERO: User,
        PRECIO_PRODUCTO: $('#Subrpoductos option:selected').attr("valorTotal"),
        PRECIO_FINAL: "",
        ESTADO_PRODUCTOS: "ENTREGADO"
    };
    VProductosFinal.push(VProductoFinalH);
}
function LlenaDesplegable(idBusqueda, Select)
{
    $('#' + Select + '').empty();
    $('#'+Select+'').append("<option value=''>NO</option>");
    
    if (idBusqueda != "") {
        $.ajax({
            type: "POST",
            url: urlListaPreciosSubproductos,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ IdSubproducto: idBusqueda }),
            dataType: "JSON",
            success: function (result) {
                var json = JSON.parse(result);
                if (json.length > 0) {
                    for (var index = 0, len = json.length; index < len; index++) {
                        
                        $('#' + Select + '').append($('<option>', {
                            value: json[index].ID,
                            text: json[index].DESCRIPCION,
                            valorIndividual: json[index].PRECIO_INDIVIDUAL
                        }));
                    }
                }
            },
            error: function (request, status, error) {
                console.log(error);
            }

        });
    }
}

function GuardaPlatoVector(e) {
    var id = e.id;
    PosicionInicial = id.substring(0, 1);
    PosicionFinal = id.substring(1, 2);
    var combo = document.getElementById(id);
    var selected = combo.options[combo.selectedIndex].text;
    var VComposiProducFH = [];
    var VComposiProducFHA = [];
    var VComposiProducFHE = [];
    if (selected != "NO")
    {
        VComposiProducFHR = {
            ID: PosicionInicial,
            ID_PRODUCTO_SOLICITUD: "",
            DESCRIPCION: selected,
            VALOR: $('option:selected', e).attr("valorIndividual"),
        };
    }
    else {
        VComposiProducFHR = {};
    }

    if (VComposiProducF.length > 0)
    {
        for (var i = 0; i < VComposiProducF.length; i++)
        {
            if (VComposiProducF[i].ID == PosicionInicial)
            {
                VComposiProducFH.push(VComposiProducF[i]);
            }
        }
        if (VComposiProducFH.length > 0)
        {
            for (var j = 0; j < VComposiProducFH.length; j++)
            {
                if (j == PosicionFinal)
                {
                    VComposiProducFHA.push(VComposiProducFHR);
                    VComposiProducFHE = VComposiProducFH[j];
                }
            }
            if (VComposiProducFHA.length > 0)
            {
                for (var i = 0; i < VComposiProducF.length; i++) {
                    console.log(VComposiProducF[i]);
                    if (VComposiProducF[i] == VComposiProducFHE || VComposiProducF[i] == {})
                    {
                        VComposiProducF[i] = VComposiProducFHA[0];
                    }
                }
                VComposiProducFH = [];
                VComposiProducFHA = [];
                VComposiProducFHE = [];
            }
            else
            {
                VComposiProducF.push(VComposiProducFHR);
            }
        }
        else
        {
            VComposiProducF.push(VComposiProducFHR);
        }
        
    }
    else
    {
        VComposiProducF.push(VComposiProducFHR);
    }
    console.log(VComposiProducF);
}

function ConsultaComposicionSub()
{
    if ($('#Subrpoductos').val() != "")
    {
        document.getElementById("SelectCantidadVender").value = '';
        var id = $("#Subrpoductos").val();
        $.ajax({
            type: "POST",
            url: urlConsultaComposicionSubProducto,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ idProducto: id }),
            dataType: "JSON",
            success: function (result) {
                var json = JSON.parse(result);
                if (json.length > 0)
                {
                    TraeComposicionSeleccion(json);
                }
            },
            error: function (request, status, error) {
                console.log(error);
            }

        });
    }
    else {
        $("#CantidadPlatos").empty();
        $("#Composicion").empty();
        $("#CantidadDisponible").val('');
        document.getElementById("SelectCantidadVender").value = '';
        $("#SelectCantidadVender").attr("disabled", "disabled");
    }
    ListarPlatosOpciones();
}
