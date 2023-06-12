using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using GestionPersonal.Vistas;

namespace GestionPersonal.Controladores
{
    public class FiltroEmpleadoControl
    {
        EmpleadoControl controladorEmpleado;

        public FiltroEmpleadoControl(EmpleadoControl controladorEmpleado) 
        {
            this.controladorEmpleado= controladorEmpleado;
            FiltroEmpleado ventanaFiltroE = new FiltroEmpleado(this);
            ventanaFiltroE.Show();
        }

        public void asignarFiltro(string filtro)
        {
            controladorEmpleado.filtro = filtro;
        }
        
        public void volver()
        {
            controladorEmpleado.cerrarFiltro();
        }
    }
}
