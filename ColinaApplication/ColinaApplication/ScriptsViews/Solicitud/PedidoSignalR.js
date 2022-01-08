
let connectPSR;
let ProductosSolicitudVector;
let descripcion;
var ProductosPedido = [];

$(function PedidoSignalR() {

    CargaCategorias();
    CargaMeseros();
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
        if (ProductosPedido.length > 0) {
            $("#AgregaProductos").attr("disabled", "disabled");
            //ENVIA AL METODO INSERTAR
            connectpsr.server.insertaProductosSolicitud(ProductosPedido, $('#ID_MESA').val());
        }
        else {
            $.alert({
                theme: 'Modern',
                icon: 'fa fa-times',
                boxWidth: '500px',
                useBootstrap: false,
                type: 'red',
                title: 'Oops',
                content: 'Debe seleccionar al menos un producto antes de enviar',
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
                            $("#setProductosElegidos").empty();
                            ProductosPedido = [];
                            $("#AgregaProductos").removeAttr("disabled");
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

    //RECIBE EL METODO CUANDO NO HAY EXISTENCIAS DE ALGUNOS PRODUCTOS
    connectpsr.client.FaltaExistencias = function (data) {
        $.alert({
            theme: 'Modern',
            icon: 'fa fa-question',
            boxWidth: '500px',
            useBootstrap: false,
            type: 'red',
            title: 'Error !',
            content: "No hay existencias para " + data.toString() + ". Consulta con el administrador.",
            buttons: {
                Continuar: {
                    btnClass: 'btn-danger',
                    action: function () {
                        CargaCategorias();
                    }
                }
            }
        });


    }

    //RECIBE DEL METODO CUANDO GUARDA CLIENTE
    connectpsr.client.GuardoCliente = function (data) {
        CargaCategorias();
    }

    ////RECIBE METODOD PARA REDIRECCIONAR
    //connectpsr.client.ListaMesas = function (listamesas, Redirecciona, idmesa) {
    //    if (Redirecciona == "SI" && idmesa == $("#ID_MESA").val()) {
    //        window.location.href = '../Solicitud/SeleccionarMesa';
    //    };
    //}

    //
    connectpsr.client.CambiaIdMesa = function (idmesa, idmesaAnterior) {
        if ($("#ID_MESA").val() == idmesaAnterior) {
            $("#ID_MESA").val(idmesa);
        }
    }
    //RECIBE EL METODO DE CAMBIO DE MESA
    connectpsr.client.ListaMesas = function (data, Redirecciona, idmesa, ruta) {
        $("#ListaMesas").empty();
        for (var i = 0; i < data.length; i++) {
            switch (data[i].ESTADO) {
                case "LIBRE":
                    $("#ListaMesas").append('<div id=' + data[i].ID + ' onclick="CambioMesa(this.id, \'OCUPADO\');" class="clic panel panel-success estilo" style="width: 100px; text-align: center; float: left; margin: 5px; cursor: pointer; ">' +
                        '<div class="panel-heading">' +
                        '<h2 class="panel-title">' + data[i].NOMBRE_MESA + '</h2>' +
                        '</div>' +
                        '<i class="fa fa-3x fa-check text-success"></i>' +
                        '</div>');
                    break;
                case "OCUPADO":
                    $("#ListaMesas").append('<div id=' + data[i].ID + ' class="panel panel-danger estilo" style="width: 100px; text-align: center; float: left; margin: 5px; cursor: not-allowed; ">' +
                        '<div class="panel-heading">' +
                        '<h2 class="panel-title">' + data[i].NOMBRE_MESA + '</h2>' +
                        '</div>' +
                        '<i class="fa fa-3x fa-arrow-down text-danger"></i>' +
                        '</div>');
                    break;
                case "ESPERA":
                    $("#ListaMesas").append('<div id=' + data[i].ID + ' class="panel panel-warning estilo" style="width: 100px; text-align: center; float: left; margin: 5px; cursor: not-allowed; ">' +
                        '<div class="panel-heading">' +
                        '<h2 class="panel-title">' + data[i].NOMBRE_MESA + '</h2>' +
                        '</div>' +
                        '<i class="fa fa-3x fa-clock-o text-warning"></i>' +
                        '</div>');
                    break;
                case "NO DISPONIBLE":
                    $("#ListaMesas").append('<div id=' + data[i].ID + ' class="panel panel-default" style="width: 100px; text-align: center; float: left; margin: 5px; cursor: not-allowed; ">' +
                        '<div class="panel-heading">' +
                        '<h2 class="panel-title">' + data[i].NOMBRE_MESA + '</h2>' +
                        '</div>' +
                        '<i class="fa fa-3x fa-times-circle text-primary"></i>' +
                        '</div>');
                    break;

                default:
                    break;
            }
        }
        // SE REALIZA EL REDIRECCIONAMIENTO EN ESTE PUNTO YA QUE SI SE DEJA SOBRE ONCLICK DE CADA MESA, NO ACTUALIZA LA MESA
        if (Redirecciona == "SI" && idmesa == $("#ID_MESA").val()) {

            window.location.href = ruta;
        }
    }
}


//METODOS DE ACTUALIZACION DE MESA, PRECIOS Y SOLICITUD EN GENERAL
function ActualizaInfoMesa(data) {
    if (data[0].EstadoSolicitud == "ABIERTA") {
        $("#DivLlevar").css("display", "block");
        $("#DivAsignar").css("display", "none");
        $("#InfoMesa").empty();
        $("#InfoMesa").append('<div class="col-lg-12">' +
            '<div class="small-box bg-danger">' +
            '<div class="inner">' +
            '<h3>' +
            '#' + data[0].NumeroMesa +
            '</h3>' +
            '<p>' + data[0].NombreMesa + '</p>' +
            '<p><b>Mesero:<b/> ' + data[0].NombreMesero + '</p>' +
            '<p><b>C.C Cliente: </b><input id="CCCliente" type="text" autocomplete="off" class="form-control input-sm" name="CCCliente" value="' + data[0].IdentificacionCliente + '" onkeypress = "return soloNum(event)" onpaste="return false"/></p>' +
            '<p><b>Nombre Cliente: </b><input id="NombreCliente" type="text" autocomplete="off" class="form-control input-sm" name="NombreCliente"  value="' + data[0].NombreCliente + '" /></p>' +
            '</div>' +
            '<div class="icon">' +
            '<i class="fa fa-arrow-down"></i>' +
            '</div>' +
            '</div>' +
            '</div>');
    }
    if (data[0].EstadoSolicitud == "LLEVAR") {
        $("#DivLlevar").css("display", "none");
        $("#DivAsignar").css("display", "block");
        $("#InfoMesa").empty();
        $("#InfoMesa").append('<div class="col-lg-12">' +
            '<div class="small-box bg-warning" style="background-color: #e3a200 !important; ">' +
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
            '<i class="fa fa-clock-o"></i>' +
            '</div>' +
            '</div>' +
            '</div>');
    }
    $("#ID_MESERO").val(data[0].IdMesero)
    $("#ESTADO_SOLICITUD").val(data[0].EstadoSolicitud)
    $("#OBSERVACIONES").val(data[0].Observaciones);
}
function ActualizaInfoPrecios(data) {
    console.log(data);
    ProductosSolicitudVector = data[0].ProductosSolicitud;
    var IVA = '';
    var ICONSUMO = '';
    var SERVICIO = '';
    if (data[0].Impuestos[0].Estado == "ACTIVO")
        IVA = '<tr> ' +
            '<td>' +
            '<small><b>I.V.A (' + data[0].PorcentajeIVA + '%) : <b></small>' +
            '<input id="Iva" type="text" class="form-control input-sm" name="Total" value="' + data[0].IVATotal + '" ReadOnly="true"/>' +
            '</td>' +
            '</tr>';
    if (data[0].Impuestos[1].Estado == "ACTIVO")
        ICONSUMO = '<tr> ' +
            '<td>' +
            '<small><b>Impuesto Consumo (' + data[0].PorcentajeIConsumo + '%) : <b></small>' +
            '<input id="SubTotal" type="text" class="form-control input-sm" name="Total" value="' + data[0].IConsumoTotal + '" ReadOnly="true"/>' +
            '</td>' +
            '</tr>';
    if (data[0].Impuestos[2].Estado == "ACTIVO")
        SERVICIO = '<tr>' +
            '<td>' +
            '<small><b>Servicio (' + data[0].Impuestos[2].Porcentaje + '% Máx.) :</b></small><small style="color: white; font-weight: bold; font-size: 20px; padding: 5px; border-radius: 5px; background-color: #30a630;">    $' + data[0].ServicioTotal + '</small><br>' +
            '<span class="input-group-btn" style="float: left;">' +
            '<button class="btn btn-success" id="menosServicio" type="button" onclick="menosServicio()"><b>-</b></button>' +
            '</span>' +
            '<input type="text" style="width:50px;text-align: center; float: left; margin-left: 10%;" id="servicio" class="form-control" value="' + data[0].PorcentajeServicio + '" readonly />' +
            '<span class="input-group-btn" style="float: left; margin-left: 2%;">' +
            '<button class="btn btn-success" id="masServicio" type="button" onclick="masServicio(' + data[0].Impuestos[2].Porcentaje + ')"><b>+</b></button>' +
            '</span>' +

            '</td>' +
            '</tr>';

    $("#InfoPrecios").empty();
    $("#InfoPrecios").append('<table class="table table-hover" style="font-size: 18px;">' +
        '<tbody>' +
        '<tr>' +
        '<td>' +
        '<small><b>Otros Cobros: </b></small>' +
        '<input id="OtrosCobros" type="text" class="form-control input-sm" name="OtrosCobros" value="' + data[0].OtrosCobros + '" onkeypress = "return soloNum(event)" onpaste="return false"/>' +
        '</td>' +
        '</tr>' +
        '<tr>' +
        '<td>' +
        '<small><b>Descuentos: </b></small>' +
        '<input id="Descuentos" type="text" class="form-control input-sm" name="Descuentos" value="' + data[0].Descuentos + '" onkeypress = "return soloNum(event)" onpaste="return false"/>' +
        '</td>' +
        '</tr>' +
        '<tr>' +
        '<td>' +
        '<small><b>SubTotal: <b></small>' +
        '<input id="SubTotal" type="text" class="form-control input-sm" name="Total" value="' + data[0].Subtotal + '" ReadOnly="true"/>' +
        '</td>' +
        '</tr>' +
        IVA +
        ICONSUMO +
        SERVICIO +
        '<tr>' +
        '<td>' +
        '<small><b>Total: <b></small>' +
        '<input id="Total" type="text" class="form-control input-sm" name="Total" value="' + data[0].Total + '" ReadOnly="true"/>' +
        '</td>' +
        '</tr>' +
        '</tbody>' +
        '</table>');
    //TotalGeneral = data[0].Total;
    //SumaTotal();

}
function ActualizaInfoProductos(data) {
    $("#InfoProductos").empty();
    $("#InfoProductos").append('<table id="Tabla2" class="table table-bordered table-hover">' +
        '<thead>' +
        '<tr>' +
        '<th>Editar</th>' +
        '<th>Producto</th>' +
        '<th>Descripcion</th>' +
        '<th>Precio</th>' +
        '</tr>' +
        '</thead>' +
        '<tbody id="BodyProductos">' +

        '</tbody>' +
        '</table>');

    $("#BodyProductos").empty();

    for (var i = 0; i < data[0].ProductosSolicitud.length; i++) {
        var code = '';
        var color = '#a90000';
        if (data[0].ProductosSolicitud[i].EstadoProducto == "ENTREGADO")
            color = '#5cb85c';
        descripcion = data[0].ProductosSolicitud[i].Descripcion.toString();
        if (IdPerfil == 1) {
            code = '<i class="fa fa-2x fa-minus-square" style="color: #a90000; cursor:pointer;" onclick="CancelaProductoxId(' + data[0].ProductosSolicitud[i].Id + ',' + data[0].ProductosSolicitud[i].Id + ')"></i>' +
                '<i id="' + descripcion + '" class="fa fa-2x fa-print" style="color: ' + color + '; cursor:pointer; margin-left: 5px;" onclick="ReEnviaProducto(' + data[0].ProductosSolicitud[i].IdProducto + ', this.id, ' + data[0].IdMesa + ')"></i >';
        }
        else {
            code = '<i id="' + descripcion + '" class="fa fa-2x fa-print" style="color: ' + color + '; cursor:pointer; margin-left: 5px;" onclick="ReEnviaProducto(' + data[0].ProductosSolicitud[i].IdProducto + ', this.id, ' + data[0].IdMesa + ')"></i >';
        }
        $("#BodyProductos").append('<tr>' +
            '<td>' +
            code +
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
}
//FUNCION DE CANCELAR PRODUCTO POR ID
function CancelaProductoxId(idProducto) {
    $.alert({
        theme: 'Modern',
        icon: 'fa fa-times',
        boxWidth: '500px',
        useBootstrap: false,
        type: 'red',
        title: 'Eliminar !',
        content: 'Esta seguro que desea eliminar este producto ?',
        buttons: {
            Si: {
                btnClass: 'btn btn-danger',
                action: function () {
                    $.alert({
                        theme: 'Modern',
                        icon: 'fa fa-warning',
                        boxWidth: '500px',
                        useBootstrap: false,
                        type: 'red',
                        title: 'Cancelar !',
                        content: 'Desea retornar el elemento al inventario ?',
                        buttons: {
                            Si: {
                                btnClass: 'btn btn-danger',
                                action: function () {
                                    connectPSR.server.cancelaPedidoXId(idProducto, true);
                                    $.alert({
                                        theme: 'Modern',
                                        icon: 'fa fa-check',
                                        boxWidth: '500px',
                                        useBootstrap: false,
                                        type: 'green',
                                        title: 'Super !',
                                        content: 'El producto fue eliminado !',
                                        buttons: {
                                            Continuar: {
                                                btnClass: 'btn btn-success',
                                                action: function () {
                                                    connectPSR.server.consultaMesaAbierta($('#ID_MESA').val());
                                                }
                                            },
                                        }
                                    });
                                }
                            },
                            No: {
                                btnClass: 'btn btn-danger',
                                action: function () {
                                    connectPSR.server.cancelaPedidoXId(idProducto, false);
                                    $.alert({
                                        theme: 'Modern',
                                        icon: 'fa fa-check',
                                        boxWidth: '500px',
                                        useBootstrap: false,
                                        type: 'green',
                                        title: 'Super !',
                                        content: 'El producto fue eliminado !',
                                        buttons: {
                                            Continuar: {
                                                btnClass: 'btn btn-success',
                                                action: function () {
                                                    connectPSR.server.consultaMesaAbierta($('#ID_MESA').val());
                                                }
                                            },
                                        }
                                    });
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

//REENVIA PRODUCTOS A IMPRESORAS
function ReEnviaProducto(idproducto, description, idmesa) {
    connectPSR.server.imprimeProductos(1, idproducto, description, idmesa);
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
            var br = '<br/><br/><br/><br/>';
            if (json.length > 0) {
                for (var index = 0, len = json.length; index < len; index++) {
                    $("#setCategoria").append('<div class="Categ" id="' + json[index].ID + '_Categ" style = "margin-left: 2%; margin-top: 2%; float:left; border: 2px solid; width: 100px; height: 100px; border-radius: 5px; display: flex; align-items: center; text-align: center; cursor: pointer; background-color: #ffc93163" onclick="CargaProducto(' + json[index].ID + ')">' +
                        '<div style="width: 100%; font-family: Copperplate Gothic Bold; font-size: 16px;"><b>' + json[index].CATEGORIA + '</b></div>' +
                        '</div >');
                    if (index == 6 || index == 13 || index == 21) {
                        $("#setCategoria").append(br);
                    }
                }
            }
        },
        error: function (request, status, error) {
            console.log(error);
        }

    });
}
function CargaMeseros() {
    $("#selectMeseros").append("<option value=''>--SELECCIONE--</option>");
    $.ajax({
        type: "POST",
        url: urlListaMeseros,
        contentType: "application/json; charset=utf-8",
        dataType: "JSON",
        success: function (result) {
            var json = JSON.parse(result);
            if (json.length > 0) {
                for (var index = 0, len = json.length; index < len; index++) {
                    $('#selectMeseros').append($('<option>', {
                        value: json[index].ID,
                        text: json[index].NOMBRE
                    }));
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
    $(".Categ").css("background-color", "#ffc93163");
    $("#" + id + "_Categ").css("background-color", "#d4d4d4");
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
            var br = '<br/><br/><br/><br/>';
            if (json.length > 0) {
                for (var index = 0, len = json.length; index < len; index++) {
                    if (json[index].CANTIDAD >= 1) {
                        $("#setProducto").append('<div class="Prod" id="' + json[index].ID + '_Producto" precio="' + json[index].PRECIO + '" cantidad="' + json[index].CANTIDAD + '" style = "margin-left: 2%; margin-top: 2%; float:left; border: 2px solid; width: 100px; height: 100px; border-radius: 5px; display: flex; align-items: center; text-align: center; cursor: pointer; background-color: #f4a1247d;" onclick="CargaAdiciones(' + json[index].ID + ', ' + json[index].PRECIO + ', \'' + json[index].NOMBRE_PRODUCTO + '\')">' +
                            '<div style="width: 100%; font-family: Copperplate Gothic Bold; font-size: 16px;"><b>' + json[index].NOMBRE_PRODUCTO + '</b></div>' +
                            '</div >');
                    }
                    else {
                        $("#setProducto").append('<div id="' + json[index].ID + '_Producto" precio="' + json[index].PRECIO + '" cantidad="' + json[index].CANTIDAD + '" style = "margin-left: 2%; margin-top: 2%; float:left; border: 2px solid; width: 100px; height: 100px; border-radius: 5px; display: flex; align-items: center; text-align: center; cursor: not-allowed; background-color: #aa020273;">' +
                            '<div style="width: 100%; font-family: Copperplate Gothic Bold; font-size: 16px;"><b>' + json[index].NOMBRE_PRODUCTO + '</b></div>' +
                            '</div >');
                    }
                    if (index == 6 || index == 13 || index == 21) {
                        $("#setProducto").append(br);
                    }
                }
            }
        },
        error: function (request, status, error) {
            console.log(error);
        }

    });
}
function CargaAdiciones(id, precio, nomproducto) {
    $("#tableAdiciones").css("display", "block");
    $(".Prod").css("background-color", "#f4a1247d");
    $("#" + id + "_Producto").css("background-color", "#d4d4d4");
    $("#Adiciones").val('');
    $("#contador").val('1');

    //ASIGNA A VARIABLES PARA GUARDAR
    $("#ID_PRODUCTO").val(id);
    $("#PRECIO_PRODUCTO").val(precio);
    $("#NOMBREPRODUCTO").val(nomproducto);


}

//METODO SOLO GUARDA DATOS DEL CLIENTE
function GuardarDatosCliente() {
    $("#GuardaDatosCliente").attr("disabled", "true");
    connectPSR.server.guardaDatosCliente($("#ID").val(), $("#CCCliente").val(), $("#NombreCliente").val(), $("#OBSERVACIONES").val(), $("#OtrosCobros").val(), $("#Descuentos").val(),
        $("#SubTotal").val(), $("#ESTADO_SOLICITUD").val(), $("#ID_MESA").val(), $("#servicio").val(), "", "", "0", $("#ID_MESERO").val());
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
//METODO IMPRIME Y PAGA FACTURA
function PagarFactura() {
    $.alert({
        theme: 'Modern',
        icon: 'fa fa-question',
        boxWidth: '500px',
        useBootstrap: false,
        type: 'orange',
        title: 'Vale !',
        content: 'Desea pagar la cuenta ? ',
        buttons: {
            Si: {
                btnClass: 'btn btn-warning',
                action: function () {
                    $.alert({
                        theme: 'Modern',
                        icon: 'fa fa-money',
                        boxWidth: '500px',
                        useBootstrap: false,
                        type: 'orange',
                        title: 'Medio de pago !',
                        content: 'Seleccione el medio de pago <br/> <label><input type="checkbox" name="Check1" Value="SI"> Desea Imprimir factura ? </label><br> ',
                        buttons: {
                            Tarjeta: {
                                btnClass: 'btn btn-warning',
                                action: function () {
                                    var Imprime = $('input:checkbox[name=Check1]:checked').val();
                                    $.alert({
                                        theme: 'Modern',
                                        icon: 'fa fa-credit-card',
                                        boxWidth: '500px',
                                        useBootstrap: false,
                                        type: 'gray',
                                        title: '# Numero Aprobación !',
                                        content: 'Digite el numero de Aprobacion del voucher <br/> <div><input style="witdh:60%; margin-left: 20%;" id="numAprobacionVoucher" type="text" class="form-control input-sm" onkeypress = "return soloNum(event)" required /></div>',
                                        buttons: {
                                            Continuar: {
                                                btnClass: 'btn btn-default',
                                                action: function () {
                                                    if ($("#numAprobacionVoucher").val() != "") {
                                                        if (Imprime == "SI")
                                                            connectPSR.server.imprimirFactura($("#ID_MESA").val());
                                                        connectPSR.server.guardaDatosCliente($("#ID").val(), $("#CCCliente").val(), $("#NombreCliente").val(), $("#OBSERVACIONES").val(), $("#OtrosCobros").val(), $("#Descuentos").val(),
                                                            $("#SubTotal").val(), "FINALIZADA", $("#ID_MESA").val(), $("#servicio").val(), "TARJETA", $("#numAprobacionVoucher").val(), "0", $("#ID_MESERO").val());
                                                        connectPSR.server.actualizaMesa($("#ID_MESA").val(), "LIBRE", User, "SI", "../Solicitud/SeleccionarMesa");
                                                    }
                                                    else {
                                                        PagarFactura();
                                                    }
                                                }
                                            }
                                        }
                                    });
                                }
                            },
                            Efectivo: {
                                btnClass: 'btn btn-warning',
                                action: function () {
                                    if ($('input:checkbox[name=Check1]:checked').val() == "SI")
                                        connectPSR.server.imprimirFactura($("#ID_MESA").val());
                                    connectPSR.server.guardaDatosCliente($("#ID").val(), $("#CCCliente").val(), $("#NombreCliente").val(), $("#OBSERVACIONES").val(), $("#OtrosCobros").val(), $("#Descuentos").val(),
                                        $("#SubTotal").val(), "FINALIZADA", $("#ID_MESA").val(), $("#servicio").val(), "EFECTIVO", "0", $("#SubTotal").val(), $("#ID_MESERO").val());
                                    connectPSR.server.actualizaMesa($("#ID_MESA").val(), "LIBRE", User, "SI", "../Solicitud/SeleccionarMesa");

                                }
                            },
                            Ambas: {
                                btnClass: 'btn btn-warning',
                                action: function () {
                                    var Imprime = $('input:checkbox[name=Check1]:checked').val();
                                    $.alert({
                                        theme: 'Modern',
                                        icon: 'fa fa-money',
                                        boxWidth: '500px',
                                        useBootstrap: false,
                                        type: 'gray',
                                        title: '$ Efectivo !',
                                        content: 'Digite la cantidad en efectivo <br/> <div><input style="witdh:60%; margin-left: 20%;" id="cantEfectivo" type="text" class="form-control input-sm" onkeypress = "return soloNum(event)" required /></div>',
                                        buttons: {
                                            Continuar: {
                                                btnClass: 'btn btn-default',
                                                action: function () {
                                                    var cantEfectivo = $("#cantEfectivo").val();
                                                    if (cantEfectivo != "") {
                                                        $.alert({
                                                            theme: 'Modern',
                                                            icon: 'fa fa-credit-card',
                                                            boxWidth: '500px',
                                                            useBootstrap: false,
                                                            type: 'gray',
                                                            title: ' Tarjeta !',
                                                            content: '# de Aprobacion del voucher <br/> <div><input style="witdh:60%; margin-left: 20%;" id="numAprobacionVoucher2" type="text" class="form-control input-sm" onkeypress = "return soloNum(event)" required /></div>',
                                                            buttons: {
                                                                Continuar: {
                                                                    btnClass: 'btn btn-default',
                                                                    action: function () {
                                                                        if ($("#numAprobacionVoucher2").val() != "") {
                                                                            if (Imprime == "SI")
                                                                                connectPSR.server.imprimirFactura($("#ID_MESA").val());
                                                                            connectPSR.server.guardaDatosCliente($("#ID").val(), $("#CCCliente").val(), $("#NombreCliente").val(), $("#OBSERVACIONES").val(), $("#OtrosCobros").val(), $("#Descuentos").val(),
                                                                                $("#SubTotal").val(), "FINALIZADA", $("#ID_MESA").val(), $("#servicio").val(), "AMBAS", $("#numAprobacionVoucher2").val(), cantEfectivo, $("#ID_MESERO").val());
                                                                            connectPSR.server.actualizaMesa($("#ID_MESA").val(), "LIBRE", User, "SI", "../Solicitud/SeleccionarMesa");
                                                                        }
                                                                        else {
                                                                            PagarFactura();
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        });
                                                    }
                                                    else {
                                                        PagarFactura();
                                                    }
                                                }
                                            }
                                        }
                                    });
                                }
                            }
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

//METODO IMPRIME FACTURA NADA MAS
function GeneraFactura() {
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
                    //IMPRIME FACTURA
                    connectPSR.server.imprimirFactura($("#ID_MESA").val());
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
                    $.alert({
                        theme: 'Modern',
                        icon: 'fa fa-times',
                        boxWidth: '500px',
                        useBootstrap: false,
                        type: 'red',
                        title: 'Cancelar !',
                        content: 'Desea retornar todos los productos al inventario ?',
                        buttons: {
                            Si: {
                                btnClass: 'btn btn-danger',
                                action: function () {
                                    connectPSR.server.guardaDatosCliente($("#ID").val(), $("#CCCliente").val(), $("#NombreCliente").val(), $("#OBSERVACIONES").val(), $("#OtrosCobros").val(), $("#Descuentos").val(),
                                        $("#SubTotal").val(), "CANCELA PEDIDO", $("#ID_MESA").val(), $("#servicio").val(), "N/A", "0", "0", $("#ID_MESERO").val());
                                    connectPSR.server.cancelaPedido($("#ID").val(), true);
                                    connectPSR.server.actualizaMesa($("#ID_MESA").val(), "LIBRE", User, "SI", "../Solicitud/SeleccionarMesa");

                                }
                            },
                            No: {
                                btnClass: 'btn btn-danger',
                                action: function () {
                                    connectPSR.server.guardaDatosCliente($("#ID").val(), $("#CCCliente").val(), $("#NombreCliente").val(), $("#OBSERVACIONES").val(), $("#OtrosCobros").val(), $("#Descuentos").val(),
                                        $("#SubTotal").val(), "CANCELA PEDIDO", $("#ID_MESA").val(), $("#servicio").val(), "N/A", "0", "0", $("#ID_MESERO").val());
                                    connectPSR.server.cancelaPedido($("#ID").val(), false);
                                    connectPSR.server.actualizaMesa($("#ID_MESA").val(), "LIBRE", User, "SI", "../Solicitud/SeleccionarMesa");

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
//METODO DE LLEVAR ORDEN
function AsignarLlevar() {
    $.alert({
        theme: 'Modern',
        icon: 'fa fa-arrow-up',
        boxWidth: '500px',
        useBootstrap: false,
        type: 'orange',
        title: 'Llevar Orden !',
        content: 'Desea convertir la orden para llevar ?',
        buttons: {
            Si: {
                btnClass: 'btn btn-warning',
                action: function () {
                    connectPSR.server.guardaDatosCliente($("#ID").val(), $("#CCCliente").val(), $("#NombreCliente").val(), $("#OBSERVACIONES").val(), $("#OtrosCobros").val(), $("#Descuentos").val(),
                        $("#SubTotal").val(), "LLEVAR", $("#ID_MESA").val(), $("#servicio").val(), "", "", "0", $("#ID_MESERO").val());
                    connectPSR.server.actualizaMesa($("#ID_MESA").val(), "ESPERA", User, "NO", "");
                }
            },
            Cancelar: {
                btnClass: 'btn btn-warning',
                action: function () {

                }
            },
        }
    });
}
//METODO DE ASIGNAR ORDEN
function AsignarAsignaMesa() {
    $.alert({
        theme: 'Modern',
        icon: 'fa fa-arrow-down',
        boxWidth: '500px',
        useBootstrap: false,
        type: 'green',
        title: 'Asignar Orden !',
        content: 'Desea asignar la orden a la mesa ?',
        buttons: {
            Si: {
                btnClass: 'btn btn-success',
                action: function () {
                    connectPSR.server.guardaDatosCliente($("#ID").val(), $("#CCCliente").val(), $("#NombreCliente").val(), $("#OBSERVACIONES").val(), $("#OtrosCobros").val(), $("#Descuentos").val(),
                        $("#SubTotal").val(), "ABIERTA", $("#ID_MESA").val(), $("#servicio").val(), "", "", "0", $("#ID_MESERO").val());
                    connectPSR.server.actualizaMesa($("#ID_MESA").val(), "OCUPADO", User, "NO", "");
                }
            },
            Cancelar: {
                btnClass: 'btn btn-success',
                action: function () {

                }
            },
        }
    });
}
//METODO BOTON CONSUMO INTERNO
function ConsumoInterno() {
    if ($("#CCCliente").val() != "") {
        $.alert({
            theme: 'Modern',
            icon: 'fa fa-users',
            boxWidth: '500px',
            useBootstrap: false,
            type: 'blue',
            title: 'Consumo Interno !',
            content: 'Desea pasar esta cuenta a consumo interno ?',
            buttons: {
                Si: {
                    btnClass: 'btn btn-primary',
                    action: function () {
                        connectPSR.server.guardaDatosCliente($("#ID").val(), $("#CCCliente").val(), $("#NombreCliente").val(), $("#OBSERVACIONES").val(), $("#OtrosCobros").val(), $("#Descuentos").val(),
                            $("#SubTotal").val(), "CONSUMO INTERNO", $("#ID_MESA").val(), $("#servicio").val(), "N/A", "0", "0", $("#ID_MESERO").val());
                        connectPSR.server.actualizaMesa($("#ID_MESA").val(), "LIBRE", User, "NO", "");
                        $.alert({
                            theme: 'Modern',
                            icon: 'fa fa-users',
                            boxWidth: '500px',
                            useBootstrap: false,
                            type: 'blue',
                            title: 'Inhabilitar Mesa !',
                            content: 'Para ser redireccionado de clic en Continuar !',
                            buttons: {
                                Continuar: {
                                    btnClass: 'btn btn-primary',
                                    action: function () {
                                        connectPSR.server.listarEstadoMesas("SI", $("#ID_MESA").val(), "../Solicitud/SeleccionarMesa");
                                    }
                                }
                            }
                        });
                    }
                },
                Cancelar: {
                    btnClass: 'btn btn-primary',
                    action: function () {

                    }
                },
            }
        });
    }
    else {
        $.alert({
            theme: 'Modern',
            icon: 'fa fa-warning',
            boxWidth: '500px',
            useBootstrap: false,
            type: 'red',
            title: 'Consumo Interno !',
            content: 'Debe digitar un numero de cedula del cliente para pasarlo como consumo interno !',
            buttons: {
                Continuar: {
                    btnClass: 'btn btn-danger',
                    action: function () {

                    }
                }
            }
        });
    }

}
//METODO PARA INHABILITAR MESA
function InhabilitarMesa() {
    if (ProductosSolicitudVector.length == 0 && $("#Total").val() == "0") {
        $.alert({
            theme: 'Modern',
            icon: 'fa fa-times',
            boxWidth: '500px',
            useBootstrap: false,
            type: 'default',
            title: 'Inhabilitar Mesa !',
            content: 'Desea Inhabilitar esta mesa ?',
            buttons: {
                Si: {
                    btnClass: 'btn btn-default',
                    action: function () {
                        connectPSR.server.guardaDatosCliente($("#ID").val(), $("#CCCliente").val(), $("#NombreCliente").val(), $("#OBSERVACIONES").val(), $("#OtrosCobros").val(), $("#Descuentos").val(),
                            $("#SubTotal").val(), "INHABILITAR", $("#ID_MESA").val(), $("#servicio").val(), "", "", "0", $("#ID_MESERO").val());
                        connectPSR.server.actualizaMesa($("#ID_MESA").val(), "NO DISPONIBLE", User, "NO", "");
                        connectPSR.server.listarEstadoMesas("SI", $("#ID_MESA").val(), "../Solicitud/SeleccionarMesa");
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
    else {
        $.alert({
            theme: 'Modern',
            icon: 'fa fa-times',
            boxWidth: '500px',
            useBootstrap: false,
            type: 'red',
            title: 'Inhabilitar Mesa Prohibido !',
            content: 'Ud. ya no puede inhabilitar esta mesa. Revise con el Cajero/Administrador',
            buttons: {
                Continuar: {
                    btnClass: 'btn btn-danger',
                    action: function () {
                    }
                }
            }
        });
    }

}
//METODOS PARA HACER CAMBIO DE MESA
function CargaMesas() {
    connectPSR.server.listarEstadoMesas("NO", 0, "");
}
function CierraModalCM() {
    $("#ListaMesas").empty();
}
function CambioMesa(id, Estado) {
    connectPSR.server.guardaDatosCliente($("#ID").val(), $("#CCCliente").val(), $("#NombreCliente").val(), $("#OBSERVACIONES").val(), $("#OtrosCobros").val(), $("#Descuentos").val(),
        $("#SubTotal").val(), $("#ESTADO_SOLICITUD").val(), id, $("#servicio").val(), "", "", "0", $("#ID_MESERO").val());
    connectPSR.server.actualizaMesa(id, Estado, User, "NO", "");
    connectPSR.server.actualizaMesa($("#ID_MESA").val(), "LIBRE", User, "NO", "");
    connectPSR.server.actualizaIdmesaHTML(id, $("#ID_MESA").val());
    Encriptar(id).then(r => {
        connectPSR.server.listarEstadoMesas("SI", id, "../Solicitud/Pedido?Id=" + encodeURIComponent(r));
    }).catch(() => {
        console.log('error');
    });

}


//FUNCIONES ADICIONALES
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
function menosServicio() {
    if ($("#servicio").val() > 0) {
        $("#servicio").val(Number($("#servicio").val()) - 1);
    }
}
function masServicio(porcentajeMaximo) {
    if ($("#servicio").val() < porcentajeMaximo) {
        $("#servicio").val(Number($("#servicio").val()) + 1);
    }
}
function Encriptar(texto) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: urlEncriptar,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ Texto: texto }),
            dataType: "JSON",
            success: function (result) {
                var json = JSON.parse(result);
                if (json != "") {
                    resolve(json);
                }
                else {
                    reject();
                }

            },
            error: function (request, status, error) {
                console.log(error);
            }

        });
    })
}
function CambiaMesero(idMesero) {
    connectPSR.server.guardaDatosCliente($("#ID").val(), $("#CCCliente").val(), $("#NombreCliente").val(), $("#OBSERVACIONES").val(), $("#OtrosCobros").val(), $("#Descuentos").val(),
        $("#SubTotal").val(), $("#ESTADO_SOLICITUD").val(), $("#ID_MESA").val(), $("#servicio").val(), "", "", "0", idMesero);
}
function AgregaProductosPedido() {
    if ($('#ID_PRODUCTO').val() != "" && $('#PRECIO_PRODUCTO').val() != "") {
        $("#AgregaPedido").attr("disabled", "disabled");
        let descripcion;
        if ($('#Adiciones').val() != "")
            descripcion = $('#Adiciones').val().toUpperCase();
        else
            descripcion = "";
        var model = {
            ID: parseInt($("#contador").val()),
            ID_SOLICITUD: $('#ID').val(),
            ID_PRODUCTO: $('#ID_PRODUCTO').val(),
            ID_MESERO: User,
            PRECIO_PRODUCTO: $('#PRECIO_PRODUCTO').val(),
            DESCRIPCION: descripcion,
            ESTADO_PRODUCTO: $('#NOMBREPRODUCTO').val()
        };
        ProductosPedido.push(model);
        $("#setProductosElegidos").empty();
        for (var i = 0; i < ProductosPedido.length; i++) {
            $("#setProductosElegidos").append("<tr><td>" + ProductosPedido[i].ID + "</td><td>" + ProductosPedido[i].ESTADO_PRODUCTO + "</td><td>" + ProductosPedido[i].DESCRIPCION +
                "</td ><td><i class=\"fa fa-2x fa-minus-square\" style=\"color: #a90000; cursor: pointer; \" onclick=\"EliminaProductoLista('" + i + "')\"></i></td></tr> ");
        }
        //console.log(ProductosPedido);
        CargaCategorias();
        $("#AgregaPedido").removeAttr("disabled");
    }
    else {
        $.alert({
            theme: 'Modern',
            icon: 'fa fa-times',
            boxWidth: '500px',
            useBootstrap: false,
            type: 'red',
            title: 'Oops',
            content: 'Debe seleccionar un producto antes de agergar a pedido',
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
function EliminaProductoLista(idElemento) {
    ProductosPedido.splice(idElemento, 1);
    $("#setProductosElegidos").empty();
    for (var i = 0; i < ProductosPedido.length; i++) {
        $("#setProductosElegidos").append("<tr><td>" + ProductosPedido[i].ID + "</td><td>" + ProductosPedido[i].ESTADO_PRODUCTO + "</td><td>" + ProductosPedido[i].DESCRIPCION +
            "</td ><td><i class=\"fa fa-2x fa-minus-square\" style=\"color: #a90000; cursor: pointer; \" onclick=\"EliminaProductoLista('" + i + "')\"></i></td></tr> ");
    }
    //console.log(ProductosPedido);
}
