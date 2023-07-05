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
                string consulta = "SELECT Usuario FROM Empleado Where CorreoE = @CorreoE";//+WHERE Borrado = 0";

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
    }
}
