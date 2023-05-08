using System;
using System.Collections.Generic;
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

        public Empleado(){

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
