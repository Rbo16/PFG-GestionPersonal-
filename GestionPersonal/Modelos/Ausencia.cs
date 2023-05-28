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

        public int IdAusencia { get; set; }
        private string Razon { get; set; }
        private DateTime FechaInicioA { get; set; }
        private DateTime FechaFinA { get; set; }
        private string DescripcionAus { get; set; }
        private string JustificantePDF { get; set; }
        public EstadoAusencia EstadoA { get; set; }
        private int IdSolicitante { get; set; }
        private int IdAutorizador { get; set; }
        public Auditoria Auditoria { get; set; }


        /// <summary>
        /// Constructor para métodos concretos que no requieren muchos parámetros
        /// </summary>
        public Ausencia()
        {
            this.Razon = string.Empty;
            this.FechaInicioA = FechaInicioA;
            this.FechaFinA = FechaFinA;
            this.DescripcionAus = string.Empty;
            this.JustificantePDF = string.Empty;
            this.EstadoA = EstadoAusencia.Rechazada;
            this.IdSolicitante = 0;
        }

        /// <summary>
        /// Constructor ausencia nueva.
        /// </summary>
        /// <param name="Razon">Indica el motivo de la ausencia</param>
        /// <param name="FechaInicio">Indica la fecha de inicio de la ausencia</param>
        /// <param name="FechaFin">Indica la fecha de fin de la ausencia</param>
        /// <param name="DescripcionAus">Indica la descripcion de la ausencia</param>
        /// <param name="JustificantePD">Indica la ruta en la que se encuentra el justificante</param>
        /// <param name="IdSolicitante">Indica el Id del empleado solicitante</param>
        public Ausencia(string Razon, DateTime FechaInicioA, DateTime FechaFinA, string DescripcionAus,
            string JustificantePD, int IdSolicitante)
        {
            this.Razon= Razon;
            this.FechaInicioA= FechaInicioA;
            this.FechaFinA= FechaFinA;
            this.DescripcionAus= DescripcionAus;
            this.JustificantePDF= JustificantePD;
            this.EstadoA = EstadoAusencia.Pendiente;
            this.IdSolicitante = IdSolicitante;//Al hacer insert es = al IdModif, pero hay que guardarlo para tenerlo cuando haya algún cambio
        }

        /// <summary>
        /// Constructor ausencia a modificar
        /// </summary>
        /// <param name="Razon"></param>
        /// <param name="FechaInicioA"></param>
        /// <param name="FechaFinA"></param>
        /// <param name="DescripcionAus"></param>
        /// <param name="JustificantePD"></param>
        /// <param name="IdSolicitante"></param>
        public Ausencia(int IdAusencia, string Razon, DateTime FechaInicioA, DateTime FechaFinA, string DescripcionAus,
            string JustificantePD, EstadoAusencia EstadoA, int IdSolicitante)
        {
            this.IdAusencia = IdAusencia;
            this.Razon = Razon;
            this.FechaInicioA = FechaInicioA;
            this.FechaFinA = FechaFinA;
            this.DescripcionAus = DescripcionAus;
            this.JustificantePDF = JustificantePD;
            this.EstadoA = EstadoA;
            this.IdSolicitante = IdSolicitante;
        }


        /// <summary>
        /// Gestiona la interacción con la BBDD a la hora de obtener un listado de Ausencias.
        /// </summary>
        /// <returns>Devuelve el Datatable de la tabla Ausencia</returns>
        public DataTable listarAusencias()
        {
            try
            {
                DataTable dtAusencias = new DataTable();

                string consulta = "SELECT * FROM Ausencia ";//WHERE Borrado = 0";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                SqlDataAdapter adaptadorSql = new SqlDataAdapter(comando);
                using (adaptadorSql)
                {
                    adaptadorSql.Fill(dtAusencias);
                }
                return dtAusencias;
            }
            catch(Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Ausencia.Listar]");
                return null;
            }
            finally 
            { 
                conexionSQL.Close(); 
            }
          
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

        public void updateAutorizador(int IdAusencia, int EstadoA,  int IdAutorizador)
        {
            try
            {
                string consulta = "UPDATE Ausencia SET IdAutorizador = @IdAutorizador, EstadoA = @EstadoA , " + Auditoria.Update +
                    " WHERE IdAusencia = @IdAusencia";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@IdAusencia", SqlDbType.Int);
                comando.Parameters["@IdAusencia"].Value = IdAusencia;

                comando.Parameters.Add("@IdAutorizador", SqlDbType.Int);
                comando.Parameters["@IdAutorizador"].Value = IdAutorizador;

                comando.Parameters.Add("@EstadoA", SqlDbType.Int);
                comando.Parameters["@EstadoA"].Value = EstadoA;

                this.Auditoria = new Auditoria(IdAutorizador);
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

        public void deleteAusencia(int IdAusencia, int IdModif)
        {
            try
            {
                string consulta = "UPDATE Ausencia SET " + Auditoria.Update + " WHERE IdAusencia = @IdAusencia";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@IdAusencia", SqlDbType.Int);
                comando.Parameters["@IdAusencia"].Value = IdAusencia;

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
