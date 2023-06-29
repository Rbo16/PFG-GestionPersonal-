using GestionPersonal.Vistas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPersonal.Controladores.Filtros
{
    public class FiltroContratoControl
    {
        public ContratoControl controladorContrato;

        public FiltroContratoControl(ContratoControl controladorContrato)
        {
            this.controladorContrato = controladorContrato;
            FiltroContrato ventanaFiltroC = new FiltroContrato(this);
            ventanaFiltroC.Show();
        }

        public void asignarFiltro(string filtro)
        {
            controladorContrato.filtro = filtro;
        }

        public void volver()
        {
            controladorContrato.cerrarFiltro();
        }

    }
}
