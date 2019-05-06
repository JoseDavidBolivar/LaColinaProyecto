var VProductosInfo = [];
var VProductosFinal = [];
var VComposiProducF = [];

var VListaSelects = [];
var CantidadColumns = 0;
var TotalGeneral = 0;
var connectPSR;

$(function PedidoSignalR() {
    
    CargaProducto();
    connectPSR = $.connection.solicitudhub;

    Llama_MetodosPSR(connectPSR);

    $.connection.hub.start().done(function () {
        Registra_EventosPSR(connectPSR);
    });
    
});

function Registra_EventosPSR(connectpsr) {
    $('#AgregaSubproductos').click(function ()
    {
        cargando();
        if (VComposiProducF.length > 0)
        {
            var CantiDispon = parseInt($("#CantidadDisponible").val());
            var CantiVender = parseInt($("#SelectCantidadVender").val());
            if (CantiDispon >= CantiVender)
            {
                $('#AgregaSubproductos').attr("disabled", "true");
                connectpsr.server.insertaProductosSolicitud(VProductosFinal, VComposiProducF, $("#ID_MESA").val());
                VProductosInfo = [];
                VProductosFinal = [];
                VComposiProducF = [];

                VListaSelects = [];

            }
            else
            {
                //alert("Debe seleccionar menos CANTIDAD a vender .....");
                $.alert({
                    theme: 'Modern',
                    icon: 'fa fa-times',
                    boxWidth: '500px',
                    useBootstrap: false,
                    type: 'red',
                    title: 'Oops',
                    content: 'Debe seleccionar menos CANTIDAD a vender',
                    buttons: {
                        Ok: {
                            btnClass: 'btn-danger',
                            action: function () {

                            }
                        }
                    }
                });
            }
        }
        else {
            //alert("Debe seleccionar primero los productos antes de Guardar .....");
            $.alert({
                theme: 'Modern',
                icon: 'fa fa-times',
                boxWidth: '500px',
                useBootstrap: false,
                type: 'red',
                title: 'Oops',
                content: 'Debe seleccionar primero los productos antes de Guardar',
                buttons: {
                    Ok: {
                        btnClass: 'btn-danger',
                        action: function () {

                        }
                    }
                }
            });
        }
        cerrar();
    });

    connectpsr.server.consultaMesaAbierta($('#ID_MESA').val());
}

