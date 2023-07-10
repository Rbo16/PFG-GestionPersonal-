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
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Xml.Linq;
using System.Security.AccessControl;
using System.Management.Instrumentation;
using GestionPersonal.Vistas;

namespace GestionPersonal
{
    public class Empleado
    {
        private static SqlConnection conexionSQL;
        private static string cadenaConexion = ConfigurationManager.ConnectionStrings["GestionPersonal.Properties.Settings.masterConnectionString"].ConnectionString;

        public int IdEmpleado { get; set; }
        public string NombreE { get; set; }
        public string Apellido { get; set; }
        public string Usuario { get; set; }
        public string Contrasenia { get; set; }
        public string DNI { get; set; }
        public string NumSS { get; set; }
        public TipoEmpleado rol { get; set; }
        public EstadoEmpleado EstadoE { get; set; }
        public string Tlf { get; set; }
        public string CorreoE { get; set; }
        public int? IdDepartamento { get; set; }
        public Auditoria Auditoria { get; set; }

        public Empleado(int IdEmpleado)
        {
            this.IdEmpleado = IdEmpleado;
            this.NombreE = string.Empty;
            this.Apellido = string.Empty;
            this.Usuario = string.Empty;
            //Contraseña en el insert
            this.rol = TipoEmpleado.Basico;//Defaul basico, será un admin quien lo tenga que cambiar
            this.EstadoE = EstadoEmpleado.Pendiente; //Ya que cuando se crea, no está autorizado. Se autoriza modificando el combobox
            this.DNI = string.Empty;
            this.NumSS = string.Empty;
            this.Tlf = string.Empty;
            this.CorreoE = string.Empty;
            this.IdDepartamento = 0;
        }

        /// <summary>
        /// Comprueba con la BBDD si la contraseña del objeto empleado actual es correcta basándose en usuario del mismo.
        /// </summary>
        /// <returns>True si coincide, false si no.</returns>
        public bool iniciarSesion()
        {
            bool inicio = false;

            string contrasenia;
            int estado;

            try
            {
                string consulta = "SELECT Contrasenia, EstadoE FROM Empleado WHERE Usuario = @Usuario";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);
                comando.Parameters.Add("@Usuario", SqlDbType.NVarChar);
                comando.Parameters["@Usuario"].Value = this.Usuario;

                SqlDataReader reader= comando.ExecuteReader();
                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        contrasenia = reader.GetString(0);
                        estado = reader.GetInt32(1);
                        if(contrasenia == this.Contrasenia && estado == 1)
                        {
                            obtenerEmpleado(this.Usuario);
                            inicio = true;
                        }
                    }
                }

