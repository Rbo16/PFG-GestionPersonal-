using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Configuration;

namespace GestionPersonal
{
    internal class InteraccionBBDD
    {
        SqlConnection miConexionSql;

        public InteraccionBBDD()
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["GestionPersonal.Properties.Settings.masterConnectionString"].ConnectionString;
            Conectar(cadenaConexion); //Me conecto a la BBDD 
        }

        private bool Conectar(string CadConexion)
        {
            try
            {
                miConexionSql = new SqlConnection(CadConexion);
                return true;
            }
            catch (Exception e) { MessageBox.Show(e.Message); return false; }
        }

        public void ejecutarConsulta(string consulta)
        {

            SqlCommand miSqlComand = new SqlCommand(consulta, miConexionSql);
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-ES"); //Para intentar poner las fechas como corresponden
                if (miConexionSql.State.CompareTo(ConnectionState.Closed) == 0) miConexionSql.Open(); //Comprueba que la conexion no se haya quedado abierta, por ejemplo por un error y que en el catch no se haya cerrado
                miSqlComand.ExecuteNonQuery();
                miConexionSql.Close(); //Cerrar explicitamente o usar el using
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-ES");
            }
            catch (Exception e)
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-ES");
                miConexionSql.Close(); //Cerrar explicitamente o usar el using
                MessageBox.Show("ERROR: " + e.Message);
            }
        }

        public string existe(string consulta)
        {
            try
            {
                DataTable dtA = new DataTable();
                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(consulta, miConexionSql);
                using (miAdaptadorSql)
                {
                    miAdaptadorSql.Fill(dtA);
                }

                if (dtA.Rows.Count > 0) return "SI";
                else return "NO";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return "ERROR"; }
        }

        public DataTable consultaSelect(string consulta)
        {
            try
            {
                DataTable dtA = new DataTable();
                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(consulta, miConexionSql);
                using (miAdaptadorSql)
                {
                    miAdaptadorSql.Fill(dtA);
                }
                return dtA;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return null; }
        }

        public string valor(string consulta)
        {
            try
            {
                DataTable dtA = new DataTable();
                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(consulta, miConexionSql);
                using (miAdaptadorSql)
                {
                    miAdaptadorSql.Fill(dtA);
                }
                if (dtA.Rows.Count > 0)
                {
                    return dtA.Rows[0][0].ToString();
                }
                else return "ERROR: Valores no coincidentes en la BBDD";
            }
            catch (Exception ex) { return "ERROR: " + ex.Message; }
        }

        public string Actualizar(string consulta)
        {
            SqlCommand miSqlComand = new SqlCommand(consulta, miConexionSql);
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-ES"); //Para intentar poner las fechas como corresponden
                if (miConexionSql.State.CompareTo(ConnectionState.Closed) == 0) miConexionSql.Open(); //Comprueba que la conexion no se haya quedado abierta, por ejemplo por un error y que en el catch no se haya cerrado
                miSqlComand.ExecuteNonQuery();
                miConexionSql.Close(); //Cerrar explicitamente o usar el using
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-ES");
                return "OK";
            }
            catch (Exception e)
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-ES");
                miConexionSql.Close(); //Cerrar explicitamente o usar el using
                return e.Message;
            }
        }



        public DataSet MultiplesConsultas(string consulta)
        {
            try
            {
                DataSet dts = new DataSet();
                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(consulta, miConexionSql);
                using (miAdaptadorSql)
                {
                    miAdaptadorSql.Fill(dts);
                }
                return dts;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return null; }
        }

    }
}

