using sisCursosOnline.Helpers;
using sisCursosOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace sisCursosOnline.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UsuarioController : ApiController
    {
        private Conexion conn = new Conexion();

        [HttpGet]
        public List<Usuario> ObtenerRegistro() => conn.TraerUsuarios();

        [HttpGet]
        public Usuario GetRegistro(int id) => conn.TraerUsuario(id);

        [HttpPut]
        public bool CrearRegistro(Usuario registro) => conn.CrearUsuario(registro);

        [HttpPost]
        public bool ModificarRegistro(Usuario registro) => conn.ModificarUsuario(registro);

        [HttpDelete]
        public bool EliminarRegistro(int id) => conn.EliminarUsuario(id);

        [HttpGet]
        [Route("api/rangoEdades")]
        public List<RangoEdad> RangoEdades() => conn.TraerEdades();

        [HttpGet]
        [Route("api/cantidadGeneros")]
        public List<CantidadGenero> CantidadGeneros() => conn.TraerGeneros();

        [HttpGet]
        [Route("api/pasaTiempos")]
        public List<Pasatiempos> Pasatiempos() => conn.TraerPasatiempos();
    }
}
