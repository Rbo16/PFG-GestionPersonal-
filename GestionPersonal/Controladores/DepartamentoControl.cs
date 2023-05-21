using GestionPersonal.Controladores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPersonal
{
    public  class DepartamentoControl : Controlador
    {
        public DepartamentoControl(VentanaControlador ventanaControl) : base(ventanaControl) 
        {
            ventanaActiva = new Departamentos(this);
            ventanaActiva.Show();
        }
    }
}