                return inicio;
            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Empleado.Login]:");
                return false;
            }

        }


        /// <summary>
        /// Hace el Insert del empleado que invoca al método en la BBDD.
        /// </summary>
        /// <param name="IdModif">Id del empleado que crea el nuevo empleado.</param>
        public void insertEmpleado(int IdModif)
        {
            try
            {
                string consulta = "INSERT INTO Empleado (NombreE, Apellido, Usuario, Contrasenia, Rol, EstadoE, DNI, " +
                    "NumSS, Tlf, CorreoE, " + Auditoria.Insert_1 + ") VALUES (@NombreE, @Apellido, " +
                    "@Usuario, @Contrasenia, @Rol, @EstadoE, @DNI, @NumSS, @Tlf, @CorreoE, "
                    + Auditoria.Insert_2 + ")";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando = introducirParametros(comando);
                comando.Parameters.Add("@Contrasenia", SqlDbType.NVarChar);
                comando.Parameters["@Contrasenia"].Value = this.Contrasenia;

                this.Auditoria = new Auditoria(IdModif);
                comando = Auditoria.introducirParametros(comando);

                comando.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                ExceptionManager.Execute(e, "ERROR[Usuario.Crear]:");
            }
            finally 
            {
                conexionSQL.Close(); 
            }
        }

        /// <summary>
        /// Realiza el borrado lógico del empleado que invoca al método.
        /// </summary>
        /// <param name="IdModif">Id del empleado que realiza el borrado.</param>
        public void deleteEmpleado(int IdModif)
        {
            try
            {
                
                string consultaDelete = "UPDATE Empleado SET" + Auditoria.Update + " WHERE IdEmpleado = @IdEmpleado";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consultaDelete, conexionSQL);

                comando.Parameters.Add("@IdEmpleado", SqlDbType.NVarChar);
                comando.Parameters["@IdEmpleado"].Value = this.IdEmpleado;

                this.Auditoria = new Auditoria(IdModif, true);
                this.Auditoria.introducirParametros(comando);

                comando.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                ExceptionManager.Execute(e, "ERROR[Usuario.Borrar]:");
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// Realiza el Update de la información del empleado que invoca al método en la BBDD.
        /// </summary>
        /// <param name="IdModif">Id de la persona que realiza los cambios.</param>
        public void updateEmpleado(int IdModif)
        {
            try
            {
                string consulta = "UPDATE Empleado SET NombreE = @NombreE, Apellido = @Apellido, Usuario = @Usuario," +
                " Rol = @Rol, DNI = @DNI, NumSS = @NumSS, Tlf = @Tlf, CorreoE = @CorreoE, " +
                " " + Auditoria.Update + "WHERE IdEmpleado = @IdEmpleado";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);
                comando = introducirParametros(comando);

                comando.Parameters.Add("@IdEmpleado", SqlDbType.Int);
                comando.Parameters["@IdEmpleado"].Value = this.IdEmpleado;

                this.Auditoria = new Auditoria(IdModif);
                comando = this.Auditoria.introducirParametros(comando);
                

                comando.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                ExceptionManager.Execute(e, " ERROR[Empleado.Actualizar]:");
            }
            finally 
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// realiza el Update relativo a la actualización del estado del empleado que invvoca al método en la BBDD.
        /// </summary>
        /// <param name="IdModif">Id de la persona que actualiza el estado</param>
        public void updateEstado(int IdModif)
        {
            try
            {
                string consulta = "UPDATE Empleado SET EstadoE = @EstadoE , " + Auditoria.Update +
                    " WHERE IdEmpleado = @IdEmpleado";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@EstadoE", SqlDbType.Int);
                comando.Parameters["@EstadoE"].Value = this.EstadoE.GetHashCode();

                comando.Parameters.Add("@IdEmpleado", SqlDbType.Int);
                comando.Parameters["@IdEmpleado"].Value = this.IdEmpleado;

                this.Auditoria = new Auditoria(IdModif);
                comando = Auditoria.introducirParametros(comando);

                comando.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                ExceptionManager.Execute(e, " ERROR[Empleado.Estado]:");
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// Comprueba si existe en el sistema el Correo del objeto Empleado que llama al método
        /// </summary>
        /// <returns>True si existe, false si no.</returns>
        public bool existeCorreo()
        {
            bool existe = false;
            try
            {
                string consulta = "SELECT * FROM Empleado WHERE CorreoE = @CorreoE ";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@CorreoE", SqlDbType.NVarChar);
                comando.Parameters["@CorreoE"].Value = this.CorreoE;

                SqlDataReader reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    existe= true;
                }
                return existe;
            }
            catch (Exception e)
            {
                ExceptionManager.Execute(e, " ERROR[Empleado.ExisteCorreo]:");
                return true;
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// Comprueba si existe en el sistema el DNI del objeto Empleado que llama al método
        /// </summary>
        /// <returns>True si existe, false si no.</returns>
        public bool existeDNI()
        {
            bool existe = false;
            try
            {
                string consulta = "SELECT * FROM Empleado WHERE DNI = @DNI ";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@DNI", SqlDbType.NVarChar);
                comando.Parameters["@DNI"].Value = this.DNI;

                SqlDataReader reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    existe = true;
                }
                return existe;
            }
            catch (Exception e)
            {
                ExceptionManager.Execute(e, " ERROR[Empleado.ExisteDNI]:");
                return true;
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// Comprueba si existe en el sistema el Usuario del objeto Empleado que llama al método
        /// </summary>
        /// <returns>True si existe, false si no.</returns>
        public bool existeUsuario()
        {
            bool existe = false;
            try
            {
                string consulta = "SELECT * FROM Empleado WHERE Usuario = @Usuario ";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@Usuario", SqlDbType.NVarChar);
                comando.Parameters["@Usuario"].Value = this.Usuario;

                SqlDataReader reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    existe = true;
                }
                return existe;
            }
            catch (Exception e)
            {
                ExceptionManager.Execute(e, " ERROR[Empleado.ExisteUsuario]:");
                return true;
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// Comprueba si existe en el sistema el Número de seguridad social del objeto Empleado que llama al método
        /// </summary>
        /// <returns>True si existe, false si no.</returns>
        public bool existeNumSS()
        {
            bool existe = false;
            try
            {
                string consulta = "SELECT * FROM Empleado WHERE NumSS = @NumSS ";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@NumSS", SqlDbType.NVarChar);
                comando.Parameters["@NumSS"].Value = this.NumSS;

                SqlDataReader reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    existe = true;
                }
                return existe;
            }
            catch (Exception e)
            {
                ExceptionManager.Execute(e, " ERROR[Empleado.ExisteNumSS]:");
                return true;
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// Realiza el Update relativo al cambio de contraseña del usuario que invoca al método
        /// </summary>
        public void updateContrasenia()
        {
            try
            {
                string consulta = "UPDATE Empleado SET Contrasenia = @Contrasenia WHERE CorreoE = @CorreoE ";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@Contrasenia", SqlDbType.NVarChar);
                comando.Parameters["@Contrasenia"].Value = this.Contrasenia;

                comando.Parameters.Add("@CorreoE", SqlDbType.NVarChar);
                comando.Parameters["@CorreoE"].Value = this.CorreoE;


                comando.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                ExceptionManager.Execute(e, " ERROR[Empleado.CambiarContrasenia]:");
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// Obtiene los datos de la BBDD del empleado cuyo usuario se ha proporcionado y los carga en un el propio objeto Empleado.
        /// </summary>
        /// <param name="Usuario">Usuario del cual se desea obtener los datos.</param>
        /// <returns></returns>
        public void obtenerEmpleado(string Usuario)
        {
            try
            {
                DataTable dtEmpleados = new DataTable();
                string consulta = "SELECT * FROM Empleado WHERE Usuario = @Usuario";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@Usuario", SqlDbType.NVarChar);
                comando.Parameters["@Usuario"].Value = Usuario;

                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    this.IdEmpleado = reader.GetInt32(0);
                    this.NombreE = reader.GetString(1);
                    this.Apellido = reader.GetString(2);
                    this.Usuario = reader.GetString(3);
                    this.Contrasenia = reader.GetString(4);
                    this.DNI = reader.GetString(5);
                    this.NumSS = reader.GetString(6);
                    this.rol = (TipoEmpleado)reader.GetInt32(7);
                    if (!reader.IsDBNull(8))
                        this.Tlf = reader.GetString(8);
                    else
                        this.Tlf = "";

                    if (!reader.IsDBNull(9))
                        this.CorreoE = reader.GetString(9);
                    else
                        this.CorreoE = "";

                    if (!reader.IsDBNull(10))
                        this.IdDepartamento = reader.GetInt32(10);
                    else
                        this.IdDepartamento = 0;

                    this.EstadoE = (EstadoEmpleado)reader.GetInt32(11);
                    this.Auditoria = new Auditoria(reader.GetInt32(13),
                                reader.GetDateTime(12), reader.GetBoolean(14));
                }
            }
            catch (Exception e)
            {
                ExceptionManager.Execute(e, "ERROR[Usuario.Obtener]:");
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// Devuelve el empleado cuyo Id se ha indicado. Utilizado para realizar las pruebas unitarias.
        /// </summary>
        /// <param name="IdEmpleado">Id del empleado a obtener.</param>
        /// <returns></returns>
        public static Empleado obtenerEmpleado(int IdEmpleado)
        {
            Empleado empleado = new Empleado(IdEmpleado);

            try
            {
                string consulta = "SELECT * FROM Empleado WHERE IdEmpleado = @IdEmpleado";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@IdEmpleado", SqlDbType.Int);
                comando.Parameters["@IdEmpleado"].Value = IdEmpleado;

                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    empleado.NombreE = reader.GetString(1);
                    empleado.Apellido = reader.GetString(2);
                    empleado.Usuario = reader.GetString(3);
                    empleado.Contrasenia = reader.GetString(4);
                    empleado.DNI = reader.GetString(5);
                    empleado.NumSS = reader.GetString(6);
                    empleado.rol = (TipoEmpleado)reader.GetInt32(7);
                    if(!reader.IsDBNull(8))
                        empleado.Tlf = reader.GetString(8);
                    if (!reader.IsDBNull(9))
                        empleado.CorreoE = reader.GetString(9);
                    if (!reader.IsDBNull(10))
                        empleado.IdDepartamento = reader.GetInt32(10);
                    empleado.EstadoE = (EstadoEmpleado)reader.GetInt32(11);
                    empleado.Auditoria = new Auditoria(reader.GetInt32(13),
                                reader.GetDateTime(12), reader.GetBoolean(14));
                }

                return empleado;
            }
            catch (Exception e)
            {
                ExceptionManager.Execute(e, "ERROR[Empleado.Obtener]:");
                return null;
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// Devuelve el IdEmpleado más alto, es decir, el id del último empleado cread0. Utilizado para realizar las pruebas unitarias.
        /// </summary>
        /// <returns></returns>
        public static int maxIdEmpleado()
        {
            int max = -1;
            try
            {
                string consulta = "SELECT MAX(IdEmpleado) FROM Empleado";

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
                ExceptionManager.Execute(e, "ERROR[Empleado.MaxId]:");
                return max;
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// Introduce todos los parámetros del empleado al comando indicado, a excepción de la Contrasenia 
        /// y el IdEmpleado.
        /// </summary>
        /// <param name="comando">Comando sql que se desea completar.</param>
        /// <returns></returns>
        private SqlCommand introducirParametros(SqlCommand comando)
        {
            comando.Parameters.Add("@NombreE", SqlDbType.NVarChar);
            comando.Parameters.Add("@Apellido", SqlDbType.NVarChar);
            comando.Parameters.Add("@Usuario", SqlDbType.NVarChar);
            comando.Parameters.Add("@Rol", SqlDbType.Int);
            comando.Parameters.Add("@EstadoE", SqlDbType.Int);
            comando.Parameters.Add("@DNI", SqlDbType.NVarChar);
            comando.Parameters.Add("@NumSS", SqlDbType.NVarChar);
            comando.Parameters.Add("@Tlf", SqlDbType.NVarChar);
            comando.Parameters.Add("@CorreoE", SqlDbType.NVarChar);

            comando.Parameters["@NombreE"].Value = this.NombreE;
            comando.Parameters["@Apellido"].Value = this.Apellido;
            comando.Parameters["@Usuario"].Value = this.Usuario;
            comando.Parameters["@Rol"].Value = this.rol.GetHashCode();
            comando.Parameters["@EstadoE"].Value = this.EstadoE.GetHashCode();
            comando.Parameters["@DNI"].Value = this.DNI;
            comando.Parameters["@NumSS"].Value = this.NumSS;
            comando.Parameters["@Tlf"].Value = this.Tlf;
            comando.Parameters["@CorreoE"].Value = this.CorreoE;

            return comando;
        }

    }

    public enum TipoEmpleado
    {
        Basico = 1, Gestor = 2, Administrador = 3
    }

    public enum EstadoEmpleado
    {
        Autorizado = 1, Pendiente = 2, NoAutorizado = 3, Retirado = 4
    }

}
