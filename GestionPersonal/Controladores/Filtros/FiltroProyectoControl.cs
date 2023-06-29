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
        public ProyectoControl controladorProyecto;

        public FiltroProyectoControl(ProyectoControl controladoProyecto)
        {
            this.controladorProyecto = controladoProyecto;
            FiltroProyecto ventanaFiltroP = new FiltroProyecto(this);
            ventanaFiltroP.Show();
        }

        public void asignarFiltro(string filtro)
        {
            if (controladorProyecto.Usuario.rol == TipoEmpleado.Basico)
                filtro = $"IdEmpleado = {controladorProyecto.Usuario.IdEmpleado} AND " + filtro;

            controladorProyecto.filtro = filtro;
        }

        public void volver()
        {
            if (controladorProyecto.Usuario.rol == TipoEmpleado.Basico)
                controladorProyecto.filtro = $"IdEmpleado = {controladorProyecto.Usuario.IdEmpleado}";
            controladorProyecto.cerrarFiltro();
        }
    }
}
