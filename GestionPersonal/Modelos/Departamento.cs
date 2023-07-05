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
        private SqlConnection conexionSQL;
        private string cadenaConexion = ConfigurationManager.ConnectionStrings["GestionPersonal.Properties.Settings.masterConnectionString"].ConnectionString;

        public int IdDepartamento { get; set; }
        public string NombreD { get; set; }
        public string DescripcionD { get; set; }
        public int IdJefeDep { get; set; }

        private Auditoria Auditoria { get; set; }

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
                ExceptionManager.Execute(ex, "ERROR[Departamento.Borrar]:");
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
        public bool comprobarJefe(int IdEmpleado)
        {
            bool esJefe = true;

            try
            {
                string consulta = "SELECT * FROM Departamento WHERE IdJefeDep = @IdJefeDep";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@IdJefeDep", SqlDbType.Int);
                comando.Parameters["@IdJefeDep"].Value = IdEmpleado;

                if (comando.ExecuteNonQuery() == -1)
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
    }
}
