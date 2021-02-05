using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sisCursosOnline.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string LugarNacimiento { get; set; }
        public string Direccion { get; set; }
        public string Genero { get; set; }
        public string Pasatiempos { get; set; }
    }
}