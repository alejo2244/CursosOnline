$(document).ready(function () {
    var tablaEdades = null;
    var tablaGenero = null;
    var tablaPasatiempo = null;
    obtenerRangoEdades();
    obtenerGeneros();
    obtenerPasatiempos();
});

function obtenerRangoEdades() {
    $.get({
        url: "http://localhost:44381/api/rangoEdades"
    }).then(function (data) {

        tablaEdades = new Tabulator("#tablaRangoEdades", {
            data: data,
            columns: [
                { title: "Rango", field: "Rango", width: "49%", hozAlign: "center" }
                , { title: "Cantidad", field: "Cantidad", width: "49%", hozAlign: "center" }
            ]
        });
    });
}

function obtenerGeneros() {
    $.get({
        url: "http://localhost:44381/api/cantidadGeneros"
    }).then(function (data) {

        tablaGenero = new Tabulator("#tablaCantidadGeneros", {
            data: data,
            columns: [
                { title: "Genero", field: "Genero", width: "49%", hozAlign: "center" }
                , { title: "Cantidad", field: "Cantidad", width: "49%", hozAlign: "center" }
            ]
        });
    });
}

function obtenerPasatiempos() {
    $.get({
        url: "http://localhost:44381/api/pasaTiempos"
    }).then(function (data) {

        tablaGenero = new Tabulator("#tablaPasatiempos", {
            data: data,
            columns: [
                { title: "Nombre Completo", field: "Nombre", width: "49%", hozAlign: "center" }
                , { title: "Hobbie", field: "Pasatiempo", width: "49%", hozAlign: "center" }
            ]
        });
    });
}