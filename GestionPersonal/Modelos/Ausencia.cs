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
        private SqlConnection conexionSQL;
        private string cadenaConexion = ConfigurationManager.ConnectionStrings["GestionPersonal.Properties.Settings.masterConnectionString"].ConnectionString;

        private int IdAusencia { get; set; }
        public string Razon { get; set; }
        public DateTime FechaInicioA { get; set; }
        public DateTime FechaFinA { get; set; }
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
            this.EstadoA = EstadoAusencia.Rechazada;
            this.IdSolicitante = 0;
            this.IdAutorizador = 0;
        }


        /// <summary>
        /// Inserta una ausencia creada.
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
        /// Actualiza la ausencia que llama al método en la Base de Datos.
        /// </summary>
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
        /// Devuelve el comando con los parámetros necesarios para insertar una Ausencia especificados.
        /// </summary>
        /// <param name="comando">Comando con los parámetros sin especificar</param>
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
