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
        Controlador controlador;

        public FiltroEmpleadoControl(Controlador controlador) 
        {
            this.controlador= controlador;
            FiltroEmpleado ventanaFiltroE = new FiltroEmpleado(this);
            ventanaFiltroE.Show();
        }

        public void asignarFiltro(string filtro)
        {
            controlador.filtro = filtro;
        }
        
        public void volver()
        {
            controlador.cerrarFiltro();
        }
    }
}
