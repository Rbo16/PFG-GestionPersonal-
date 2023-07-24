using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace GestionPersonal.Utiles
{
    public static class Querys
    {
        private static SqlConnection conexionSQL;
        private static string cadenaConexion = ConfigurationManager.ConnectionStrings["GestionPersonal.Properties.Settings.masterConnectionString"].ConnectionString;

        /// <summary>
        /// Comprueba si el correo proporcionado existe en el sistema sin necesidad de crear antes un objeto Empleado.
        /// </summary>
        /// <param name="correo">Correo cuya existencia se desea comprobar.</param>
        /// <returns></returns>
        public static string existeCorreo(string correo)
        {
            string usuario = string.Empty;
            try
            {
                string consulta = "SELECT Usuario FROM Empleado WHERE CorreoE = @CorreoE AND Borrado = 0";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);
                comando.Parameters.Add("@CorreoE", SqlDbType.NVarChar);
                comando.Parameters["@CorreoE"].Value = correo;

                SqlDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    usuario = reader.GetString(0);
                }
                return usuario;
            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Querys.ExisteCorreo]:");
                return usuario;
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// Cambia el estado del empleado cuyo usuario se ha indicado a No Autorizado.
        /// </summary>
        /// <param name="usuario"></param>
        public static void bloquearUsuario(string usuario)
        {
            try
            {
                string consulta = "UPDATE Empleado SET EstadoE = 3 WHERE Usuario = @Usuario";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@Usuario", SqlDbType.NVarChar);
                comando.Parameters["@Usuario"].Value = usuario;

                comando.ExecuteReader();

            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Querys.BloquearUsuario]:");
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        public static string obtenerIdEmpleado(string dni)
        {
            string IdEmpleado = "";
            try
            {
                string consulta = "SELECT IdEmpleado From Empleado WHERE DNI = @DNI";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@DNI", SqlDbType.NVarChar);
                comando.Parameters["@DNI"].Value = dni;

                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    IdEmpleado = reader.GetInt32(0).ToString();
                }
                return IdEmpleado;
            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Querys.IdEmpleado]:");
                return null;
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        public static string obtenerNombreCompleto(string dni)
        {
            string Nombrecompleto = "";
            try
            {
                string consulta = "SELECT CONCAT(Apellido, ', ', NombreE) AS Nombrecompleto From Empleado WHERE DNI = @DNI";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@DNI", SqlDbType.NVarChar);
                comando.Parameters["@DNI"].Value = dni;

                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    Nombrecompleto = reader.GetString(0);
                }
                return Nombrecompleto;
            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Querys.NombreCompleto]:");
                return null;
            }
            finally
            {
                conexionSQL.Close();
            }
        }
    }
}
