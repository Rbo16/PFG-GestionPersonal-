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
using System.Net;

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
                this.Auditoria = new Auditoria(IdModif);
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

        public void deleteProyecto(int IdModif)
        {
            try
            {
                string consulta = "UPDATE Proyecto SET " + Auditoria.Update +
                    "WHERE IdProyecto = @IdProyecto";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@IdProyecto");
                comando.Parameters["@IdProyecto"].Value = this.IdProyecto;

                this.Auditoria = new Auditoria(IdModif, true);
                comando = this.Auditoria.introducirParametros(comando);

                comando.ExecuteNonQuery();

                this.deleteParticipacion();
            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Proyecto.Borrar]:");
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        private void deleteParticipacion()
        {
            try 
            {
                string consulta = "UPDATE ParticipacionProyecto SET " + Auditoria.Update +
                        "WHERE IdProyecto = @IdProyecto";

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@IdProyecto");
                comando.Parameters["@IdProyecto"].Value = this.IdProyecto;

                comando = this.Auditoria.introducirParametros(comando);

                comando.ExecuteNonQuery();
        }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Participacion.Borrar]:");
            }
        }

        public void addEmpleado(string DNI, int IdModif)
        {
            try
            {
                string consulta = "INSERT INTO ParticipacionProyecto (IdEmpleado, IdProyecto," + Auditoria.Insert_1 + ")" +
                    "VALUES ((SELECT IdEmpleado From Empleado where DNI = @DNI), @IdProyecto," + Auditoria.Insert_2 + ")";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@IdProyecto", SqlDbType.Int);
                comando.Parameters["@IdProyecto"].Value = this.IdProyecto;
                comando.Parameters.Add("@DNI", SqlDbType.NVarChar);
                comando.Parameters["@DNI"].Value = DNI;

                this.Auditoria = new Auditoria(IdModif);
                comando = Auditoria.introducirParametros(comando);

                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Proyecto.AniadirEmpleado]:");
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        public void removeEmpleado(int IdEmpleado, int IdModif)
        {
            try
            {
                string consulta = "UPDATE ParticipacionProyecto SET " + Auditoria.Update + "WHERE IdProyecto = @IdProyecto " +
                    "AND IdEmpleado = @IdEmpleado";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@IdProyecto", SqlDbType.Int);
                comando.Parameters["@IdProyecto"].Value = this.IdProyecto;
                comando.Parameters.Add("@IdEmpleado", SqlDbType.Int);
                comando.Parameters["@IdEmpleado"].Value = IdEmpleado;

                this.Auditoria = new Auditoria(IdModif);
                comando = Auditoria.introducirParametros(comando);

                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Proyecto.EliminarEmpleado]:");
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        public bool Participa(string DNI)
        {
            bool existe = true;

            try
            {
                string consulta = "SELECT * FROM ParticipacionProyecto LEFT JOIN Empleado " +
                    "ON ParticipacionProyecto.IdEmpleado = Empleado.IdEmpleado WHERE Empleado.DNI = @DNI AND " +
                    "ParticipacionProyecto.IdProyecto = @IdProyecto";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@IdProyecto", SqlDbType.Int);
                comando.Parameters["@IdProyecto"].Value = this.IdProyecto;
                comando.Parameters.Add("@DNI", SqlDbType.NVarChar);
                comando.Parameters["@DNI"].Value = DNI;
                
                if(comando.ExecuteNonQuery() == -1)
                    existe = false;

                return existe;
            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Proyecto.Existe]:");
                return true;
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

