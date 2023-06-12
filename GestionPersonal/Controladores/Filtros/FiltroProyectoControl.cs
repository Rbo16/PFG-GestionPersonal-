using GestionPersonal.Vistas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPersonal.Controladores.Filtros
{
    public class FiltroProyectoControl
    {
        ProyectoControl controladorProyecto;

        public FiltroProyectoControl(ProyectoControl controladoProyecto)
        {
            this.controladorProyecto = controladoProyecto;
            FiltroProyecto ventanaFiltroP = new FiltroProyecto(this);
            ventanaFiltroP.Show();
        }

        public void asignarFiltro(string filtro)
        {
            controladorProyecto.filtro = filtro;
        }

        public void volver()
        {
            controladorProyecto.cerrarFiltro();
        }
    }
}
