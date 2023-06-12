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

namespace GestionPersonal
{
    public class Empleado
    {
        private SqlConnection conexionSQL;
        private string cadenaConexion = ConfigurationManager.ConnectionStrings["GestionPersonal.Properties.Settings.masterConnectionString"].ConnectionString;

        public int IdEmpleado { get; set; }
        public string NombreE { get; set; }
        public string Apellido { get; set; }
        public string Usuario { get; set; }
        public string Contrasenia { get; set; }
        public string DNI { get; set; }
        public string NumSS { get; set; }
        public TipoEmpleado rol { get; set; }
        EstadoEmpleado EstadoE { get; set; }
        public string Tlf { get; set; }
        public string CorreoE { get; set; }
        public int IdDepartamento { get; set; }
        public Auditoria Auditoria { get; set; }

        public Empleado(string Usuario)//ESTO HAY QUE CAMBIARLO
        {
            DataTable dtEmpleado = obtenerEmpleado(Usuario);

            if(dtEmpleado.Rows.Count != 0)
            {
                this.IdEmpleado = Convert.ToInt32(dtEmpleado.Rows[0]["IdEmpleado"].ToString());
                this.NombreE = dtEmpleado.Rows[0]["NombreE"].ToString();
                this.Apellido = dtEmpleado.Rows[0]["Apellido"].ToString();
                this.Usuario = dtEmpleado.Rows[0]["Usuario"].ToString();
                this.Contrasenia = dtEmpleado.Rows[0]["Contrasenia"].ToString();
                this.rol = (TipoEmpleado)Convert.ToInt32(dtEmpleado.Rows[0]["Rol"].ToString());
                this.EstadoE = (EstadoEmpleado)Convert.ToInt32(dtEmpleado.Rows[0]["EstadoE"].ToString());
                this.DNI = dtEmpleado.Rows[0]["DNI"].ToString();
                this.NumSS = dtEmpleado.Rows[0]["NumSS"].ToString();
                this.Tlf = dtEmpleado.Rows[0]["Tlf"].ToString();
                this.CorreoE = dtEmpleado.Rows[0]["CorreoE"].ToString();
                this.IdDepartamento = Convert.ToInt32(dtEmpleado.Rows[0]["IdDepartamento"].ToString());
            }
        }

        /// <summary>
        /// Constructor para crear un empleado nuevo
        /// </summary>
        /// <param name="NombreE"></param>
        /// <param name="Apellido"></param>
        /// <param name="Usuario"></param>
        /// <param name="DNI"></param>
        /// <param name="NumSS"></param>
        /// <param name="Tlf"></param>
        /// <param name="CorreoE"></param>
        /// <param name="IdDepartamento"></param>
        public Empleado( string NombreE, string Apellido, string Usuario,
            string DNI, string NumSS, string Tlf, string CorreoE, int IdDepartamento, int IdModif)
        {
            this.NombreE = NombreE;
            this.Apellido = Apellido;
            this.Usuario = Usuario;
            this.Contrasenia = "DEFAULT????";
            this.rol = TipoEmpleado.Basico;//Defaul basico, será un admin quien lo tenga que cambiar
            this.EstadoE = EstadoEmpleado.Pendiente; //Ya que cuando se crea, no está autorizado. Se autoriza modificando el combobox
            this.DNI = DNI;
            this.NumSS = NumSS;
            this.Tlf = Tlf;
            this.CorreoE = CorreoE;
            this.IdDepartamento = IdDepartamento;
            this.Auditoria = new Auditoria(IdModif);
        }

        /// <summary>
        /// Constructor para guardar un empleado existente
        /// </summary>
        /// <param name="IdEmpleado"></param>
        /// <param name="NombreE"></param>
        /// <param name="Apellido"></param>
        /// <param name="Usuario"></param>
        /// <param name="rol"></param>
        /// <param name="EstadoE"></param>
        /// <param name="DNI"></param>
        /// <param name="NumSS"></param>
        /// <param name="Tlf"></param>
        /// <param name="CorreoE"></param>
        /// <param name="IdDepartamento"></param>
        public Empleado(int IdEmpleado, string NombreE, string Apellido, string Usuario, TipoEmpleado rol,
            EstadoEmpleado EstadoE, string DNI, string NumSS, string Tlf, string CorreoE, int IdDepartamento, int IdModif)
        {
            this.IdEmpleado = IdEmpleado;
            this.NombreE = NombreE;
            this.Apellido = Apellido;
            this.Usuario = Usuario;
            this.rol = rol;
            this.EstadoE = EstadoE; 
            this.DNI = DNI;
            this.NumSS = NumSS;
            this.Tlf = Tlf;
            this.CorreoE = CorreoE;
            this.IdDepartamento = IdDepartamento;
            this.Auditoria = new Auditoria(IdModif);
        }

