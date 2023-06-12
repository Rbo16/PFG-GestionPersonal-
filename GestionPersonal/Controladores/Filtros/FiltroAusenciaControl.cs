using GestionPersonal.Vistas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPersonal.Controladores.Filtros
{
    public class FiltroAusenciaControl
    {
        AusenciaControl controladorAusencia;

        public FiltroAusenciaControl(AusenciaControl controladorAusencia) 
        { 
            this.controladorAusencia = controladorAusencia;
            FiltroAusencia ventanaFiltroA = new FiltroAusencia(this);
            ventanaFiltroA.Show();
        }

        public void asignarFiltro(string filtro)
        {
            controladorAusencia.filtro = filtro;
        }

        public void volver()
        {
            controladorAusencia.cerrarFiltro();
        }
    }
}
