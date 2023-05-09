using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Empleado(){
            this.IdEmpleado = -1;
            this.NombreE= string.Empty;
            this.Apellido= string.Empty;
            this.Usuario= string.Empty;
            this.Contrasenia= string.Empty;
            this.rol = TipoEmpleado.Basico;
            this.EstadoE = EstadoEmpleado.Autorizado;
            this.DNI= string.Empty;
            this.NumSS= string.Empty;
            this.Tlf= string.Empty;
            this.CorreoE= string.Empty;
            this.IdDepartamento = -1; //Porque no está en niguno
            }

        //Constructor de un empleado concreto. No recibe contraseña por temas de seguridad
        public Empleado(int IdEmpleado, string NombreE,string Apellido, string Usuario, TipoEmpleado Rol, EstadoEmpleado
            EstadoE, string DNI, string NumSS, string Tlf, string CorreoE, int IdDepartamento)
        {
            this.IdEmpleado = IdEmpleado;
            this.NombreE = NombreE;
            this.Apellido = Apellido;
            this.Usuario = Usuario;
            this.Contrasenia = "DEFAULT????";
            this.rol = Rol;
            this.EstadoE = EstadoE;
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
    }

    enum TipoEmpleado
    {
        Basico, Gestor, Administrador
    }

    enum EstadoEmpleado
    {
        NoAutorizado, Pendiente, Autorizado, Retirado
    }
}
