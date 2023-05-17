using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GestionPersonal
{
    internal class Empleado
    {
        private int IdEmpleado { get; set; }
        private string NombreE { get; set; }
        private string Apellido { get; set; }
        private string Usuario { get; set; }
        private string Contrasenia { get; set; }
        private string DNI { get; set; }
        private string NumSS { get; set; }
        TipoEmpleado rol { get; set; }
        EstadoEmpleado EstadoE { get; set; }
        private string Tlf { get; set; }
        private string CorreoE { get; set; }
        private int IdDepartamento { get; set; }

        InteraccionBBDD miBBDD = new InteraccionBBDD();
        public Empleado()
        {
            this.IdEmpleado = -1;
            this.NombreE = string.Empty;
            this.Apellido = string.Empty;
            this.Usuario = string.Empty;
            this.Contrasenia = string.Empty;
            this.rol = TipoEmpleado.Basico;
            this.EstadoE = EstadoEmpleado.Autorizado;
            this.DNI = string.Empty;
            this.NumSS = string.Empty;
            this.Tlf = string.Empty;
            this.CorreoE = string.Empty;
            this.IdDepartamento = -1; //Porque no está en niguno
        }

        //Constructor de un empleado concreto. No recibe contraseña por temas de seguridad
        public Empleado(int IdEmpleado, string NombreE, string Apellido, string Usuario,
            string DNI, string NumSS, string Tlf, string CorreoE, int IdDepartamento)
        {
            this.IdEmpleado = IdEmpleado;
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
        public void insertEmpleado(string IdModif)
        {
            string consulta = "INSERT INTO Empleado (NombreE, Apellido, Usuario, Contrasenia, Rol, EstadoE, DNI, NumSS, Tlf, CorreoE, IdDepartamento, FechaUltModif, IdModif) ";
            string valores = "VALUES ('" + this.NombreE + "', '" +
                this.Apellido + "', '" +
                this.Usuario + "', '" +
                this.Contrasenia + "', '" +
                this.rol.GetHashCode() + "', '" +
                this.EstadoE.GetHashCode() + "', '" +
                this.DNI + "', '" +
                this.NumSS + "', '" +
                this.Tlf + "', '" +
                this.CorreoE + "', " +
                "NULL, '" +  //DE MOMENTO SERÁ NULL PORQUE NO HAY DEPAS
                DateTime.Now.ToString() +
                IdModif + "')"; 

            consulta += valores;

            miBBDD.ejecutarConsulta(consulta);
        }

        public void deleteEmpleado(string IdBorrado, string IdModif)
        {
            //Realmente solo ponemos el campo borrado a TRUE
            string consultaDelete = "UPDATE Empleado SET Borrado = 1, FechaUltModif = GETDATE(), IdModif = " + IdModif + " WHERE IdEmpleado = " + IdBorrado;
            miBBDD.ejecutarConsulta(consultaDelete);

            //AÑADIR TAMBIEN RESPONSABLE DEL CAMBIO
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
    }

    enum TipoEmpleado
    {
        Basico = 1, Gestor = 2, Administrador = 3
    }

    enum EstadoEmpleado
    {
        Autorizado = 1, Pendiente = 2, NoAutorizado = 3, Retirado = 4
    }

}
