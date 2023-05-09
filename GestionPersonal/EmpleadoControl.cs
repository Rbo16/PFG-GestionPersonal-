using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPersonal
{
    internal class EmpleadoControl
    {
        Empleado Empleado = new Empleado();
        public EmpleadoControl() { }
        
        public DataTable listarEmpleados()
        {
            DataTable dtEmpleados = Empleado.listadoEmpleados(string.Empty);
            return dtEmpleados;
        }
    }
}
