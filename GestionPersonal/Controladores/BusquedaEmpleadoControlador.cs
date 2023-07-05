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

        /// <summary>
        /// Llama a la clase Listar para obtener un DataTable con los empleados del sistema, lo asigna al DataTable
        /// principal del controlador y le quita la contraseña.
        /// </summary>
        private void cargarEmpleados()
        {
            dtEmpleados = Listar.listarEmpleados();
            dtEmpleados.Columns.Remove("Contrasenia");
        }

        /// <summary>
        /// Devuelve un DataTable que filtra el DataTable principal de empleados a partir de la clase Listar
        /// </summary>
        /// <param name="filtro">string con el filtro que se quiere aplicar</param>
        /// <returns></returns>
        public DataTable listaEmpleados(string filtro)
        {
            return Listar.filtrarTabla(dtEmpleados, filtro);
        }

        /// <summary>
        /// Invoca el método de la clase Controlador que cierra los filtros.
        /// </summary>
        public void volver()
        {
            controlador.cerrarFiltro();
        }

    }
}
