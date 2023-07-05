using GestionPersonal.Controladores;
using GestionPersonal.Utiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MessageBox = System.Windows.Forms.MessageBox;

namespace GestionPersonal.Vistas
{
    /// <summary>
    /// Lógica de interacción para BusquedaEmpleado.xaml
    /// </summary>
    public partial class BusquedaEmpleado : Window
    {
        DataTable dtEmpleados;
        BusquedaEmpleadoControlador controladorBusqueda;

        public BusquedaEmpleado(BusquedaEmpleadoControlador controladorBusqueda)
        {
            this.controladorBusqueda = controladorBusqueda;

            InitializeComponent();
        }

        /// <summary>
        /// Llama al controlador para obtener una lista de empleados filtrada.
        /// </summary>
        /// <param name="filtro"></param>
        private void cargarDTG(string filtro)
        {

            dtEmpleados = controladorBusqueda.listaEmpleados(filtro);

            dtgEmpleados.ItemsSource = null;
            dtgEmpleados.ItemsSource = eliminarColumnas(dtEmpleados).DefaultView;
        }

        /// <summary>
        /// Carga el DataGrid con los empleados cuyo estado sea 'Autorizado'.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtgEmpleados_Loaded(object sender, RoutedEventArgs e)
        {
            cargarDTG("Estado = 'Autorizado'");
        }

        /// <summary>
        /// Devuelve un DataTable compuesto solamente por las columnas relevantes para el usuario.
        /// </summary>
        /// <param name="dt">DataTable de empleados que cuyas columnas se quieren reducir.</param>
        /// <returns></returns>
        private DataTable eliminarColumnas(DataTable dt)
        {
            DataTable dtShow = dt.Copy();

            dtShow.Columns.Remove("IdEmpleado");
            dtShow.Columns.Remove("NumSS");
            dtShow.Columns.Remove("CorreoE");
            dtShow.Columns.Remove("Rol");
            dtShow.Columns.Remove("EstadoE");
            dtShow.Columns.Remove("IdDepartamento");
            dtShow.Columns.Remove("FechaUltModif");
            dtShow.Columns.Remove("Borrado");
            dtShow.Columns.Remove("IdModif");
            dtShow.Columns.Remove("NombreD");
            dtShow.Columns.Remove("Estado");

            return dtShow;
        }

        /// <summary>
        /// cambia el valor del filtro cada vez que el texto de un Textbox es cambiado y carga el DataGrid en base a
        /// ese filtro.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filtro = string.Empty;

            if (txbDNI.Text.Trim() != "")
                filtro += $"DNI like '%{txbDNI.Text}%' AND ";
            if (txbNombreE.Text.Trim() != "")
                filtro += $"NombreE like '%{txbNombreE.Text}%' AND ";
            if (txbApellido.Text.Trim() != "")
                filtro += $"Apellido like '%{txbApellido.Text}%' AND ";

            if(filtro != string.Empty)
            {
                filtro += "Estado = 'Autorizado'";
                cargarDTG(filtro);
            }
            else
            {
                cargarDTG("Estado = 'Autorizado'");         
            }
        }

        /// <summary>
        /// Cierra la ventana de búsqueda.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Si hay un empleado seleccionado, guarda su DNI en el controlador que ha abierto la ventana.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            if(dtgEmpleados.SelectedItem != null)
            {
                controladorBusqueda.dniBusqueda = dtEmpleados.Rows[dtgEmpleados.SelectedIndex]["DNI"].ToString();
                this.Close();
            }
            else
            {
                MessageBox.Show("Debe seleccionar un empleado.");
            }
        }

        /// <summary>
        /// LLama al método de vuelta del controlador.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            controladorBusqueda.volver();
        }
    }
}
