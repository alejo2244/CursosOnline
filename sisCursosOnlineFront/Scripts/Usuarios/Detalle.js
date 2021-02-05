var id = getUrlId();

$(document).ready(function () {
    if (id !== "Detalle") {
        $.get({
            url: "http://localhost:44381/api/usuario/" + id
        }).then(function (data) {
            $("#formularioUsuario")[0].Nombres.value = data.Nombres;
            $("#formularioUsuario")[0].PrimerApellido.value = data.PrimerApellido;
            $("#formularioUsuario")[0].SegundoApellido.value = data.SegundoApellido;
            $("#formularioUsuario")[0].FechaNacimiento.value = data.FechaNacimiento.substring(0, 10);
            $("#formularioUsuario")[0].LugarNacimiento.value = data.LugarNacimiento;
            $("#formularioUsuario")[0].Direccion.value = data.Direccion;
            $("#formularioUsuario")[0].Genero.value = data.Genero;
            $("#formularioUsuario")[0].Pasatiempos.value = data.Pasatiempos;
            $("#formularioUsuario")[0].Pasatiempos.disabled = true;
        });
    }
});

function getUrlId() {
    var actual = window.location + '';
    var split = actual.split("/");
    return split[split.length - 1];
}

function enviarConsumo() {

    $("#formularioUsuario").on('submit', function (evt) {
        evt.preventDefault();
        $("#btnSave")[0].disabled = true
        $("#btnSave")[0].value = 'Enviando...';
        var usuario = {
            "Id": id,
            "Nombres": $("#formularioUsuario")[0].Nombres.value,
            "PrimerApellido": $("#formularioUsuario")[0].PrimerApellido.value,
            "SegundoApellido": $("#formularioUsuario")[0].SegundoApellido.value,
            "FechaNacimiento": $("#formularioUsuario")[0].FechaNacimiento.value,
            "LugarNacimiento": $("#formularioUsuario")[0].LugarNacimiento.value,
            "Direccion": $("#formularioUsuario")[0].Direccion.value,
            "Genero": $("#formularioUsuario")[0].Genero.value,
            "Pasatiempos": $("#formularioUsuario")[0].Pasatiempos.value
        }
        var method = "";
        var res = ""
        if (id === "Detalle") {
            method = "PUT";
            res = "Insertado";
        }
        else {
            method = "POST";
            res = "Actualizado";
        }

        fetch("http://localhost:44381/api/usuario/", {
            method: method,
            body: JSON.stringify(usuario),
            headers: {
                "Content-type": "application/json"
            }
        }).then(function (data) {
            if (data) {
                alert("Registro " + res + " correctamente.")
                regresar();
            }
        });
    });
}

function regresar() {
    window.location.href = "/Usuarios";
}