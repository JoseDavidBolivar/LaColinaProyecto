$(document).ready(function () {

});
function ValidaSesion()
{
    var cedula = $("#Cedula").val();
    var contraseña = $("#Contrasena").val();

    if(cedula != "" && contraseña != "")
    {
        //$("#Cargando").css("display","block");
        $.ajax({
            type: "POST",
            url: urlInicioSesion,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ Cedula: cedula, Contraseña: contraseña }),
            dataType: "JSON",
            success: function (result) {
                var json = JSON.parse(result);
                if (json != null)
                {
                    if(json.ID > 0)
                    {
                        window.location.href = '../Inicio/LaColinaRestaurante';
                    }
                    else
                    {
                        if (json.ID == -1) {
                            alert("La contraseña digitada es INCORRECTA ......");
                        }
                        else
                        {
                            alert("El usuario digitado NO existe dentro de la plataforma ......");
                        }
                        
                    }
                }
                else
                {
                    alert("El usuario digitado no existe....");
                }
            },
            error: function (request, status, error) {
                console.log(error);
            }

        });
        //$("#Cargando").css("display", "none");
    }
    else 
    {
        alert("Digite una Cédula y/o Contraseña");
    }

}
function cargando() {
    $("#Cargando").css("display", "block");

}

function cerrar() {
    $("#Cargando").css("display", "none");
}
$(document).ajaxStart(function () {
    cargando();
});
$(document).ajaxStop(function () {
    cerrar();
});