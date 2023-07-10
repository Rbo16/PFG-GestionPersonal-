using GestionPersonal.Modelos;
using GestionPersonal.Utiles;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GestionPersonal
{
    public class Ausencia
    {
        private static SqlConnection conexionSQL;
        private static string cadenaConexion = ConfigurationManager.ConnectionStrings["GestionPersonal.Properties.Settings.masterConnectionString"].ConnectionString;

        public int IdAusencia { get; set; }
        public string Razon { get; set; }
        public DateTime? FechaInicioA { get; set; }
        public DateTime? FechaFinA { get; set; }
        public string DescripcionAus { get; set; }
        public string JustificantePDF { get; set; }
        public EstadoAusencia EstadoA { get; set; }
        public int IdSolicitante { get; set; }
        public int IdAutorizador { get; set; }
        public Auditoria Auditoria { get; set; }


        /// <summary>
        /// Constructor para métodos concretos que no requieren muchos parámetros
        /// </summary>
        public Ausencia(int IdAusencia)
        {
            this.IdAusencia = IdAusencia;
            this.Razon = string.Empty;
            this.FechaInicioA = DateTime.MinValue;
            this.FechaFinA = DateTime.MinValue;
            this.DescripcionAus = string.Empty;
            this.JustificantePDF = string.Empty;
            this.EstadoA = EstadoAusencia.Pendiente;
            this.IdSolicitante = 0;
            this.IdAutorizador = 0;
        }


        /// <summary>
        /// Hace el Insert de la ausencia que invocaal método a la BBDD.
        /// </summary>
        public void insertAusencia()
        {
            try
            {
                string consulta = "INSERT INTO Ausencia (IdSolicitante, Razon, FechaInicioA, FechaFinA, DescripcionAus,  " +
                "JustificantePDF, EstadoA," + Auditoria.Insert_1 +
                ") VALUES (@IdSolicitante, @Razon, @FechaInicioA, @FechaFinA, @DescripcionAus, @JustificantePDF, " +
                "@EstadoA, " + Auditoria.Insert_2 + ")";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);
                comando = introducirParametros(comando);
                this.Auditoria = new Auditoria(this.IdSolicitante);
                comando = Auditoria.introducirParametros(comando);

                comando.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                ExceptionManager.Execute(e, "ERROR[Ausencia.Crear]:");
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// Realiza el Update de la ausencia que invoca al método en la BBDD.
        /// </summary>
        /// <param name="IdModif">Id del empleado que ha relizado la modificación.</param>
        public void updateAusencia(int IdModif)
        {
            try
            {
                string consulta = "UPDATE Ausencia SET IdSolicitante = @IdSolicitante, Razon = @Razon, FechaInicioA = @FechaInicioA," +
                    "FechaFinA = @FechaFinA, DescripcionAus = @DescripcionAus, JustificantePDF = @JustificantePDF, " +
                    "EstadoA = @EstadoA, " + Auditoria.Update + "WHERE IdAusencia = @IdAusencia";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);
                comando = introducirParametros(comando);
                this.Auditoria = new Auditoria(IdModif);
                comando = this.Auditoria.introducirParametros(comando);

                comando.Parameters.Add("@IdAusencia", SqlDbType.Int);
                comando.Parameters["@IdAusencia"].Value = this.IdAusencia;

                comando.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                ExceptionManager.Execute(e, " ERROR[Ausencia.Actualizar]:");
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// Realiza el Update relativo a la gestión de la ausencia que invoca al método en la BBDD.
        /// </summary>
        public void updateAutorizador()
        {
            try
            {
                string consulta = "UPDATE Ausencia SET IdAutorizador = @IdAutorizador, EstadoA = @EstadoA , " + Auditoria.Update +
                    " WHERE IdAusencia = @IdAusencia";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@IdAusencia", SqlDbType.Int);
                comando.Parameters["@IdAusencia"].Value = this.IdAusencia;

                comando.Parameters.Add("@IdAutorizador", SqlDbType.Int);
                comando.Parameters["@IdAutorizador"].Value = this.IdAutorizador;

                comando.Parameters.Add("@EstadoA", SqlDbType.Int);
                comando.Parameters["@EstadoA"].Value = this.EstadoA;

                this.Auditoria = new Auditoria(this.IdAutorizador);
                comando = Auditoria.introducirParametros(comando);

                comando.ExecuteNonQuery();
            }
            catch(Exception e) 
            {
                ExceptionManager.Execute(e, " ERROR[Ausencia.Autorizar]:");
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// Realiza el borrado lógico de la ausencia que lo invoca en la BBDD.
        /// </summary>
        /// <param name="IdModif">Id del empleado que ha relizado el borrado.</param>
        public void deleteAusencia(int IdModif)
        {
            try
            {
                string consulta = "UPDATE Ausencia SET " + Auditoria.Update + " WHERE IdAusencia = @IdAusencia";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@IdAusencia", SqlDbType.Int);
                comando.Parameters["@IdAusencia"].Value = this.IdAusencia;

                this.Auditoria = new Auditoria(IdModif, true);
                comando = Auditoria.introducirParametros(comando);

                comando.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                ExceptionManager.Execute(e, " ERROR[Ausencia.Borrar]:");
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// Devuelve la ausencia cuyo Id se ha indicado. Utilizado para realizar las pruebas unitarias.
        /// </summary>
        /// <param name="IdAusencia">Id de la ausencia a obtener.</param>
        /// <returns></returns>
        public static Ausencia obtenerAusencia(int IdAusencia)
        {
            Ausencia ausencia = new Ausencia(IdAusencia);

            try
            {
                string consulta = "SELECT * FROM Ausencia WHERE IdAusencia = @IdAusencia";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@IdAusencia", SqlDbType.Int);
                comando.Parameters["@IdAusencia"].Value = IdAusencia;

                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    ausencia.IdSolicitante = reader.GetInt32(1);
                    if (!reader.IsDBNull(2))
                        ausencia.IdAutorizador = reader.GetInt32(2);
                    ausencia.Razon = reader.GetString(3);
                    ausencia.FechaInicioA = reader.GetDateTime(4);
                    ausencia.FechaFinA = reader.GetDateTime(5);
                    if(!reader.IsDBNull(6))
                        ausencia.DescripcionAus = reader.GetString(6);
                    if (!reader.IsDBNull(7))
                        ausencia.JustificantePDF = reader.GetString(7);
                    ausencia.EstadoA = (EstadoAusencia)reader.GetInt32(8);

                    ausencia.Auditoria = new Auditoria(reader.GetInt32(10), reader.GetDateTime(9),
                        reader.GetBoolean(11));
                    
                }

                return ausencia;
            }
            catch (Exception e)
            {
                ExceptionManager.Execute(e, "ERROR[Ausencia.Obtener]:");
                return null;
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// Devuelve el IdAusencia más alto, es decir, el id de la última ausencia creada. Utilizado para realizar las pruebas unitarias.
        /// </summary>
        /// <returns></returns>
        public static int maxIdAusencia()
        {
            int max = -1;
            try
            {
                string consulta = "SELECT MAX(IdAusencia) FROM Ausencia";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    max= reader.GetInt32(0);
                }

                return max;
            }
            catch (Exception e)
            {
                ExceptionManager.Execute(e, "ERROR[Ausencia.MaxId]:");
                return max;
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// Devuelve el comando con los parámetros necesarios para insertar una Ausencia especificados.
        /// </summary>
        /// <param name="comando">Comando sql que se desea completar.</param>
        /// <returns></returns>
        private SqlCommand introducirParametros(SqlCommand comando)
        {
            comando.Parameters.Add("@IdSolicitante",SqlDbType.Int);
            comando.Parameters.Add("@Razon", SqlDbType.NVarChar);
            comando.Parameters.Add("@FechaInicioA", SqlDbType.Date);
            comando.Parameters.Add("@FechaFinA", SqlDbType.Date);
            comando.Parameters.Add("@DescripcionAus", SqlDbType.NVarChar);
            comando.Parameters.Add("@JustificantePDF", SqlDbType.NVarChar);
            comando.Parameters.Add("@EstadoA", SqlDbType.Int);

            comando.Parameters["@IdSolicitante"].Value = this.IdSolicitante;
            comando.Parameters["@Razon"].Value = this.Razon;
            comando.Parameters["@FechaInicioA"].Value = this.FechaInicioA;
            comando.Parameters["@FechaFinA"].Value = this.FechaFinA;
            comando.Parameters["@DescripcionAus"].Value = this.DescripcionAus;
            comando.Parameters["@JustificantePDF"].Value = this.JustificantePDF;
            comando.Parameters["@EstadoA"].Value = this.EstadoA.GetHashCode();

            return comando;
        }

    }
    public enum EstadoAusencia
    {
        Aceptada = 1, Pendiente = 2, Rechazada = 3
    }
}
