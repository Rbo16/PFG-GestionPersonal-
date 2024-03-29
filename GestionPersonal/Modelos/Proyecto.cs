﻿using GestionPersonal.Utiles;
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
        private static SqlConnection conexionSQL;
        private static string cadenaConexion = ConfigurationManager.ConnectionStrings["GestionPersonal.Properties.Settings.masterConnectionString"].ConnectionString;

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

        /// <summary>
        /// Hace el Insert del proyecto que invoca el método en la BBDD.
        /// </summary>
        /// <param name="IdModif">Id del empleado que crea el proyecto.</param>
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

        /// <summary>
        /// Realiza el Update del proyecto que invoca al método en la BBDD.
        /// </summary>
        /// <param name="IdModif">Id del empleado que ha realizado los cambios.</param>
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

                comando.Parameters.Add("@IdProyecto", SqlDbType.Int);
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

        /// <summary>
        /// Realiza el borrado lógico del proyecto que llama al método en la BBDD.
        /// </summary>
        /// <param name="IdModif">Id del empleado que realiza el borrado.</param>
        public void deleteProyecto(int IdModif)
        {
            try
            {
                string consulta = "UPDATE Proyecto SET FechaFinP = @FechaFinP," + Auditoria.Update +
                    "WHERE IdProyecto = @IdProyecto";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@IdProyecto", SqlDbType.Int);
                comando.Parameters["@IdProyecto"].Value = this.IdProyecto;
                this.FechaFinP = DateTime.Now.Date;
                comando.Parameters.Add("@FechaFinP", SqlDbType.DateTime);
                comando.Parameters["@FechaFinP"].Value = this.FechaFinP;

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

        /// <summary>
        /// Realiza el borrado lógico de todas las participaciones del proyecto que invoca al método en la BBDD.
        /// </summary>
        private void deleteParticipacion()
        {
            try 
            {
                string consulta = "UPDATE ParticipacionProyecto SET " + Auditoria.Update +
                        "WHERE IdProyecto = @IdProyecto";

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@IdProyecto",SqlDbType.Int);
                comando.Parameters["@IdProyecto"].Value = this.IdProyecto;

                comando = this.Auditoria.introducirParametros(comando);

                comando.ExecuteNonQuery();
        }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Participacion.Borrar]:");
            }
        }

        /// <summary>
        /// Hace el Insert del empleado cuyo DNI se indica en la tabla de ParticipacionProyecto con el proyecto que
        /// invoca al método en la BBDD.
        /// </summary>
        /// <param name="DNI">DNI del empleado que se desea añadir.</param>
        /// <param name="IdModif">Id del empleado que añade al empleado indicado en el proyecto.</param>
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

        /// <summary>
        /// Hace el borrado lógico de la ParticipacionProyecto con el empleado indicado y el proyecto que llama al
        /// método en la BBDD.
        /// </summary>
        /// <param name="IdEmpleado">Id del empleado cuya participación se desea eliminar.</param>
        /// <param name="IdModif">Id del empleado que elimina la participación.</param>
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

                this.Auditoria = new Auditoria(IdModif,true);
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

        /// <summary>
        /// Comprueba con la BBDD si el empleado indicado participa en el proyecto que invoca al método.
        /// </summary>
        /// <param name="DNI">DNI del empleado cuya participación se desea comprobar.</param>
        /// <returns>True si participa, false si no.</returns>
        public bool Participa(string DNI)
        {
            bool existe = true;

            try
            {
                string consulta = "SELECT * FROM ParticipacionProyecto LEFT JOIN Empleado " +
                    "ON ParticipacionProyecto.IdEmpleado = Empleado.IdEmpleado WHERE Empleado.DNI = @DNI AND " +
                    "ParticipacionProyecto.IdProyecto = @IdProyecto AND ParticipacionProyecto.Borrado = 0";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@IdProyecto", SqlDbType.Int);
                comando.Parameters["@IdProyecto"].Value = this.IdProyecto;
                comando.Parameters.Add("@DNI", SqlDbType.NVarChar);
                comando.Parameters["@DNI"].Value = DNI;
                
                if(!comando.ExecuteReader().HasRows)
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

        /// <summary>
        /// Devuelve el proyecto cuyo Id se ha indicado. Utilizado para realizar las pruebas unitarias.
        /// </summary>
        /// <param name="IdProyecto">Id del proyecto a obtener.</param>
        /// <returns></returns>
        public static Proyecto obtenerProyecto(int IdProyecto)
        {
            Proyecto proyecto = new Proyecto(IdProyecto);

            try
            {
                string consulta = "SELECT * FROM Proyecto WHERE IdProyecto = @IdProyecto";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@IdProyecto", SqlDbType.Int);
                comando.Parameters["@IdProyecto"].Value = IdProyecto;

                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    proyecto.NombreP = reader.GetString(1);
                    proyecto.Cliente = reader.GetString(2);
                    proyecto.FechaInicioP = reader.GetDateTime(3);
                    if(!reader.IsDBNull(4))
                        proyecto.FechaFinP = reader.GetDateTime(4);
                    proyecto.Tiempo = reader.GetString(5);
                    proyecto.Presupuesto = reader.GetInt32(6);
                    proyecto.Prioridad = (TipoPrioridad)reader.GetInt32(7);
                    proyecto.DescripcionP = reader.GetString(8);
                    proyecto.Auditoria =new Auditoria(reader.GetInt32(10),
                        reader.GetDateTime(9), reader.GetBoolean(11));
                }

                return proyecto;
            }
            catch (Exception e)
            {
                ExceptionManager.Execute(e, "ERROR[Proyecto.Obtener]:");
                return null;
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// Devuelve el IdProyecto más alto, es decir, el id del último empleado creado. Utilizado para realizar las pruebas unitarias.
        /// </summary>
        /// <returns></returns>
        public static int maxIdProyecto()
        {
            int max = -1;
            try
            {
                string consulta = "SELECT MAX(IdProyecto) FROM Proyecto";

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
                ExceptionManager.Execute(e, "ERROR[Proyecto.MaxId]:");
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

