using GestionPersonal.Vistas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
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
using MessageBox = System.Windows.MessageBox;

namespace GestionPersonal
{
    /// <summary>
    /// Lógica de interacción para Ausencias.xaml
    /// </summary>
    public partial class Ausencias : Window
    {
        private readonly AusenciaControl controladorAusencia;
        private DataTable dtAusencias;
        private int contAusencia;//para guardar el número de fila seleccionado
        private bool hayCambiostxt;
        private bool cambioEstado = false;
        private bool dblClic = false;
        private DataRow ausenciaActual;

        public Ausencias(AusenciaControl controladorAusencias)
        {
            this.controladorAusencia = controladorAusencias;

            this.controladorAusencia.filtro = $"IdSolicitante = {controladorAusencia.Usuario.IdEmpleado}";

            InitializeComponent();

            cargarRol();

            cargarDTG(controladorAusencia.filtro);

            ausenciaActual = dtAusencias.NewRow();
        }

        /// <summary>
        /// Carga el listado de ausencias en función del filtro que se le indique. Si el filtro = string.Empty,
        /// carga un listado de todas las ausencias del sistema
        /// </summary>
        /// <param name="filtro"></param>
        private void cargarDTG(string filtro)
        {
            if(filtro == string.Empty)
            {
                dtAusencias = controladorAusencia.listaAusencias();
            }
            else
            {
                dtAusencias = controladorAusencia.listaAusencias(filtro);
            }

            dtgListaAus.ItemsSource = null;
            dtgListaAus.ItemsSource = eliminarColumnas(dtAusencias).DefaultView;
            
        }

        private DataTable eliminarColumnas(DataTable dt)
        {
            DataTable dtShow = dt.Copy();

            dtShow.Columns.Remove("IdAusencia");
            dtShow.Columns.Remove("IdAutorizador");
            dtShow.Columns.Remove("Autorizador");
            dtShow.Columns.Remove("EstadoA");
            dtShow.Columns.Remove("IdSolicitante");
            dtShow.Columns.Remove("DescripcionAus");
            dtShow.Columns.Remove("JustificantePDF");
            dtShow.Columns.Remove("DNI");
            dtShow.Columns.Remove("FechaUltModif");
            dtShow.Columns.Remove("IdModif");
            //dtShow.Columns.Remove("Borrado");

            return dtShow;
        }

        /// <summary>
        /// Carga los elementos en función del rol del Usuario
        /// </summary>
        private void cargarRol()
        {
            if (controladorAusencia.Usuario.rol == TipoEmpleado.Basico)
            {
                txbIdSolicitante.Visibility = Visibility.Hidden;
                lblSolicitante.Visibility = Visibility.Hidden;
                cmbEstadoAus.IsEnabled = false;
                btnGuardar.Visibility = Visibility.Hidden;
                btnBorrar.Visibility = Visibility.Hidden;
            }
        }

