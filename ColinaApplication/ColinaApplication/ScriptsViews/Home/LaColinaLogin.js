﻿var enter = false;
$(document).keyup(function (event) {
    if (event.which === 13 && document.getElementsByClassName("btn") !== null && enter) {
        //alert('Enter is pressed!');
        document.querySelector('.btn2').click();
        
    }
});

function ValidaSesion() {
    var codigo = $("#Codigo").val();

    if (codigo != "") {
        $.ajax({
            type: "POST",
            url: urlInicioSesion,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ Codigo: codigo }),
            dataType: "JSON",
            success: function (result) {
                var json = JSON.parse(result);
                if (json.ID > 0) {
                    window.location.href = '../Solicitud/SeleccionarMesa';
                }
                else {
                    if (json.ID == -1) {
                        $.alert({
                            theme: 'Modern',
                            icon: 'fa fa-times',
                            boxWidth: '500px',
                            useBootstrap: false,
                            type: 'red',
                            title: 'Error',
                            content: 'Código o Usuario INCORRECTO ......',
                            buttons: {
                                Continuar: {
                                    btnClass: 'btn btn-danger btn2',
                                    action: function () {
                                        location.reload();
                                    }
                                }
                            }
                        });
                        setInterval(function () { enter = true; }, 1000);                            
                    }
                }
            },
            error: function (request, status, error) {
                console.log(error);
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
            title: 'Cancelar !',
            content: 'Digite un Código',
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


var cells = document.querySelectorAll('#teclado td');

Array.prototype.forEach.call(cells, function (td) {
    td.addEventListener('click', logText);
});

function logText() {
    if (this.textContent != "x") {
        var texto = $("#Codigo").val();
        $("#Codigo").val(texto + this.textContent);
    }
    else {
        var texto = document.getElementById('Codigo');
        texto.value = texto.value.substring(0, texto.value.length - 1);
    }

}

function valideKey(evt) {

    var code = (evt.which) ? evt.which : evt.keyCode;
    
    if (code == 13) { // backspace.
        ValidaSesion();
    } else if ((code >= 48 && code <= 57)) { // is a number or ENTER.
        return true;
    } else { // other keys.
        return false;
    }
}