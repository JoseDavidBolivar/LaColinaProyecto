
var TotalGeneral = 0;
var connectPSR;

$(function PedidoSignalR() {

    CargaCategorias();
    connectPSR = $.connection.solicitudhub;

    Llama_MetodosPSR(connectPSR);

    $.connection.hub.start().done(function () {
        Registra_EventosPSR(connectPSR);
    });

});

//SE ENVIAN LOS METODOS AL HUB
function Registra_EventosPSR(connectpsr) {
    //BOTON DE GUARDAR PRODUCTOS
    $('#AgregaProductos').click(function () {
        cargando();
        if ($('#ID_PRODUCTO').val() != "" && $('#PRECIO_PRODUCTO').val() != "") {
            var CantiVender = parseInt($("#contador").val());
            var model = {
                ID_SOLICITUD: $('#ID').val(),
                ID_PRODUCTO: $('#ID_PRODUCTO').val(),
                ID_MESERO: User,
                PRECIO_PRODUCTO: $('#PRECIO_PRODUCTO').val(),
                DESCRIPCION: $('#Adiciones').val()
            };
            //ENVIA AL METODO GUARDAR
            connectpsr.server.insertaProductosSolicitud(model, CantiVender, $('#ID_MESA').val());
            
        }
        else {
            $.alert({
                theme: 'Modern',
                icon: 'fa fa-times',
                boxWidth: '500px',
                useBootstrap: false,
                type: 'red',
                title: 'Oops',
                content: 'Debe seleccionar un producto antes de guardar !',
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

    //CONsULTA MESA ABIERTA EN PANTALLA
    connectpsr.server.consultaMesaAbierta($('#ID_MESA').val());
}

//SE EJECUTAN LOS METODOS QUE ENVIAN DESDE EL HUB
function Llama_MetodosPSR(connectpsr) {
    //LISTA LOS DETALLES DE LA MESA
    connectpsr.client.ListaDetallesMesa = function (data) {
        if (data[0].IdMesa == $('#ID_MESA').val()) {
            if (data.length > 0) {
                ActualizaInfoMesa(data);
                ActualizaInfoPrecios(data);
                ActualizaInfoProductos(data);
                $("#ID").val(data[0].Id);
            }
            $("#GuardaDatosCliente").removeAttr("disabled");
        }

    }

    //RECIBE DEL METODO CUANDO GUARDA PRODUCTOS
    connectpsr.client.GuardoProductos = function (data) {
        if (data == "Productos Insertados Exitosamente !") {
            $.alert({
                theme: 'Modern',
                icon: 'fa fa-check',
                boxWidth: '500px',
                useBootstrap: false,
                type: 'green',
                title: 'Super !',
                content: data,
                buttons: {
                    Continuar: {
                        btnClass: 'btn-success',
                        action: function () {
                            CargaCategorias();
                        }
                    }
                }
            });
        }
        else {
            $.alert({
                theme: 'Modern',
                icon: 'fa fa-question',
                boxWidth: '500px',
                useBootstrap: false,
                type: 'red',
                title: 'Error !',
                content: data,
                buttons: {
                    Continuar: {
                        btnClass: 'btn-success',
                        action: function () {
                            CargaCategorias();
                        }
                    }
                }
            });
        }
        
    }

    //RECIBE DEL METODO CUANDO GUARDA CLIENTE
    connectpsr.client.GuardoCliente = function (data) {
        CargaCategorias();
    }
}


//METODOS DE ACTUALIZACION DE MESA, PRECIOS Y SOLICITUD EN GENERAL
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
        '<th>Editar</th>' +
        '<th>Producto</th>' +
        '<th>Descripcion</th>' +
        //'<th>Cantidad</th>' +
        '<th>Precio</th>' +
        '</tr>' +
        '</thead>' +
        '<tbody id="BodyProductos">' +

        '</tbody>' +
        '</table>');

    $("#BodyProductos").empty();

    for (var i = 0; i < data[0].ProductosSolicitud.length; i++) {
        $("#BodyProductos").append('<tr>' +
            '<td>' +
                
            '</<td>' +
            '<td>' +
            data[0].ProductosSolicitud[i].NombreProducto +
            '</<td>' +
            '<td>' +
            data[0].ProductosSolicitud[i].Descripcion +
            '</<td>' +                
            '<td>' +
            data[0].ProductosSolicitud[i].PrecioProducto +
            '</<td>' +
            '</tr>');
    }
    $('#Tabla2').DataTable({
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json"
        }
    });
    //Content/plugins/datatables/js/Spanish.json
}

//METODOS DE CARGA DE CATEGORIA, PRODUCTO Y ADICIONES. 
function CargaCategorias() {
    $("#setCategoria").empty();
    $("#setProducto").empty();
    $("#tableProductos").css("display", "none");
    $("#tableAdiciones").css("display", "none");
    $(".Categ").css("background-color", "transparent");
    $("#Adiciones").val('');
    $("#contador").val('1');
    $("#ID_PRODUCTO").val('');
    $("#PRECIO_PRODUCTO").val('');

    $.ajax({
        type: "POST",
        url: urlListaCategorias,
        contentType: "application/json; charset=utf-8",
        dataType: "JSON",
        success: function (result) {
            var json = JSON.parse(result);
            if (json.length > 0) {
                for (var index = 0, len = json.length; index < len; index++) {
                    $("#setCategoria").append('<td style="width: 25 %; text - align: left; " >' +
                        '<div class="Categ" id="' + json[index].ID + '_Categ" style = "border: 3px solid; width: 100px; height: 100px; border-radius: 5px; display: flex; align-items: center; text-align: center; cursor: pointer;" onclick="CargaProducto(' + json[index].ID + ')">' +
                        '<div style="width: 100%;">' + json[index].CATEGORIA + '</div>' +
                        '</div >' +
                        '</td >');
                }
            }
        },
        error: function (request, status, error) {
            console.log(error);
        }

    });
}
function CargaProducto(id) {
    $("#setProducto").empty();
    $("#tableProductos").css("display", "block");
    $("#tableAdiciones").css("display", "none");
    $(".Categ").css("background-color", "transparent");
    $("#" + id + "_Categ").css("background-color", "white");
    $("#Adiciones").val('');
    $("#contador").val('1');
    $("#ID_PRODUCTO").val('');
    $("#PRECIO_PRODUCTO").val('');

    $.ajax({
        type: "POST",
        url: urlListaProductos,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ IdProducto: id }),
        dataType: "JSON",
        success: function (result) {
            var json = JSON.parse(result);
            if (json.length > 0) {
                for (var index = 0, len = json.length; index < len; index++) {
                    if (json[index].CANTIDAD >= 1) {
                        $("#setProducto").append('<td style="width: 25 %; text - align: left; " >' +
                            '<div class="Prod" id="' + json[index].ID + '_Producto" precio="' + json[index].PRECIO + '" cantidad="' + json[index].CANTIDAD + '" style = "border: 3px solid; width: 100px; height: 100px; border-radius: 5px; display: flex; align-items: center; text-align: center; cursor: pointer;" onclick="CargaAdiciones(' + json[index].ID + ', ' + json[index].PRECIO + ')">' +
                            '<div style="width: 100%;">' + json[index].NOMBRE_PRODUCTO + '</div>' +
                            '</div >' +
                            '</td >');
                    }
                    else {
                        $("#setProducto").append('<td style="width: 25 %; text - align: left; " >' +
                            '<div id="' + json[index].ID + '_Producto" precio="' + json[index].PRECIO + '" cantidad="' + json[index].CANTIDAD + '" style = "border: 3px solid; width: 100px; height: 100px; border-radius: 5px; display: flex; align-items: center; text-align: center; cursor: pointer; background-color: #aa020273" ">' +
                            '<div style="width: 100%;">' + json[index].NOMBRE_PRODUCTO + '</div>' +
                            '</div >' +
                            '</td >');
                    }
                }
            }
        },
        error: function (request, status, error) {
            console.log(error);
        }

    });
}
function CargaAdiciones(id, precio) {
    $("#tableAdiciones").css("display", "block");
    $(".Prod").css("background-color", "transparent");
    $("#" + id + "_Producto").css("background-color", "white");
    $("#Adiciones").val('');
    $("#contador").val('1');

    //ASIGNA A VARIABLES PARA GUARDAR
    $("#ID_PRODUCTO").val(id);
    $("#PRECIO_PRODUCTO").val(precio);

}

//METODO SOLO GUARDA DATOS DEL CLIENTE
function GuardarDatosCliente() {
    $("#GuardaDatosCliente").attr("disabled", "true");
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
//METODO SOLO IMPRIME Y PAGA FACTURA
function PagarFactura() {
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
                                    //IMPRIME FACTURA

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
                                        content: 'La cuenta fue pagada !',
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
                                        content: 'La cuenta fue pagada !',
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
//METODO IMPRIME FACTURA NADA MAS
function GeneraFactura() {
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
                    //IMPRIME FACTURA

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
//METODO CANCELA PEDIDO
function CancelaPedido() {
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

//METODO DE SUMA EN OTROS COBROS, DESCUENTOS Y TOTAL 
function SumaTotal() {
    if ($("#Descuentos").val() != "" && $("#OtrosCobros").val() != "") {
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
function menos() {
    if ($("#contador").val() > 1) {
        $("#contador").val(Number($("#contador").val()) - 1);
    }
}
function mas() {
    if ($("#contador").val() < 5) {
        $("#contador").val(Number($("#contador").val()) + 1);
    }
}