        private void cmbEstadoAus_Loaded(object sender, RoutedEventArgs e)
        {
            cmbEstadoAus.ItemsSource = controladorAusencia.devolverEstadosA();
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
            if (dtgListaAus.SelectedItem == null)
            {
                if (controladorAusencia.crearAusencia(txbRazon.Text, dtpFechaInicioA.SelectedDate, dtpFechaFinA.SelectedDate,
                    txbDescripcionAus.Text, txbJustificantePDF.Text))
                {
                    vaciarCampos();
                    cargarDTG(controladorAusencia.filtro);
                }
            }
        }
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (ausenciaActual["IdAusencia"].ToString() != string.Empty)
            {
                if (hayCambiostxt)
                {
                        controladorAusencia.modificarAusencia(ausenciaActual);
                        
                        hayCambiostxt = false;
                }

                if (cambioEstado)
                {
                    string IdAusencia = ausenciaActual["IdAusencia"].ToString();
                    string EstadoA = ausenciaActual["EstadoA"].ToString();

                    controladorAusencia.gestionarAusencia(IdAusencia, EstadoA);

                    cambioEstado = false;
                }
                cargarDTG(string.Empty);
                ausenciaActual = dtAusencias.Copy().Rows[contAusencia];
                MessageBox.Show("Datos guardados correctamente");
            }


        }

        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (dtgListaAus.SelectedItem != null)
            {
                DialogResult dr = System.Windows.Forms.MessageBox.Show("¿Eliminar la AUSENCIA seleccionada?", "Eliminar Ausencia",
                    MessageBoxButtons.YesNo);

                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    controladorAusencia.borrarAusencia(dtAusencias.Rows[dtgListaAus.SelectedIndex]["IdAusencia"].ToString());
                    cargarDTG(string.Empty);
                    vaciarCampos();
                }
                else
                    return;

            }
            else
                System.Windows.MessageBox.Show("Seleccione una ausencia de la tabla");
        }


        /// <summary>
        /// Guarda los cambios del Textbox. El atributo Name del elemento cambiado ha de tener sus 3 primeras letras
        /// descartables y lo demás ha de coincidir con el nombre del atributo en el datatable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cambioAusenciaTxb(object sender, TextChangedEventArgs e)
        {
            if (!dblClic)//Esto es para que al cargar los Txb después del dtg dobleclick, no haga esto.
            {
                if (ausenciaActual["IdAusencia"].ToString() != string.Empty)
                {
                    string columna = ((System.Windows.Controls.TextBox)sender).Name.Substring(3);
                    ausenciaActual[columna] = ((System.Windows.Controls.TextBox)sender).Text;
                    hayCambiostxt = true;
                }
            }
        }


        private void cmbEstadoAus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!dblClic)
            {
                if (ausenciaActual["IdAusencia"].ToString() != string.Empty)
                {
                    cambioEstado = true;
                    ausenciaActual["EstadoA"] = cmbEstadoAus.SelectedIndex + 1;
                }
            }
        }


        private void dtpFecha_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!dblClic)
            {
                if (ausenciaActual["IdAusencia"].ToString() != string.Empty)
                {
                    string columna = ((DatePicker)sender).Name.Substring(3);
                    ausenciaActual[columna] = ((DatePicker)sender).SelectedDate.ToString();
                    hayCambiostxt = true;
                }
            }
        }


        private void dtgListaAus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dtgListaAus.SelectedItem != null)
            {
                hayCambiostxt = false;
                cambioEstado = false;

                contAusencia = dtgListaAus.SelectedIndex;
                ausenciaActual = dtAusencias.Copy().Rows[contAusencia];
                cargarAusencia();
            }
        }

        private void cargarAusencia()
        {
            dblClic = true;
            CultureInfo culturaEspañola = new CultureInfo("es-ES");

            txbRazon.Text = ausenciaActual["Razon"].ToString();
            dtpFechaInicioA.Text = ausenciaActual["FechaInicioA"].ToString().Split(' ')[0];//Separo la fecha de la hora
            dtpFechaFinA.Text = ausenciaActual["FechaFinA"].ToString().Split(' ')[0];
            txbDescripcionAus.Text = ausenciaActual["DescripcionAus"].ToString();
            cmbEstadoAus.SelectedIndex = Convert.ToInt32(ausenciaActual["EstadoA"].ToString()) - 1;
            txbIdSolicitante.Text = ausenciaActual["Solicitante"].ToString();
            txbIdAutorizador.Text = ausenciaActual["Autorizador"].ToString();
            txbJustificantePDF.Text = ausenciaActual["JustificantePDF"].ToString();

            dblClic = false;
        }


        /// <summary>
        /// Vacía el contenido de los elementos de la vista
        /// </summary>
        private void vaciarCampos()
        {
            dblClic = true;
            ausenciaActual = dtAusencias.NewRow();

            txbRazon.Text = string.Empty;
            dtpFechaInicioA.Text = string.Empty;
            dtpFechaFinA.Text = string.Empty;
            cmbEstadoAus.SelectedIndex = -1;
            txbIdSolicitante.Text = string.Empty;
            txbDescripcionAus.Text = string.Empty;
            txbJustificantePDF.Text = string.Empty;
            txbIdAutorizador.Text = string.Empty;

            dtgListaAus.SelectedItem = null;

            hayCambiostxt = false;
            cambioEstado = false;

            dblClic = false;
        }

        private void btnFiltrarAus_Click(object sender, RoutedEventArgs e)
        {
            controladorAusencia.prepararFiltro();
        }

        private void Window_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsEnabled)
            {
                cargarDTG(controladorAusencia.filtro);
                vaciarCampos();
            }
        }

        private void btnVacio_Click(object sender, RoutedEventArgs e)
        {
            vaciarCampos();
            cargarDTG(string.Empty);
        }
    }
}
