using GestionPersonal.Utiles;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using GestionPersonal.Modelos;

namespace GestionPersonal
{
    public class Proyecto
    {
        private SqlConnection conexionSQL;
        private string cadenaConexion = ConfigurationManager.ConnectionStrings["GestionPersonal.Properties.Settings.masterConnectionString"].ConnectionString;

        private int IdProyecto { get; set; }
        public string NombreP { get; set; }
        public string Cliente { get; set; }
        public string Tiempo { get; set; }
        public DateTime FechaInicioP { get; set; }
        public DateTime FechaFinP { get; set; }
        public int Presupuesto { get; set; }
        public TipoPrioridad Prioridad { get; set; }
        public string DescripcionP { get; set; }
        public Auditoria Auditoria { get; set; }


        public Proyecto(int IdProyecto) 
        { 
            this.IdProyecto= IdProyecto;
            this.NombreP = string.Empty;
            this.Cliente = string.Empty;
            this.Tiempo = string.Empty;
            this.FechaInicioP = DateTime.MinValue;
            this.FechaFinP= DateTime.MinValue;
            this.Presupuesto= 0;
            this.Prioridad= new TipoPrioridad();
            this.DescripcionP= string.Empty;
        }

        public void insertarProyecto(int IdModif)
        {
            try
            {
                string consulta = "INSERT INTO Proyecto (NombreP, Cliente, FechaInicioP, Tiempo, Presupuesto, Prioridad, " + 
                    "DescripcionP, " + Auditoria.Insert_1 + ") VALUES (@NombreP, @Cliente, @FechaInicioP, @Tiempo, @Presupuesto, " +
                    "@Prioridad, @DescripcionP, " + Auditoria.Insert_2 + ")";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);
                comando = introducirParametros(comando);
                this.Auditoria = new Auditoria(IdModif, true);
                comando = Auditoria.introducirParametros(comando);

                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Proyecto.Crear]:");
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        public DataTable listarProyectos()
        {
            try
            {
                DataTable dtProyectos = new DataTable();

                string consulta = "SELECT * FROM Proyecto ";//+
                    //"INNER JOIN ParticipacionProyecto ON Proyecto.IdProyecto = " +
                   // "ParticipacionProyecto.IdProyecto";//WHERE Borrado = 0";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                SqlDataAdapter adaptadorSql = new SqlDataAdapter(comando);
                using (adaptadorSql)
                {
                    adaptadorSql.Fill(dtProyectos);
                }

                return dtProyectos;

            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Proyecto.Listar]:");
                return null;
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        public void updateProyecto(int IdModif)
        {
            try
            {
                string consulta = "UPDATE Proyecto SET NombreP = @NombreP, Cliente = @Cliente, FechaInicioP = @FechaInicioP, " +
                    "Tiempo = @Tiempo, Presupuesto = @Presupuesto, Prioridad = @Prioridad, DescripcionP = @DescripcionP, "
                    + Auditoria.Update + " WHERE IdProyecto = @IdProyecto";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);
                comando = introducirParametros(comando);

                comando.Parameters.Add("@IdProyecto");
                comando.Parameters["@IdProyecto"].Value = this.IdProyecto;

                this.Auditoria = new Auditoria(IdModif);
                comando = Auditoria.introducirParametros(comando);

                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Proyecto.Modificar]:");
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        private SqlCommand introducirParametros(SqlCommand comando)
        {
            comando.Parameters.Add("@NombreP", SqlDbType.NVarChar);
            comando.Parameters.Add("@Cliente", SqlDbType.NVarChar);
            comando.Parameters.Add("@FechaInicioP", SqlDbType.Date);
            comando.Parameters.Add("@Tiempo", SqlDbType.NVarChar);
            comando.Parameters.Add("@Presupuesto", SqlDbType.Int);
            comando.Parameters.Add("@Prioridad", SqlDbType.Int);
            comando.Parameters.Add("@DescripcionP", SqlDbType.NVarChar);

            comando.Parameters["@NombreP"].Value = this.NombreP;
            comando.Parameters["@Cliente"].Value = this.Cliente;
            comando.Parameters["@FechaInicioP"].Value = this.FechaInicioP;
            comando.Parameters["@Tiempo"].Value = this.Tiempo;
            comando.Parameters["@Presupuesto"].Value = this.Presupuesto;
            comando.Parameters["@Prioridad"].Value = this.Prioridad;
            comando.Parameters["@DescripcionP"].Value = this.DescripcionP;

            return comando;
        }
    }
    public enum TipoPrioridad
    {
        Urgente = 1, Alta = 2, Moderada = 3, Baja = 4
    }
}

