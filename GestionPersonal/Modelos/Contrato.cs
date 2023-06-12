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
        public float HorasTrabajo { get; set; }
        public float HorasDescanso { get; set; }
        public TimeSpan HoraEntrada { get; set; }
        public TimeSpan HoraSalida { get; set; }
        public float Salario { get; set; }
        public string Puesto { get; set; }
        public float VacacionesMes { get; set; }
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

        public void deleteContrato(int IdModif)
        {
            try
            {
                string consulta = "UPDATE Contrato SET " + Auditoria.Update +
                    "WHERE IdContrato = @IdContrato";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);
                comando.Parameters.Add("@IdContrato");
                comando.Parameters["@IdEmpleado"].Value = this.IdContrato;
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
