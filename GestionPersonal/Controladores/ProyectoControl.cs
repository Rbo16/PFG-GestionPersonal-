using GestionPersonal.Controladores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPersonal
{
    public class ProyectoControl : Controlador
    {
        public ProyectoControl(VentanaControlador ventanaControl) : base(ventanaControl)
        {
            ventanaActiva = new Proyectos(this);
            ventanaActiva.Show();
        }
    }
}
