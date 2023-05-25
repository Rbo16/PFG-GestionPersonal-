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

namespace GestionPersonal
{
    public class Empleado
    {
        private SqlConnection conexionSQL;
        private string cadenaConexion = ConfigurationManager.ConnectionStrings["GestionPersonal.Properties.Settings.masterConnectionString"].ConnectionString;

        public int IdEmpleado { get; set; }
        private string NombreE { get; set; }
        private string Apellido { get; set; }
        private string Usuario { get; set; }
        private string Contrasenia { get; set; }
        private string DNI { get; set; }
        private string NumSS { get; set; }
        public TipoEmpleado rol { get; set; }
        EstadoEmpleado EstadoE { get; set; }
        private string Tlf { get; set; }
        private string CorreoE { get; set; }
        private int IdDepartamento { get; set; }
        private Auditoria Auditoria { get; set; }

        InteraccionBBDD miBBDD = new InteraccionBBDD();
        public Empleado(string Usuario)
        {
            DataTable dtEmpleado = obtenerEmpleado(Usuario);

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

        //Falta meterle la contraseña aleatoria, creo que la meteré en el controlador
        public Empleado(string NombreE, string Apellido, string Usuario,
            string DNI, string NumSS, string Tlf, string CorreoE, int IdDepartamento)
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
        }

        public DataTable listadoEmpleados(string filtros)
        {
            DataTable dtEmpleados = new DataTable();
            string consulta = "SELECT * FROM Empleado ";
            consulta += filtros;

            dtEmpleados = miBBDD.consultaSelect(consulta);

            return dtEmpleados;

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

        public void insertEmpleado(Empleado nuevoEmpleado)
        {
            try
            {
                string consulta = "INSERT INTO Empleado (NombreE, Apellido, Usuario, Contrasenia, Rol, EstadoE, DNI, NumSS, Tlf, CorreoE, IdDepartamento, " + Auditoria.Insert_1 + ")";
                string valores = "VALUES (@NombreE, @Apellido, @Usuario, @Contrasenia, @Rol, @EstadoE, @DNI, @NumSS, @Tlf, @CorreoE, @IdDepartamento, " + Auditoria.Insert_2 + ")";
                consulta += valores;

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);
                comando = introducirParametros(comando,nuevoEmpleado);
                nuevoEmpleado.Auditoria = new Auditoria(this.IdEmpleado);//Se le pasa el Id del empleado que lo ha creado, es decir, el Usuario
                comando = nuevoEmpleado.Auditoria.introducirParametros(comando);

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

        public void deleteEmpleado(string UsuarioBorrado)
        {
            try
            {
                //Realmente solo ponemos el campo borrado a TRUE
                this.Auditoria = new Auditoria(this.IdEmpleado, true);
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
        public void updateEmpleado(DataRow empleadoModif)//IdModif
        {
            //IdModif e IdDepartamento quedan vacios, pero se genera la consulta correctamente
            string consulta = "UPDATE Empleado SET ";

            for (int i = 1; i < empleadoModif.Table.Columns.Count - 1; i++)// Empieza en 1 porque no actualizamos el Id. El último no para que no acabe en , 
            {
                if(empleadoModif[i].ToString() != "")
                {
                    consulta += empleadoModif.Table.Columns[i].ColumnName;
                    consulta += " = ";

                    if (empleadoModif.Table.Columns[i].DataType == typeof(String) 
                        || empleadoModif.Table.Columns[i].DataType == typeof(DateTime)) //Sii es string o fecha, le añadimos las comillas
                        consulta += "'";

                    consulta += empleadoModif[i].ToString();

                    if (empleadoModif.Table.Columns[i].DataType == typeof(String) 
                        || empleadoModif.Table.Columns[i].DataType == typeof(DateTime))
                        consulta += "'";

                    consulta += ", ";
                }
            }

            consulta += empleadoModif.Table.Columns["Borrado"].ColumnName;//Lo último en nuestro caso siempre será el borrado
            consulta += " = ";
            if (empleadoModif["Borrado"].ToString() == "False") consulta += "0";  
            else consulta += "1";

            consulta += " WHERE IdEmpleado = '" + empleadoModif["IdEmpleado"].ToString() + "'";

            miBBDD.ejecutarConsulta(consulta);
        }

        private SqlCommand introducirParametros(SqlCommand comando, Empleado nuevoEmpleado)
        {
            comando.Parameters.Add("@NombreE", SqlDbType.NVarChar);
            comando.Parameters.Add("@Apellido", SqlDbType.NVarChar);
            comando.Parameters.Add("@Usuario", SqlDbType.NVarChar);
            comando.Parameters.Add("@Contrasenia", SqlDbType.NVarChar);
            comando.Parameters.Add("@Rol", SqlDbType.Int);
            comando.Parameters.Add("@EstadoE", SqlDbType.Int);
            comando.Parameters.Add("@DNI", SqlDbType.NVarChar);
            comando.Parameters.Add("@NumSS", SqlDbType.NVarChar);
            comando.Parameters.Add("@Tlf", SqlDbType.NVarChar);
            comando.Parameters.Add("@CorreoE", SqlDbType.NVarChar);
            comando.Parameters.Add("@IdDepartamento", SqlDbType.Int);

            comando.Parameters["@NombreE"].Value = nuevoEmpleado.NombreE;
            comando.Parameters["@Apellido"].Value = nuevoEmpleado.Apellido;
            comando.Parameters["@Usuario"].Value = nuevoEmpleado.Usuario;
            comando.Parameters["@Contrasenia"].Value = nuevoEmpleado.Contrasenia;
            //comando.Parameters["@password"].Value = Convert.ToBase64String(mySHA256.ComputeHash(Encoding.UTF8.GetBytes(password)));
            comando.Parameters["@Rol"].Value = nuevoEmpleado.rol.GetHashCode();
            comando.Parameters["@EstadoE"].Value = nuevoEmpleado.EstadoE.GetHashCode();
            comando.Parameters["@DNI"].Value = nuevoEmpleado.DNI;
            comando.Parameters["@NumSS"].Value = nuevoEmpleado.NumSS;
            comando.Parameters["@Tlf"].Value = nuevoEmpleado.Tlf;
            comando.Parameters["@CorreoE"].Value = nuevoEmpleado.CorreoE;
            comando.Parameters["@IdDepartamento"].Value = nuevoEmpleado.IdDepartamento;


            return comando;
        }
    }

    public enum TipoEmpleado
    {
        Basico = 1, Gestor = 2, Administrador = 3
    }

    enum EstadoEmpleado
    {
        Autorizado = 1, Pendiente = 2, NoAutorizado = 3, Retirado = 4
    }

}
