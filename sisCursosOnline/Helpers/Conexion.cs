using sisCursosOnline.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace sisCursosOnline.Helpers
{
    public class Conexion
    {
        private SqlConnection Conn()
        {
            SqlConnection conexion = new SqlConnection("server=" + ConfigurationManager.AppSettings["instanciaBD"] + "; database=" + ConfigurationManager.AppSettings["BD"] + "; integrated security = true");
            return conexion;
        }

        private static readonly string Tabla = "TblUsuario";
        private static readonly string Format = "yyyy-MM-dd HH:mm:ss";

        public List<Usuario> TraerUsuarios()
        {

            List<Usuario> listaUsuarios = new List<Usuario>();
            try
            {
                SqlCommand command = new SqlCommand("SELECT * FROM " + Tabla, Conn());
                command.Connection.Open();
                command.CommandTimeout = 15;
                command.CommandType = CommandType.Text;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Usuario usuario = new Usuario
                    {
                        Id = (int)reader[0],
                        Nombres = reader[1].ToString(),
                        PrimerApellido = reader[2].ToString(),
                        SegundoApellido = reader[3].ToString(),
                        FechaNacimiento = (DateTime)reader[4],
                        LugarNacimiento = reader[5].ToString(),
                        Direccion = reader[6].ToString(),
                        Genero = reader[7].ToString(),
                        Pasatiempos = reader[8].ToString()
                    };
                    listaUsuarios.Add(usuario);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn().Close();
            }
            return listaUsuarios;
        }

        public Usuario TraerUsuario(int id)
        {
            Usuario usuario = null;
            try
            {
                SqlCommand command = new SqlCommand("SELECT * FROM " + Tabla + " where id = " + id, Conn());
                command.Connection.Open();
                command.CommandTimeout = 15;
                command.CommandType = CommandType.Text;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    usuario = new Usuario
                    {
                        Id = (int)reader[0],
                        Nombres = reader[1].ToString(),
                        PrimerApellido = reader[2].ToString(),
                        SegundoApellido = reader[3].ToString(),
                        FechaNacimiento = (DateTime)reader[4],
                        LugarNacimiento = reader[5].ToString(),
                        Direccion = reader[6].ToString(),
                        Genero = reader[7].ToString(),
                        Pasatiempos = reader[8].ToString()
                    };
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn().Close();
            }
            return usuario;
        }

        public bool CrearUsuario(Usuario reg)
        {
            try
            {
                string query = "Insert into " + Tabla + " values(" +
                    "'" + reg.Nombres + "'" +
                    ",'" + reg.PrimerApellido + "'" +
                    ",'" + reg.SegundoApellido + "'" +
                    ",'" + reg.FechaNacimiento.ToString(Format) + "'" +
                    ",'" + reg.LugarNacimiento + "'" +
                    ",'" + reg.Direccion + "'" +
                    ",'" + reg.Genero + "'" +
                    ",'" + reg.Pasatiempos + "'" +
                    ")";

                EjecutarQuery(query);

                SqlCommand command = new SqlCommand("SELECT MAX(id) from TblUsuario", Conn());
                command.Connection.Open();
                command.CommandTimeout = 15;
                command.CommandType = CommandType.Text;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    reg.Id = Int32.Parse(reader[0].ToString());
                }
                reader.Close();

                Pasatiempo(reg.Pasatiempos, reg.Id);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool Pasatiempo(string pasatiempo, int idUsuario) {
            string[] conectores = 
                { "podemos", "de", "y", "además", "ademas", "tambien", "también", "me", "gusta", "generalmente", "vez", "cuando", "el", "la", "los", "pero", "no", "si", "es", "ya" };

            try
            {
                string[] arrPasatiempo = pasatiempo.Split(' ');
                foreach (string item in arrPasatiempo)
                {
                    if (!Array.Exists(conectores, element => element == item))
                    {
                        string query = "select id from TblPasatiempos where nombre like ('%" + item.ToUpper() + "%')";

                        SqlCommand command = new SqlCommand(query, Conn());
                        command.Connection.Open();
                        command.CommandTimeout = 15;
                        command.CommandType = CommandType.Text;
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            EjecutarQuery("insert into TblPasatiemposUsuario values (" + (int)reader[0] + "," + idUsuario + ")");
                        }
                        reader.Close();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool ModificarUsuario(Usuario reg)
        {
            string query = "Update " + Tabla + " set " +
                "nombres = '" + reg.Nombres + "'" +
                (String.IsNullOrEmpty(reg.PrimerApellido) ? "" : ", primerApellido = '" + reg.PrimerApellido + "'") +
                (String.IsNullOrEmpty(reg.SegundoApellido) ? "" : ", segundoApellido = '" + reg.SegundoApellido + "'") +
                (String.IsNullOrEmpty(reg.FechaNacimiento.ToString()) ? "" : ", fechaNacimiento = '" + reg.FechaNacimiento.ToString(Format) + "'") +
                (String.IsNullOrEmpty(reg.LugarNacimiento) ? "" : ", lugarNacimiento = '" + reg.LugarNacimiento + "'") +
                (String.IsNullOrEmpty(reg.Direccion) ? "" : ", direccion = '" + reg.Direccion + "'") +
                (String.IsNullOrEmpty(reg.Genero) ? "" : ", genero = '" + reg.Genero + "'") +
                (String.IsNullOrEmpty(reg.Pasatiempos) ? "" : ", pasaTiempos = '" + reg.Pasatiempos + "'") +
                " where id = " + reg.Id;
            return EjecutarQuery(query);
        }

        public bool EliminarUsuario(int id)
        {
            string query = "Delete from " + Tabla + " where id = " + id;
            EjecutarQuery("delete from TblPasatiemposUsuario where idUsuario = "+ id);
            return EjecutarQuery(query);
        }

        private bool EjecutarQuery(string query)
        {

            bool res = false;
            try
            {
                using (Conn())
                {
                    SqlCommand command = new SqlCommand(query, Conn());
                    command.Connection.Open();
                    command.CommandText = query;
                    command.CommandTimeout = 15;
                    command.CommandType = CommandType.Text;
                    if (command.ExecuteNonQuery() > 0)
                    {
                        res = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn().Close();
            }
            return res;
        }


        public List<RangoEdad> TraerEdades()
        {
            List<RangoEdad> listaEdades = new List<RangoEdad>();
            try
            {
                SqlCommand command = new SqlCommand("exec SP_VerificarEdades", Conn());
                command.Connection.Open();
                command.CommandTimeout = 15;
                command.CommandType = CommandType.Text;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    RangoEdad edad = new RangoEdad
                    {
                        Rango = "0 y 10 Años",
                        Cantidad = (int)reader[0]
                    };
                    listaEdades.Add(edad);
                    edad = new RangoEdad
                    {
                        Rango = "11 y 20 Años",
                        Cantidad = (int)reader[1]
                    };
                    listaEdades.Add(edad);
                    edad = new RangoEdad
                    {
                        Rango = "21 y 50 Años",
                        Cantidad = (int)reader[2]
                    };
                    listaEdades.Add(edad);
                    edad = new RangoEdad
                    {
                        Rango = "Mayores de 50 Años",
                        Cantidad = (int)reader[3]
                    };
                    listaEdades.Add(edad);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn().Close();
            }
            return listaEdades;
        }

        public List<CantidadGenero> TraerGeneros()
        {
            List<CantidadGenero> listaCantidadGenero = new List<CantidadGenero>();
            try
            {
                SqlCommand command = new SqlCommand("exec SP_VerificarGenero", Conn());
                command.Connection.Open();
                command.CommandTimeout = 15;
                command.CommandType = CommandType.Text;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CantidadGenero cantidad = new CantidadGenero
                    {
                        Genero = reader[1].ToString(),
                        Cantidad = (int)reader[0]
                    };
                    listaCantidadGenero.Add(cantidad);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn().Close();
            }
            return listaCantidadGenero;
        }
        public List<Pasatiempos> TraerPasatiempos()
        {
            List<Pasatiempos> listaPasatiempos = new List<Pasatiempos>();
            try
            {
                SqlCommand command = new SqlCommand("exec SP_TraerPasatiemposUsuario", Conn());
                command.Connection.Open();
                command.CommandTimeout = 15;
                command.CommandType = CommandType.Text;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Pasatiempos hobbie = new Pasatiempos
                    {
                        Nombre = reader[0].ToString(),
                        Pasatiempo = reader[1].ToString()
                    };
                    listaPasatiempos.Add(hobbie);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn().Close();
            }
            return listaPasatiempos;
        }
    }
}