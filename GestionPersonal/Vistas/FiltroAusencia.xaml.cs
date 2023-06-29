using GestionPersonal.Controladores.Filtros;
using System;
using System.Collections.Generic;
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

namespace GestionPersonal.Vistas
{
    /// <summary>
    /// Lógica de interacción para FiltroAusencia.xaml
    /// </summary>
    public partial class FiltroAusencia : Window
    {
        FiltroAusenciaControl controladorFiltroA;

        private string[] contenidoFiltro = new string[5];

        public FiltroAusencia(FiltroAusenciaControl controladorFiltroA)
        {
            this.controladorFiltroA = controladorFiltroA;
            InitializeComponent();

            cargarListas();

            comprobarRol();
        }

        private void comprobarRol()
        {
            if (this.controladorFiltroA.controladorAusencia.Usuario.rol == TipoEmpleado.Basico)
            {
                txbDNI.Text = this.controladorFiltroA.controladorAusencia.Usuario.DNI;
                txbDNI.IsEnabled = false;
                contenidoFiltro[0] = txbDNI.Text;
            }
        }

        private void cmbEstadoS_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> lEstadoS = new List<string>();

            lEstadoS.Add("Próxima");
            lEstadoS.Add("En curso");
            lEstadoS.Add("Finalizada");

            cmbEstadoS.ItemsSource = lEstadoS;
        }
        private void cmbEstadoA_Loaded(object sender, RoutedEventArgs e)
        {
            cmbEstadoA.ItemsSource = Enum.GetValues(typeof(EstadoAusencia));
        }

        private void cargarListas()
        {
            contenidoFiltro[0] = "";
            contenidoFiltro[1] = "";
            contenidoFiltro[2] = "";
            contenidoFiltro[3] = "";
            contenidoFiltro[4] = "";
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            string filtro = string.Empty;

            if (contenidoFiltro[0].Trim() != "")
                filtro += "DNI like '%" + contenidoFiltro[0] + "%' AND ";

            if (contenidoFiltro[1] != "")
            {
                string fechaActual = DateTime.Now.ToShortDateString();
                if (contenidoFiltro[1] == "En curso")
                {
                    filtro += $"FechaFinA > '{fechaActual}' AND FechaInicioA < '{fechaActual}' AND ";
                }
                else if (contenidoFiltro[1] == "Finalizada")
                {
                    filtro += $"FechaFinA < '{fechaActual}' AND ";
                }
                else if (contenidoFiltro[1] == "Próxima")
                {
                    filtro += $"FechaInicioA > '{fechaActual}' AND ";
                }
            }

            if (contenidoFiltro[2] != "")
                filtro += "EstadoA = " + contenidoFiltro[2] + " AND ";

            if (contenidoFiltro[3] != "")
                filtro += "FechaInicioA > '"+ contenidoFiltro[3] + "' AND ";

            if (contenidoFiltro[4] != "")
                filtro += "FechaInicioA < '" + contenidoFiltro[4] + "' AND ";

            if (filtro == string.Empty)
            {
                MessageBox.Show("Debe especificar al menos un campo");
            }
            else
            {
                filtro = filtro.Remove(filtro.Length - 4);
                controladorFiltroA.asignarFiltro(filtro);
                this.Close();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            controladorFiltroA.volver();
        }

        private void txbDNI_TextChanged(object sender, TextChangedEventArgs e)
        {
            contenidoFiltro[0] = txbDNI.Text;
        }

        private void cmbEstadoS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            contenidoFiltro[1] = (cmbEstadoS.SelectedIndex+1).ToString();
        }

        private void cmbEstadoA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            contenidoFiltro[2] = (cmbEstadoA.SelectedIndex+1).ToString();
        }

        private void dtpFechaDesde_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            contenidoFiltro[3] = dtpFechaDesde.SelectedDate.ToString();
        }

        private void dtpFechaHasta_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            contenidoFiltro[4] = dtpFechaHasta.SelectedDate.ToString();
        }


    }
}
