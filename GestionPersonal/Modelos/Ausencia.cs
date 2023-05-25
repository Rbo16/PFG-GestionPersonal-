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
        private string Razon { get; set; }
        private DateTime FechaInicioA { get; set; }
        private DateTime FechaFinA { get; set; }
        private string DescripcionAus { get; set; }
        private string JustificantePDF { get; set; }
        public EstadoAusencia EstadoAus { get; set; }
        private int IdSolicitante { get; set; }
        private int IdGestor { get; set; }
        private Auditoria Auditoria { get; set; }


        /// <summary>
        /// Constructor vacío para usar los métodos 
        /// </summary>
        public Ausencia()
        {
            this.Razon = string.Empty;
            this.FechaInicioA = FechaInicioA;
            this.FechaFinA = FechaFinA;
            this.DescripcionAus = string.Empty;
            this.JustificantePDF = string.Empty;
            this.EstadoAus = EstadoAusencia.Rechazada;
            this.IdSolicitante = 0;
        }

        /// <summary>
        /// Constructor ausencia nueva
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
            this.EstadoAus = EstadoAusencia.Pendiente;
            this.IdSolicitante = IdSolicitante;//Aunque hay que guardarlo permanentemente también
        }


        /// <summary>
        /// Gestiona la interacción con la BBDD a la hora de obtener un listado de Ausencias
        /// </summary>
        /// <param name="IdSolicitante">Id del empleado del que se quieren obtener las ausencias. 0 si se busca
        /// un listado general</param>
        /// <returns></returns>
        public DataTable listarAusencias(int IdSolicitante)
        {
            try
            {
                DataTable dtAusencias = new DataTable();

                string consulta = "SELECT * FROM Ausencia WHERE Borrado = 0";
                if (IdSolicitante != 0)//Se comparará con 0 cuando se quieran obtener todas las ausencias
                {
                    consulta += " AND IdSolicitante = @IdSolicitante";
                }

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);
                if (IdSolicitante != 0)
                {
                    comando.Parameters.Add("@IdSolicitante", SqlDbType.Int);
                    comando.Parameters["@IdSolicitante"].Value = IdSolicitante;
                }

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
          
        }

        /// <summary>
        /// Inserta una ausencia creada
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
                this.Auditoria = new Auditoria(IdSolicitante);
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
        /// Devuelve el comando con los parámetros necesarios para insertar una Ausencia especificados
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
            comando.Parameters["@EstadoA"].Value = this.EstadoAus.GetHashCode();

            return comando;
        }
    }
    public enum EstadoAusencia
    {
        Aceptada = 1, Pendiente = 2, Rechazada = 3
    }
}
