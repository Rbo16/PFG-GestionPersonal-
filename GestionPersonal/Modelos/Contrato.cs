using GestionPersonal.Modelos;
using GestionPersonal.Utiles;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPersonal
{
    public class Contrato
    {
        private SqlConnection conexionSQL;
        private string cadenaConexion = ConfigurationManager.ConnectionStrings["GestionPersonal.Properties.Settings.masterConnectionString"].ConnectionString;

        private int IdContrato { get; set; }
        private float HorasTrabajo { get; set; }
        private float HorasDescanso { get; set; }
        private TimeSpan HoraEntrada { get; set; }
        private TimeSpan HoraSalida { get; set; }
        private float Salario { get; set; }
        private string Puesto { get; set; }
        private float VacacionesMes { get; set; }
        private DateTime FechaAlta { get; set; }
        private DateTime FechaBaja { get; set; }
        private string Duracion { get; set; }
        private bool Activo { get; set; }
        private string DocumentoPDF { get; set; }
        private int IdEmpleado { get; set; }
        TipoContrato TipoContrato { get; set; }
        Auditoria Auditoria { get; set; }

        public Contrato() { }

        /// <summary>
        /// Constructor de contrato nuevo.
        /// </summary>
        /// <param name="HorasTrabajo">Horas diarias trabajadas</param>
        /// <param name="HorasDescanso">Horas diarias de descanso</param>
        /// <param name="HoraEntrada"> Hora de entrada diaria</param>
        /// <param name="HoraSalida">Hora de salida diaria</param>
        /// <param name="Salario">Salario anual</param>
        /// <param name="Puesto">Nombre del puesto dentro de la empresa</param>
        /// <param name="VacacionesMes">Días de vacaciones por mes</param>
        /// <param name="Duracion">Duración del contrato</param>
        /// <param name="DocumentoPDF">Ruta en la que se encuenrta el PDF con la información del contrato</param>
        /// <param name="IdEmpleado">Id del empleado poseedor del contrato</param>
        /// <param name="TipoContrato">Tipo de contrato</param>
        public Contrato(float HorasTrabajo, float HorasDescanso, TimeSpan HoraEntrada,
            TimeSpan HoraSalida, float Salario, string Puesto, float VacacionesMes,
            string Duracion, string DocumentoPDF, int IdEmpleado,
            TipoContrato TipoContrato)
        {

            this.HorasTrabajo = HorasTrabajo;
            this.HorasDescanso = HorasDescanso;
            this.HoraEntrada = HoraEntrada;
            this.HoraSalida = HoraSalida;
            this.Salario = Salario;
            this.Puesto = Puesto;
            this.VacacionesMes = VacacionesMes;
            this.Duracion = Duracion;
            this.DocumentoPDF = DocumentoPDF;
            this.IdEmpleado = IdEmpleado;
            this.TipoContrato = TipoContrato;
        }

        public DataTable listadoContratos()
        {
            try
            {
                DataTable dtContratos= new DataTable();

                string consulta = "SELECT * FROM Contrato";//WHERE Borrado = 0";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                SqlDataAdapter adaptadorSql = new SqlDataAdapter(comando);
                using (adaptadorSql)
                {
                    adaptadorSql.Fill(dtContratos);
                }

                return dtContratos;
            }
            catch(Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Contrato.Listar]");
                return null;
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        public void insertContrato(int IdModif)
        {
            try
            {
                //cambiarActivo(this.IdEmpleado);

                string consulta = "INSERT INTO Contrato (HorasTrabajo, HorasDescanso, HoraEntrada, HoraSalida, Salario, " +
                    "Puesto, VacacionesMes, FechaAlta, Duracion, Activo, DocumentoPDF, IdEmpleado, " +
                    "TipoContrato, " + Auditoria.Insert_1 + ") VALUES (@HorasTrabajo, @HorasDescanso, @HoraEntrada, @HoraSalida, " +
                    "@Salario, @Puesto, @VacacionesMes, @FechaAlta, @Duracion, 1, @DocumentoPDF, @IdEmpleado, " +
                    "@TipoContrato, " + Auditoria.Insert_2 + ")";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);
                this.FechaAlta = DateTime.Now;
                comando = introducirParametros(comando);
                this.Auditoria = new Auditoria(IdModif);
                comando = Auditoria.introducirParametros(comando);

                comando.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Contrato.Crear]:");
            }
            finally 
            {
                conexionSQL.Close();
            }
        }

        private void cambiarActivo(int IdEmpleado)
        {
            try
            {
                string consulta = "UPDATE Contrato SET Activo = 0, FechaBaja = GETDATE()" +
                    " WHERE IdEmpleado = @IdEmpleado AND Activo = 1";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);
                comando.Parameters.Add("@IdEmpleado");
                comando.Parameters["@IdEmpleado"].Value = IdEmpleado;

                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Contrato.CambiarActivo]:");
            }
            finally
            {
                conexionSQL.Close();
            }
        }


        private SqlCommand introducirParametros(SqlCommand comando)
        {
            comando.Parameters.Add("@HorasTrabajo",SqlDbType.Float);
            comando.Parameters.Add("@HorasDescanso", SqlDbType.Float);
            comando.Parameters.Add("@HoraEntrada", SqlDbType.Time);
            comando.Parameters.Add("@HoraSalida", SqlDbType.Time);
            comando.Parameters.Add("@Salario", SqlDbType.Int);
            comando.Parameters.Add("@Puesto", SqlDbType.NVarChar);
            comando.Parameters.Add("@VacacionesMes", SqlDbType.Float);
            comando.Parameters.Add("@FechaAlta", SqlDbType.Date);
            comando.Parameters.Add("@Duracion", SqlDbType.NVarChar);
            comando.Parameters.Add("@DocumentoPDF", SqlDbType.NVarChar);
            comando.Parameters.Add("@IdEmpleado", SqlDbType.Int);
            comando.Parameters.Add("@TipoContrato", SqlDbType.Int);

            comando.Parameters["@HorasTrabajo"].Value = this.HorasTrabajo;
            comando.Parameters["@HorasDescanso"].Value = this.HorasDescanso;
            comando.Parameters["@HoraEntrada"].Value = this.HoraEntrada;
            comando.Parameters["@HoraSalida"].Value = this.HoraSalida;
            comando.Parameters["@Salario"].Value = this.Salario;
            comando.Parameters["@Puesto"].Value = this.Puesto;
            comando.Parameters["@VacacionesMes"].Value = this.VacacionesMes;
            comando.Parameters["@FechaAlta"].Value = this.FechaAlta;
            comando.Parameters["@Duracion"].Value = this.Duracion;
            comando.Parameters["@DocumentoPDF"].Value = this.DocumentoPDF;
            comando.Parameters["@IdEmpleado"].Value = this.IdEmpleado;
            comando.Parameters["@TipoContrato"].Value = this.TipoContrato;

            return comando;
        }
    }
    public enum TipoContrato
    {
        ParcialManiana, ParcialTarde, Completa, Practicas
    }
}