        private DataTable obtenerEmpleado(string Usuario)//casi igual que el listado pero todavía no se me ocurre cómo hacer varias inserciones. creo que lo mejor será separar la clase
        {
            try
            {
                DataTable dtEmpleados = new DataTable();
                string consulta = "SELECT * FROM Empleado WHERE Usuario = @Usuario";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(consulta, conexionSQL);
                miAdaptadorSql.SelectCommand.Parameters.Add("@Usuario", SqlDbType.NVarChar);
                miAdaptadorSql.SelectCommand.Parameters["@Usuario"].Value = Usuario;

                using (miAdaptadorSql)
                {
                    miAdaptadorSql.Fill(dtEmpleados);
                }
                return dtEmpleados;
            }
            catch (Exception e)
            {
                ExceptionManager.Execute(e, "ERROR[Usuario.Obtener]:");
                return null; 
            }
        }

        public void insertEmpleado()
        {
            try
            {
                string consulta = "INSERT INTO Empleado (NombreE, Apellido, Usuario, Contrasenia, Rol, EstadoE, DNI, NumSS, Tlf, CorreoE, IdDepartamento, " + Auditoria.Insert_1 + ")";
                string valores = "VALUES (@NombreE, @Apellido, @Usuario, @Contrasenia, @Rol, @EstadoE, @DNI, @NumSS, @Tlf, @CorreoE, @IdDepartamento, " + Auditoria.Insert_2 + ")";
                consulta += valores;

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);
                comando = introducirParametros(comando);
                comando = introducirParametroContraseña(comando);
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

        public void deleteEmpleado(string UsuarioBorrado, int IdModif)
        {
            try
            {
                this.Auditoria = new Auditoria(IdModif, true);
                string consultaDelete = "UPDATE Empleado SET" + Auditoria.Update + " WHERE Usuario = @Usuario";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consultaDelete, conexionSQL);
                comando.Parameters.Add("@Usuario", SqlDbType.NVarChar);
                comando.Parameters["@Usuario"].Value = UsuarioBorrado;
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

            //!!LUEGO HAY QUE ELIMINARLO DEL DEPARTAMENTO, PROYECTOS, SUS CONTRATOS, AUSENCIAS 
        }
        public void updateEmpleado()
        {
            try
            {
                string consulta = "UPDATE Empleado SET NombreE = @NombreE, Apellido = @Apellido, Usuario = @Usuario," +
                " Rol = @Rol, EstadoE = @EstadoE, DNI = @DNI, NumSS = @NumSS, Tlf = @Tlf, CorreoE = @CorreoE, " +
                "IdDepartamento = @IdDepartamento, " + Auditoria.Update + "WHERE IdEmpleado = @IdEmpleado";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);
                comando = introducirParametros(comando);
                comando = this.Auditoria.introducirParametros(comando);
                comando = introducirParametroId(comando);

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
        /// Introduce todos los parámetros del empleado al comando indicado, a excepción de la Contrasenia 
        /// y el IdEmpleado.
        /// </summary>
        /// <param name="comando"></param>
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
            comando.Parameters.Add("@IdDepartamento", SqlDbType.Int);

            comando.Parameters["@NombreE"].Value = this.NombreE;
            comando.Parameters["@Apellido"].Value = this.Apellido;
            comando.Parameters["@Usuario"].Value = this.Usuario;

            //comando.Parameters["@password"].Value = Convert.ToBase64String(mySHA256.ComputeHash(Encoding.UTF8.GetBytes(password)));
            comando.Parameters["@Rol"].Value = this.rol.GetHashCode();
            comando.Parameters["@EstadoE"].Value = this.EstadoE.GetHashCode();
            comando.Parameters["@DNI"].Value = this.DNI;
            comando.Parameters["@NumSS"].Value = this.NumSS;
            comando.Parameters["@Tlf"].Value = this.Tlf;
            comando.Parameters["@CorreoE"].Value = this.CorreoE;
            comando.Parameters["@IdDepartamento"].Value = this.IdDepartamento;


            return comando;
        }

        /// <summary>
        /// Introduce el parámetro Contrasenia y su valor en el comando indicado.
        /// </summary>
        /// <param name="comando"></param>
        /// <returns></returns>
        private SqlCommand introducirParametroContraseña(SqlCommand comando)
        {
            comando.Parameters.Add("@Contrasenia", SqlDbType.NVarChar);
            comando.Parameters["@Contrasenia"].Value = this.Contrasenia;

            return comando;

        }

        /// <summary>
        /// Introduce el parámetro Contrasenia y su valor en el comando indicado.
        /// </summary>
        /// <param name="comando"></param>
        /// <returns></returns>
        private SqlCommand introducirParametroId(SqlCommand comando)
        {
            comando.Parameters.Add("@IdEmpleado", SqlDbType.Int);
            comando.Parameters["@IdEmpleado"].Value = this.IdEmpleado;

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
