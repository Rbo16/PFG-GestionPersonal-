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

        private void cargarDTG(string filtro)
        {

            dtEmpleados = controladorBusqueda.listaEmpleados(filtro);

            dtgEmpleados.ItemsSource = null;
            dtgEmpleados.ItemsSource = eliminarColumnas(dtEmpleados).DefaultView;
        }

        private void dtgEmpleados_Loaded(object sender, RoutedEventArgs e)
        {
            cargarDTG("Estado = 'Autorizado'");
        }

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

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            controladorBusqueda.volver();
        }
    }
}
