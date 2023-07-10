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
        private static SqlConnection conexionSQL;
        private static string cadenaConexion = ConfigurationManager.ConnectionStrings["GestionPersonal.Properties.Settings.masterConnectionString"].ConnectionString;

        private int IdContrato { get; set; }
        public double HorasTrabajo { get; set; }
        public double HorasDescanso { get; set; }
        public TimeSpan HoraEntrada { get; set; }
        public TimeSpan HoraSalida { get; set; }
        public double Salario { get; set; }
        public string Puesto { get; set; }
        public double VacacionesMes { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime FechaBaja { get; set; }
        public string Duracion { get; set; }
        public bool Activo { get; set; }
        public string DocumentoPDF { get; set; }
        public int IdEmpleado { get; set; }
        public TipoContrato TipoContrato { get; set; }
        public Auditoria Auditoria { get; set; }

        public Contrato(int IdContrato) 
        {
            this.IdContrato= IdContrato;
            this.HorasTrabajo = 0;
            this.HorasDescanso = 0;
            this.HoraEntrada = TimeSpan.Zero;
            this.HoraSalida = TimeSpan.Zero;
            this.Salario = 0;
            this.Puesto = "";
            this.VacacionesMes = 0;
            this.Duracion = "";
            this.DocumentoPDF = "";
            this.IdEmpleado = 0;
            this.TipoContrato = TipoContrato.ParcialManiana;
        }



        /// <summary>
        /// Hace el insert del contrato que invoca al método a la BBDD, estableciéndolo como contrato Activo.
        /// </summary>
        /// <param name="IdModif"></param>
        public void insertContrato(int IdModif)
        {
            try
            {
                cambiarActivo();

                string consulta = "INSERT INTO Contrato (HorasTrabajo, HorasDescanso, HoraEntrada, HoraSalida, Salario, " +
                    "Puesto, VacacionesMes, FechaAlta, Duracion, Activo, DocumentoPDF, IdEmpleado, " +
                    "TipoContrato, " + Auditoria.Insert_1 + ") VALUES (@HorasTrabajo, @HorasDescanso, @HoraEntrada, @HoraSalida, " +
                    "@Salario, @Puesto, @VacacionesMes, @FechaAlta, @Duracion, 1, @DocumentoPDF, @IdEmpleado, " +
                    "@TipoContrato, " + Auditoria.Insert_2 + ")";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);
                this.FechaAlta = DateTime.Now.Date;
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

        /// <summary>
        /// Establece el contrato que invoca al método como Activo dentro de la BBDD.
        /// </summary>
        public void cambiarActivo()
        {
            try
            {
                string consulta = "UPDATE Contrato SET Activo = 0, FechaBaja = @FechaBaja" +
                    " WHERE IdEmpleado = @IdEmpleado AND Activo = 1";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);
                comando.Parameters.Add("@IdEmpleado", SqlDbType.Int);
                comando.Parameters["@IdEmpleado"].Value = this.IdEmpleado;

                this.FechaBaja = DateTime.Now;
                comando.Parameters.Add("@FechaBaja", SqlDbType.DateTime);
                comando.Parameters["@FechaBaja"].Value = this.FechaBaja;

                
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
        
        /// <summary>
        /// Realiza el Update del contrato que invoca al método en la BBDD. 
        /// </summary>
        /// <param name="IdModif">Id del empleado que ha relizado la modificación.</param>
        public void updateContrato(int IdModif)
        {
            try
            {
                string consulta = "UPDATE  Contrato SET HorasTrabajo = @HorasTrabajo, HorasDescanso = @HorasDescanso, " +
                    "HoraEntrada = @HoraEntrada, HoraSalida = @HoraSalida, Salario = @Salario, Puesto = @Puesto, " +
                    "VacacionesMes = @VacacionesMes, Duracion = @Duracion, " +
                    "DocumentoPDF = @DocumentoPDF, TipoContrato = @TipoContrato, "
                    + Auditoria.Update + " WHERE IdContrato = @IdContrato";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);
                comando = introducirParametros(comando);
                comando.Parameters.Add("@IdContrato",SqlDbType.Int);
                comando.Parameters["@IdContrato"].Value = this.IdContrato;
                this.Auditoria = new Auditoria(IdModif);
                comando = Auditoria.introducirParametros(comando);

                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Contrato.Update]:");
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// Realiza el borrado lógico del contrato que invoca al método en la BBDD.
        /// </summary>
        /// <param name="IdModif">Id del empleado que ha relizado el borrado.</param>
        public void deleteContrato(int IdModif)
        {
            try
            {
                string consulta = "UPDATE Contrato SET " + Auditoria.Update +
                    "WHERE IdContrato = @IdContrato";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@IdContrato", SqlDbType.Int);
                comando.Parameters["@IdContrato"].Value = this.IdContrato;

                this.Auditoria = new Auditoria(IdModif, true);
                comando = this.Auditoria.introducirParametros(comando);

                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Contrato.Borrar]:");
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// Devuelve el contrato cuyo Id se ha indicado. Utilizado para realizar las pruebas unitarias.
        /// </summary>
        /// <param name="IdContrato">Id del contrato a obtener.</param>
        /// <returns></returns>
        public static Contrato obtenerContrato(int IdContrato)
        {
            Contrato contrato = new Contrato(IdContrato);
            try
            {
                string consulta = "SELECT * FROM Contrato WHERE IdContrato = @IdContrato";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@IdContrato", SqlDbType.Int);
                comando.Parameters["@IdContrato"].Value = IdContrato;

                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    contrato.IdEmpleado = reader.GetInt32(1);
                    contrato.HorasTrabajo = reader.GetDouble(2);
                    contrato.HoraEntrada = reader.GetTimeSpan(3);
                    contrato.HoraSalida = reader.GetTimeSpan(4);
                    contrato.HorasDescanso = reader.GetDouble(5);
                    contrato.Salario = reader.GetDouble(6);
                    contrato.Puesto = reader.GetString(7);
                    contrato.VacacionesMes = reader.GetDouble(8);
                    contrato.FechaAlta = reader.GetDateTime(9);
                    if(!reader.IsDBNull(10))
                        contrato.FechaBaja = reader.GetDateTime(10);
                    contrato.Duracion = reader.GetString(11);
                    contrato.TipoContrato = (TipoContrato)reader.GetInt32(12);
                    contrato.Activo = reader.GetBoolean(13);
                    contrato.DocumentoPDF = reader.GetString(14);
                    contrato.Auditoria = new Auditoria(reader.GetInt32(16),
                        reader.GetDateTime(15), reader.GetBoolean(17));

                }

                return contrato;
            }
            catch (Exception e)
            {
                ExceptionManager.Execute(e, "ERROR[Contrato.Obtener]:");
                return null;
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// Devuelve el IdContrato más alto, es decir, el id del último contrato creado. Utilizado para realizar las pruebas unitarias.
        /// </summary>
        /// <returns></returns>
        public static int maxIdContrato()
        {
            int max = -1;
            try
            {
                string consulta = "SELECT MAX(IdContrato) FROM Contrato";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    max = reader.GetInt32(0);
                }

                return max;
            }
            catch (Exception e)
            {
                ExceptionManager.Execute(e, "ERROR[Contrato.MaxId]:");
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
        ParcialManiana = 1, ParcialTarde = 2, Completa = 3, Practicas = 4
    }
}
