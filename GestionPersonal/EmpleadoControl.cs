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
        DataTable dtEmpleadosCif = new DataTable();
        public EmpleadoControl() { }
        
        public DataTable listarEmpleados()
        {
            //Mostramos una tabla en la que no se muestren las contaseñas
            dtEmpleadosCif = Empleado.listadoEmpleados(string.Empty);
            dtEmpleadosCif.Columns.Remove("Contrasenia");

            return dtEmpleadosCif;
        }

        public Array devolverEnum(string NombreEnum)
        {
            Array array = Array.Empty<Array>();
            if (NombreEnum == "TipoEmpleado")
                array = Enum.GetValues(typeof(TipoEmpleado));
            if (NombreEnum == "EstadoEmpleado")
                array = Enum.GetValues(typeof(EstadoEmpleado));
            return array;
        }
    }
}
