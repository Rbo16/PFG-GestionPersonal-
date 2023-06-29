using GestionPersonal.Utiles;
using GestionPersonal.Vistas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPersonal.Controladores
{
    public class BusquedaEmpleadoControlador
    {
        DataTable dtEmpleados;
        public string dniBusqueda;
        Controlador controlador;

        public BusquedaEmpleadoControlador(Controlador controlador)
        {
            this.controlador = controlador;
            cargarEmpleados();
            BusquedaEmpleado ventanaBusqueda = new BusquedaEmpleado(this);
            ventanaBusqueda.Show();
        }

        private void cargarEmpleados()
        {
            //Mostramos una tabla en la que no se muestren las contaseñas
            dtEmpleados = Listar.listarEmpleados();
            dtEmpleados.Columns.Remove("Contrasenia");
        }

        /// <summary>
        /// Devuelve un DataTable que filtra el DataTable principal de Empleados
        /// </summary>
        /// <param name="filtro">string con el filtro que se quiere aplicar</param>
        /// <returns></returns>
        public DataTable listaEmpleados(string filtro)
        {
            return Listar.filtrarTabla(dtEmpleados, filtro);
        }


        public void volver()
        {
            controlador.cerrarFiltro();
        }

    }
}
