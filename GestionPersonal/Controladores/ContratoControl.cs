using GestionPersonal.Controladores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPersonal
{
    public class ContratoControl : Controlador
    {
        public ContratoControl(VentanaControlador ventanaControl) : base(ventanaControl)
        {
            ventanaActiva = new Contratos(this);
            ventanaActiva.Show();
        }
    }
}
