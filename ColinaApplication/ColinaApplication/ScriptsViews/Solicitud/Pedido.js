$(document).ready(function () {
    CargaProducto();
    
});

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
function CargaSubProducto()
{
    var idProducto = $("#Categoria").val();
    $("#Subrpoductos").empty();
    $("#Subrpoductos").append("<option value=''>--SELECCIONE--</option>");
    if (idProducto != null || idProducto != "")
    {
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
                            text: json[index].NOMBRE_SUBPRODUCTO
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

