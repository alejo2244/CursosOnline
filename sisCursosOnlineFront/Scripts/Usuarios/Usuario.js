$(document).ready(function () {
    var table = null;
    obtenerUsuarios();
});

function filtroID() {
    var x = document.getElementById("filtro");
    if (x.value === "") {
        removeFilter();
    }
    else {
        removeFilter();
        addFilter(x.value);
    }    
}

function addFilter(id) {
    table.setFilter("Id", "=", id);
}

function removeFilter() {
    table.clearFilter();
}

function obtenerUsuarios() {
    $.get({
        url: "http://localhost:44381/api/usuario"
    }).then(function (data) {

        table = new Tabulator("#tablaUsuarios", {
            data: data, //assign data to table
            columns: [
                {
                    title: "Editar", formatter: function (e, cell) { return "<button class=\"btn btn-primary\">Detalle</button>"; }, width: "10%",
                    cellClick: function (e, cell) {
                        var data = cell._cell.row.data;
                        enviarDatos(data.Id);
                    }
                },
                {
                    title: "Eliminar", formatter: function (e, cell) { return "<button class=\"btn btn-danger\">Eliminar</button>"; }, width: "10%",
                    cellClick: function (e, cell) {
                        if (confirm("Seguro que desea eliminar el registro?") === true) {
                            fetch("http://localhost:44381/api/usuario/" + cell._cell.row.data.Id, {
                                method: 'DELETE'
                            }).then(function (data) {
                                if (data) {
                                    obtenerUsuarios();
                                    alert("Se elimino correctamente el registro.");
                                }
                            });
                        }
                    }
                }
                , { title: "Id", field: "Id", width: "5%", align: "center" }
                , { title: "Nombres", field: "Nombres", width: "15%" }
                , { title: "Primer Apellido", field: "PrimerApellido", width: "15%" }
                , { title: "Segundo Apellido", field: "SegundoApellido", width: "17%" }
                , {
                    title: "Fecha Nacimiento", field: "FechaNacimiento", formatter: "datetime", formatterParams: {
                        outputFormat: "DD/MM/YYYY"
                    }
                    , width: "20%"
                }
                , { title: "Lugar Nacimiento", field: "LugarNacimiento", width: "20%" }
                , { title: "Direccion", field: "Direccion" }
            ]
        });
    });

}

function crearDatos() {
    window.location.href = "Usuarios/Detalle";
}

function enviarDatos(data) {
    window.location.href = "Usuarios/Detalle/" + data;
}