function Llama_MetodosPSR(connectpsr) {
    connectpsr.client.ListaDetallesMesa = function (data) {
        if (data[0].IdMesa == $('#ID_MESA').val())
        {
            if (data.length > 0) {
                ActualizaInfoMesa(data);
                ActualizaInfoPrecios(data);
                ActualizaInfoProductos(data);
                $("#ID").val(data[0].Id);

            }
            $("#GuardaDatosCliente").removeAttr("disabled");
        }
        
    }
    
    connectpsr.client.GuardoProductos = function (data) {
        //alert(data);
        document.getElementById("Categoria").value = '';
        $("#Subrpoductos").empty();
        $("#Subrpoductos").append("<option value=''>--SELECCIONE--</option>");
        $("#Composicion").empty();
        $("#CantidadPlatos").empty();
        $("#CantidadDisponible").val('');
        document.getElementById("SelectCantidadVender").value = '';
        $("#SelectCantidadVender").attr("disabled", "disabled");
        $('#AgregaSubproductos').removeAttr("disabled");
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
                        '<p><b>C.C Cliente: </b><input id="CCCliente" type="text" class="form-control input-sm" name="CCCliente" value="' + data[0].IdentificacionCliente + '" onkeypress = "return soloNum(event)" onpaste="return false"/></p>' +
                        '<p><b>Nombre Cliente: </b><input id="NombreCliente" type="text" class="form-control input-sm" name="NombreCliente"  value="' + data[0].NombreCliente + '" /></p>' +
                    '</div>' +
                    '<div class="icon">' +
                        '<i class="fa fa-arrow-down"></i>' +
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
                        '<p><b>C.C Cliente: </b><input id="CCCliente" type="text" class="form-control input-sm" name="CCCliente" value="' + data[0].IdentificacionCliente + '" onkeypress = "return soloNum(event)" onpaste="return false"/></p>' +
                        '<p><b>Nombre Cliente: </b><input id="NombreCliente" type="text" class="form-control input-sm" name="NombreCliente"  value="' + data[0].NombreCliente + '" /></p>' +
                    '</div>' +
                    '<div class="icon">' +
                        '<i class="fa fa-clock-o"></i>' +
                    '</div>' +
                '</div>' +
            '</div>');
    }
    $("#ESTADO_SOLICITUD").val(data[0].EstadoSolicitud)
    $("#OBSERVACIONES").val(data[0].Observaciones);
}
function ActualizaInfoPrecios(data) {
    console.log(data);
    $("#InfoPrecios").empty();
    $("#InfoPrecios").append('<table class="table table-hover">' +
            '<tbody>' +
                '<tr>' +
                    '<td>' +
                        '<small>Otros Cobros: </small>' +
                        '<input id="OtrosCobros" type="text" class="form-control input-sm" name="OtrosCobros" value="' + data[0].OtrosCobros + '" onchange="SumaTotal()" onkeypress = "return soloNum(event)" onpaste="return false"/>' +
                    '</td>' +
                '</tr>' +
                '<tr>' +
                    '<td>' +
                        '<small>Descuentos: </small>' +
                        '<input id="Descuentos" type="text" class="form-control input-sm" name="Descuentos" value="' + data[0].Descuentos + '" onchange="SumaTotal()" onkeypress = "return soloNum(event)" onpaste="return false"/>' +
                    '</td>' +
                '</tr>' +
                '<tr>' +
                    '<td>' +
                        '<small>Total: </small>' +
                        '<input id="Total" type="text" class="form-control input-sm" name="Total" value="' + data[0].Total + '" ReadOnly="true"/>' +
                    '</td>' +
                '</tr>' +
            '</tbody>' +
        '</table>');
    TotalGeneral = data[0].Total;
    SumaTotal();
    
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
    $('#Tabla2').DataTable({
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json"
        }
    });
    //Content/plugins/datatables/js/Spanish.json
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
    VProductosFinal = [];
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
    VProductosFinal = [];
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
    VComposiProducFH = [];
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
                if (json.length > 0)
                {
                    for (var index = 0, len = json.length; index < len; index++)
                    {
                        if (json[index].CANTIDAD_PORCION > 0 && json[index].ID_SUBPRODUCTO == 7)
                        {
                            $('#' + Select + '').append($('<option>', {
                                value: json[index].ID,
                                text: json[index].DESCRIPCION,
                                valorIndividual: json[index].PRECIO_INDIVIDUAL
                            }));
                        }
                        else
                        {
                            if (json[index].ID_SUBPRODUCTO != 7)
                            {
                                $('#' + Select + '').append($('<option>', {
                                    value: json[index].ID,
                                    text: json[index].DESCRIPCION,
                                    valorIndividual: json[index].PRECIO_INDIVIDUAL
                                }));
                            }
                        }
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

function GuardarDatosCliente()
{
    $("#GuardaDatosCliente").attr("disabled","true");
    connectPSR.server.guardaDatosCliente($("#ID").val(), $("#CCCliente").val(), $("#NombreCliente").val(), $("#OBSERVACIONES").val(), $("#OtrosCobros").val(), $("#Descuentos").val(),
        $("#Total").val(), $("#ESTADO_SOLICITUD").val(), $("#ID_MESA").val());
    $.alert({
        theme: 'Modern',
        icon: 'fa fa-check',
        boxWidth: '500px',
        useBootstrap: false,
        type: 'green',
        title: 'Super!',
        content: "Datos Guardados !",
        buttons: {
            Continuar: {
                btnClass: 'btn-success',
                action: function () {

                }
            }
        }
    });
}
function PagarFactura()
{
    $.alert({
        theme: 'Modern',
        icon: 'fa fa-question',
        boxWidth: '500px',
        useBootstrap: false,
        type: 'green',
        title: 'Vale !',
        content: 'Desea pagar cuenta ? ',
        buttons: {
            Si: {
                btnClass: 'btn btn-success',
                action: function () {
                    $.alert({
                        theme: 'Modern',
                        icon: 'fa fa-list-alt',
                        boxWidth: '500px',
                        useBootstrap: false,
                        type: 'orange',
                        title: 'Factura !',
                        content: 'Desea Imprimir la Factura ? ',
                        buttons: {
                            Si: {
                                btnClass: 'btn btn-warning',
                                action: function () {
                                    //Imprime Factura
                                    connectPSR.server.guardaDatosCliente($("#ID").val(), $("#CCCliente").val(), $("#NombreCliente").val(), $("#OBSERVACIONES").val(), $("#OtrosCobros").val(), $("#Descuentos").val(),
                                        $("#Total").val(), "FINALIZADA", $("#ID_MESA").val());
                                    connectPSR.server.actualizaMesa($("#ID_MESA").val(), "LIBRE", User);
                                    $.alert({
                                        theme: 'Modern',
                                        icon: 'fa fa-check',
                                        boxWidth: '500px',
                                        useBootstrap: false,
                                        type: 'green',
                                        title: 'Super !',
                                        content: 'La cuenta fue pagada exitosamente !',
                                        buttons: {
                                            Continuar: {
                                                btnClass: 'btn btn-success',
                                                action: function () {
                                                    window.location.href = '../Solicitud/SeleccionarMesa';
                                                }
                                            },
                                        }
                                    });
                                }
                            },
                            No: {
                                btnClass: 'btn btn-warning',
                                action: function () {
                                    connectPSR.server.guardaDatosCliente($("#ID").val(), $("#CCCliente").val(), $("#NombreCliente").val(), $("#OBSERVACIONES").val(), $("#OtrosCobros").val(), $("#Descuentos").val(),
                                        $("#Total").val(), "FINALIZADA", $("#ID_MESA").val());
                                    connectPSR.server.actualizaMesa($("#ID_MESA").val(), "LIBRE", User);
                                    $.alert({
                                        theme: 'Modern',
                                        icon: 'fa fa-check',
                                        boxWidth: '500px',
                                        useBootstrap: false,
                                        type: 'green',
                                        title: 'Super !',
                                        content: 'La cuenta fue pagada exitosamente !',
                                        buttons: {
                                            Continuar: {
                                                btnClass: 'btn btn-success',
                                                action: function () {
                                                    window.location.href = '../Solicitud/SeleccionarMesa';
                                                }
                                            },
                                        }
                                    });
                                    
                                }
                            },
                            Cancelar: {
                                btnClass: 'btn btn-warning',
                                action: function () {

                                }
                            }
                        }
                    });
                }
            },
            Cancelar: {
                btnClass: 'btn btn-success',
                action: function () {

                }
            }
        }
    });
}
function GeneraFactura()
{
    $.alert({
        theme: 'Modern',
        icon: 'fa fa-list-alt',
        boxWidth: '500px',
        useBootstrap: false,
        type: 'gray',
        title: 'Factura !',
        content: 'Desea imprimir la factura !',
        buttons: {
            Si: {
                btnClass: 'btn btn-default',
                action: function () {
                    //Imprime Factura
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
function CancelaPedido()
{
    $.alert({
        theme: 'Modern',
        icon: 'fa fa-question',
        boxWidth: '500px',
        useBootstrap: false,
        type: 'red',
        title: 'Cancelar !',
        content: 'Desea cancelar todo el pedido ?',
        buttons: {
            Si: {
                btnClass: 'btn btn-danger',
                action: function () {
                    connectPSR.server.guardaDatosCliente($("#ID").val(), $("#CCCliente").val(), $("#NombreCliente").val(), $("#OBSERVACIONES").val(), $("#OtrosCobros").val(), $("#Descuentos").val(),
                                        $("#Total").val(), "CANCELA PEDIDO", $("#ID_MESA").val());
                    connectPSR.server.cancelaPedido($("#ID").val());
                    connectPSR.server.actualizaMesa($("#ID_MESA").val(), "LIBRE", User);
                    $.alert({
                        theme: 'Modern',
                        icon: 'fa fa-check',
                        boxWidth: '500px',
                        useBootstrap: false,
                        type: 'green',
                        title: 'Super !',
                        content: 'La cuenta fue cancelada totalmente !',
                        buttons: {
                            Continuar: {
                                btnClass: 'btn btn-success',
                                action: function () {
                                    window.location.href = '../Solicitud/SeleccionarMesa';
                                }
                            },
                        }
                    });
                }
            },
            Cancelar: {
                btnClass: 'btn btn-danger',
                action: function () {

                }
            },
        }
    });
}

function SumaTotal()
{
    if ($("#Descuentos").val() != "" && $("#OtrosCobros").val() != "")
    {
        var TotalFinal = parseInt(TotalGeneral) - parseInt($("#Descuentos").val()) + parseInt($("#OtrosCobros").val());
        $("#Total").val(TotalFinal);
    }
    else {
        if ($("#Descuentos").val() == "") { $("#Descuentos").val(0) }
        if ($("#OtrosCobros").val() == "") { $("#OtrosCobros").val(0) }
        SumaTotal();
    }
}

function soloNum(e) {
    var key = window.Event ? e.which : e.keyCode
    return (key >= 48 && key <= 57)
}