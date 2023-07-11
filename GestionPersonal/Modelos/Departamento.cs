using GestionPersonal.Modelos;
using GestionPersonal.Utiles;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GestionPersonal
{
    public class Departamento
    {
        private static SqlConnection conexionSQL;
        private static string cadenaConexion = ConfigurationManager.ConnectionStrings["GestionPersonal.Properties.Settings.masterConnectionString"].ConnectionString;

        public int IdDepartamento { get; set; }
        public string NombreD { get; set; }
        public string DescripcionD { get; set; }
        public int IdJefeDep { get; set; }

        public Auditoria Auditoria { get; set; }

        public Departamento()
        {
            IdDepartamento = 0;
            NombreD = string.Empty;
            DescripcionD = string.Empty;
            IdJefeDep = 0;
        }

        /// <summary>
        /// Hace el insert del departamento que invoca al método en la BBDD.
        /// </summary>
        /// <param name="IdModif">Id del empleado que crea el departamento.</param>
        public void insertarDepartamento(int IdModif)
        {
            try
            {
                string consulta = "INSERT INTO Departamento (NombreD, DescripcionD, " + Auditoria.Insert_1 +
                                  ") VALUES (@NombreD, @DescripcionD, " + Auditoria.Insert_2 + ")";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@NombreD", SqlDbType.NVarChar);
                comando.Parameters.Add("@DescripcionD", SqlDbType.NVarChar);

                comando.Parameters["@NombreD"].Value = this.NombreD;
                comando.Parameters["@DescripcionD"].Value = this.DescripcionD;

                this.Auditoria = new Auditoria(IdModif);
                comando = Auditoria.introducirParametros(comando);

                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Departamento.Crear]:");
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// Realiza el Update del departamento que invoca al método en la BBDD.
        /// </summary>
        /// <param name="IdModif">Id del empleado que ha relizado la modificación.</param>
        public void updateDepartamento(int IdModif)
        {
            try
            {
                string consulta = "UPDATE Departamento SET NombreD = @NombreD, DescripcionD = @DescripcionD, " 
                    + Auditoria.Update + " WHERE IdDepartamento = @IdDepartamento";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@NombreD", SqlDbType.NVarChar);
                comando.Parameters.Add("@DescripcionD", SqlDbType.NVarChar);
                comando.Parameters.Add("@IdDepartamento", SqlDbType.Int);

                comando.Parameters["@NombreD"].Value = this.NombreD;
                comando.Parameters["@DescripcionD"].Value = this.DescripcionD;
                comando.Parameters["@IdDepartamento"].Value = this.IdDepartamento;

                this.Auditoria = new Auditoria(IdModif);
                comando = Auditoria.introducirParametros(comando);

                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Departamento.Modificar]:");
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// Realiza el borrado lógico del departamento que invoca al método en la BBDD.
        /// </summary>
        /// <param name="IdModif">Id del empleado que ha relizado el borrado.</param>
        public void deleteDepartamento(int IdModif)
        {
            try
            {
                string consulta = "UPDATE Departamento SET " + Auditoria.Update + "WHERE IdDepartamento = @IdDepartamento";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@IdDepartamento", SqlDbType.Int);
                comando.Parameters["@IdDepartamento"].Value = this.IdDepartamento;

                this.Auditoria = new Auditoria(IdModif, true);
                comando = Auditoria.introducirParametros(comando);

                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Departamento.Borrar]:");
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// Realiza el update relativo a la adición de un empleado al departamento que invoca el método en la BBDD.
        /// </summary>
        /// <param name="DNI">DNI del nuevo empleado.</param>
        /// <param name="IdModif">Id del empleado que ha añadido al empleado.</param>
        public void addEmpleado(string DNI, int IdModif)
        {
            try
            {
                string consulta = "UPDATE Empleado SET IdDepartamento = @IdDepartamento," + Auditoria.Update + "WHERE DNI = @DNI";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@IdDepartamento", SqlDbType.Int);
                comando.Parameters["@IdDepartamento"].Value = this.IdDepartamento;
                comando.Parameters.Add("@DNI", SqlDbType.NVarChar);
                comando.Parameters["@DNI"].Value = DNI;

                this.Auditoria = new Auditoria(IdModif);
                comando = Auditoria.introducirParametros(comando);

                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Departamento.Aniadir]:");
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// Realiza el Update relativo a la asignación de un nuevo jefe del departamento que invoca al método en
        /// la BBDD si este no es jefe de otro departamento.
        /// </summary>
        /// <param name="IdEmpleado">Id del nuevo jefe de departamento.</param>
        /// <param name="IdModif">Id del empleado que ha realizado la asignación de un nuevo jefe.</param>
        public void asignarJefe(int IdEmpleado, int IdModif)
        {
            try
            {
                string consulta = "UPDATE Departamento SET IdJefeDep = @IdJefeDep," + Auditoria.Update +
                    "WHERE IdDepartamento = @IdDepartamento";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@IdJefeDep", SqlDbType.Int);
                comando.Parameters["@IdJefeDep"].Value = IdEmpleado;
                comando.Parameters.Add("@IdDepartamento", SqlDbType.Int);
                comando.Parameters["@IdDepartamento"].Value = this.IdDepartamento;

                this.Auditoria = new Auditoria(IdModif);
                comando = Auditoria.introducirParametros(comando);

                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Departamento.AsignarJefe]:");
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// Comprueba con la BBDD si el empleado indicado es jefe de algún departamento.
        /// </summary>
        /// <param name="IdEmpleado">Id del empleado a comprobar.</param>
        /// <returns>True si es jefe, False si no lo es.</returns>
        public static bool comprobarJefe(int IdEmpleado)
        {
            bool esJefe = true;

            try
            {
                string consulta = "SELECT * FROM Departamento WHERE IdJefeDep = @IdJefeDep " +
                    "AND Borrado = 0";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@IdJefeDep", SqlDbType.Int);
                comando.Parameters["@IdJefeDep"].Value = IdEmpleado;

                if (!comando.ExecuteReader().HasRows)
                    esJefe = false;

                return esJefe;
            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Departamento.ComprobarJefe]:");
                return true;
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// Devuelve el departamento cuyo Id se ha indicado. Utilizado para realizar las pruebas unitarias.
        /// </summary>
        /// <param name="IdDepartamento">Id del departamento a obtener.</param>
        /// <returns></returns>
        public static Departamento obtenerDepartamento(int IdDepartamento)
        {
            Departamento departamento= new Departamento() { IdDepartamento=IdDepartamento};

            try
            {
                string consulta = "SELECT * FROM Departamento WHERE IdDepartamento = @IdDepartamento";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@IdDepartamento", SqlDbType.Int);
                comando.Parameters["@IdDepartamento"].Value = IdDepartamento;

                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    if(!reader.IsDBNull(1))
                        departamento.IdJefeDep = reader.GetInt32(1);
                    departamento.NombreD = reader.GetString(2);
                    departamento.DescripcionD = reader.GetString(3);
                    departamento.Auditoria = new Auditoria(reader.GetInt32(5),
                                reader.GetDateTime(4), reader.GetBoolean(6));
                }

                return departamento;
            }
            catch (Exception e)
            {
                ExceptionManager.Execute(e, "ERROR[Departamento.Obtener]:");
                return null;
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// Devuelve el IdDepartamento más alto, es decir, el id del último empleado cread0. Utilizado para realizar las pruebas unitarias.
        /// </summary>
        /// <returns></returns>
        public static int maxIdDepartamento()
        {
            int max = -1;
            try
            {
                string consulta = "SELECT MAX(IdDepartamento) FROM Departamento";

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
                ExceptionManager.Execute(e, "ERROR[Departamento.MaxId]:");
                return max;
            }
            finally
            {
                conexionSQL.Close();
            }
        }
    }
}
