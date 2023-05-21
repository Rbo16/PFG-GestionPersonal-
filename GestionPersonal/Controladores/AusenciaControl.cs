using GestionPersonal.Controladores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPersonal
{
    public class AusenciaControl : Controlador
    {
        public AusenciaControl(VentanaControlador ventanaControl) : base(ventanaControl)
        {
            ventanaActiva = new Ausencias(this);
            ventanaActiva.Show();
        }
    }
}
