using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
    /// Lógica de interacción para Ausencias.xaml
    /// </summary>
    public partial class Ausencias : Window
    {
        private readonly AusenciaControl controladorAusencia;
        private DataTable dtAusencias;

        public Ausencias(AusenciaControl controladorAusencias)
        {
            this.controladorAusencia = controladorAusencias;
            InitializeComponent();
            cargarRol();
            cargarDTG(controladorAusencia.Usuario.IdEmpleado);
        }

        /// <summary>
        /// Carga un listado de ausencias en función del valor IdSolicitante especificado. Si el valor es 0,
        /// carga un listado de las ausencias del sistema.
        /// </summary>
        /// <param name="IdSolicitante"></param>
        private void cargarDTG(int IdSolicitante)
        {
            dtAusencias = controladorAusencia.listarAusencias(IdSolicitante);
            dtgListaAus.ItemsSource = dtAusencias.DefaultView;
        }

        /// <summary>
        /// Carga los elementos en función del rol del Usuario
        /// </summary>
        private void cargarRol()
        {
            if (controladorAusencia.Usuario.rol != TipoEmpleado.Basico)
            {
                txbSolicitante.Visibility = Visibility.Visible;
                lblSolicitante.Visibility = Visibility.Visible;
                cmbEstadoAus.IsEnabled = true;
            }
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            controladorAusencia.volverMenu();
        }

        /// <summary>
        /// comprueba que las fechas tengan el formato correcto y, si lo tiene, 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSolicitar_Click(object sender, RoutedEventArgs e)
        {
            DateTime FechaInicioA = DateTime.MaxValue, FechaFinA = DateTime.MinValue;
            CultureInfo culturaEspañola = new CultureInfo("es-ES");

            if (DateTime.TryParseExact(txbfechaInic.Text, "d", culturaEspañola, DateTimeStyles.None, out FechaInicioA))
            {
                if (DateTime.TryParseExact(txbFechaFin.Text, "d", culturaEspañola, DateTimeStyles.None, out FechaFinA))
                {
                    if (controladorAusencia.crearAusencia(txbMotivoAus.Text, FechaInicioA, FechaFinA, txbDescripcion.Text,
                        txbJustificante.Text))
                        vaciarCampos();
                }
                else
                {
                    MessageBox.Show("Introduzca la fecha fin con formato dd/MM/yyyy.");
                }
            }
            else
            {
                MessageBox.Show("Introduzca la fecha inicio con formato dd/MM/yyyy.");
            }
        }

        /// <summary>
        /// Vacía el contenido de los elementos de la vista
        /// </summary>
        private void vaciarCampos()
        {
            txbMotivoAus.Text = string.Empty;
            txbfechaInic.Text = string.Empty;
            txbFechaFin.Text = string.Empty;
            txbDescripcion.Text = string.Empty;
            txbJustificante.Text = string.Empty;
            //habrá que cargar el dtg
        }
    }
}
