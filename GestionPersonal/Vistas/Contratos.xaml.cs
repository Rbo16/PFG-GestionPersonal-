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
            txbIdEmpleado.Text = this.controladorContrato.Usuario.Apellido + ", " + this.controladorContrato.Usuario.NombreE;
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
        
        /// <summary>
        /// Carga los elementos del ComboBox TipoContrato
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTipoContrato_Loaded(object sender, RoutedEventArgs e)
        {
            //Lo hacemos manualmente para que le quede más claro al usuario (Para no poner ParcialManiana, ParcialTarde)
            List<string> nombresTipoContrato = new List<string>();

            nombresTipoContrato.Add("Parcial Mañana");
            nombresTipoContrato.Add("Parcial Tarde");
            nombresTipoContrato.Add("Completa");
            nombresTipoContrato.Add("Prácticas");

            cmbTipoContrato.ItemsSource = nombresTipoContrato;
        }

        private void cmbDuracion_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> lDuracion = new List<string>();

            lDuracion.Add("Meses");
            lDuracion.Add("Años");
            lDuracion.Add("Indefinido");

            cmbDuracion.ItemsSource = lDuracion;
        }

        private void cmbDuracion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cmbDuracion.SelectedIndex != -1)
            {
                if(cmbDuracion.SelectedItem.ToString() == "Indefinido")
                {
                    txbDuracion.Text = string.Empty;
                    txbDuracion.IsEnabled= false;
                }
                else
                    txbDuracion.IsEnabled= true;
            }
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            controladorContrato.volverMenu();
        }

        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {

            if (controladorContrato.crearContrato(txbHorasTrabajo.Text, txbHorasDescanso.Text, txbHoraEntrada.Text,
                txbHoraSalida.Text, txbSalario.Text, txbPuesto.Text, txbVacacionesMes.Text, txbDuracion.Text + cmbDuracion.Text,
                txbIdEmpleado.Text, cmbTipoContrato.Text))
                MessageBox.Show("Contrato creado con éxito."); //Hay que poner un txb vacío que cargue el IdEmpleado sobre el que se actua
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (dtgContratos.SelectedItem != null)
            {
                DialogResult dr = MessageBox.Show("¿Eliminar el contrato seleccionada?", "Eliminar Contrato",
                    MessageBoxButtons.YesNo);

                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    controladorContrato.eliminarContrato(dtContratos.Rows[dtgContratos.SelectedIndex]["IdContrato"].ToString());
                    cargarDTG(1);
                    
                    //Se cargará del empleado actual!!!!!!
                
                }
                else
                    return;

            }
            else
                System.Windows.MessageBox.Show("Seleccione un contrato de la tabla");
        }

        /// <summary>
        /// Carga las Horas de trabajo, de descanso, y las vacaciones por mes según el tipo de contrato
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTipoContrato_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cmbTipoContrato.SelectedIndex != -1)
            {
                if(cmbTipoContrato.SelectedItem.ToString() == "Parcial Mañana" || cmbTipoContrato.SelectedItem.ToString() == "Parcial Tarde")
                {
                    txbHorasTrabajo.Text = "80";
                    txbHorasDescanso.Text = "0,5";
                    txbVacacionesMes.Text = "2,5";
                }
                else if (cmbTipoContrato.SelectedItem.ToString() == "Completa")
                {
                    txbHorasTrabajo.Text = "160";
                    txbHorasDescanso.Text = "1,5";
                    txbVacacionesMes.Text = "2,5";
                }
                else if (cmbTipoContrato.SelectedItem.ToString() == "Prácticas")
                {
                    txbHorasTrabajo.Text = "100";
                    txbHorasDescanso.Text = "0,5";
                    txbVacacionesMes.Text = "0";
                }
            }
        }


    }
}
