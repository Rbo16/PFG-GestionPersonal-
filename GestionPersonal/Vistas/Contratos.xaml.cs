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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GestionPersonal
{
    /// <summary>
    /// Lógica de interacción para Contratos.xaml
    /// </summary>
    public partial class Contratos : Window
    {
        private readonly ContratoControl controladorContrato;
        private DataTable dtContratos;
        private DataRow contratoActual;
        public Contratos(ContratoControl controladorContrato)
        {
            this.controladorContrato = controladorContrato;
            InitializeComponent();
            cargarDTG(this.controladorContrato.Usuario.IdEmpleado);
        }

        private void cargarDTG(int IdEmpleado)
        {
            dtContratos = controladorContrato.listarContratos(IdEmpleado);

            contratoActual = dtContratos.NewRow();

            DataTable dtShow = dtContratos.Copy();
            dtShow.Columns.Remove("IdContrato");
            dtShow.Columns.Remove("HorasTrabajo");
            dtShow.Columns.Remove("HorasDescanso");
            dtShow.Columns.Remove("HoraEntrada");
            dtShow.Columns.Remove("HoraSalida");
            dtShow.Columns.Remove("Salario");
            dtShow.Columns.Remove("VacacionesMes");
            dtShow.Columns.Remove("Duracion");
            dtShow.Columns.Remove("DocumentoPDF");
            dtShow.Columns.Remove("IdEmpleado");
            dtShow.Columns.Remove("FechaUltModif");
            dtShow.Columns.Remove("IdModif");
            //dtShow.Columns.Remove("Borrado");

            dtgContratos.ItemsSource = null;
            dtgContratos.ItemsSource = dtShow.DefaultView;

        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            controladorContrato.volverMenu();
        }

        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            if (controladorContrato.crearContrato(txbHTrabajo.Text, txbHDescanso.Text, txbEntrada.Text, txbSalida.Text,
                txbSalario.Text, txbPuesto.Text, txbVacaciones.Text, txbDuracion.Text, txbPoseedor.Text,
                cmbTipoContrato.SelectedValue.ToString()))
                MessageBox.Show("Contrato creado con éxito.");
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
