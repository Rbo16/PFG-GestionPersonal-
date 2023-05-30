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
            InitializeComponent();
            cargarRol();
            cargarDTG(controladorAusencia.Usuario.IdEmpleado);
        }

        /// <summary>
        /// Carga un listado de ausencias en función del valor IdSolicitante especificado. Si el valor es -1,
        /// carga un listado de las ausencias del sistema.
        /// </summary>
        /// <param name="IdSolicitante"></param>
        private void cargarDTG(int IdSolicitante)
        {
            dtAusencias = controladorAusencia.listarAusencias(IdSolicitante);

            ausenciaActual = dtAusencias.NewRow();

            DataTable dtShow = dtAusencias.Copy();
            dtShow.Columns.Remove("IdAusencia");
            dtShow.Columns.Remove("IdAutorizador");
            dtShow.Columns.Remove("IdSolicitante");
            dtShow.Columns.Remove("DescripcionAus");
            dtShow.Columns.Remove("JustificantePDF");
            dtShow.Columns.Remove("FechaUltModif");
            dtShow.Columns.Remove("IdModif");
            //dtShow.Columns.Remove("Borrado");

            dtgListaAus.ItemsSource = null;
            dtgListaAus.ItemsSource = dtShow.DefaultView;
            
        }

        /// <summary>
        /// Carga los elementos en función del rol del Usuario
        /// </summary>
        private void cargarRol()
        {
            if (controladorAusencia.Usuario.rol != TipoEmpleado.Basico)
            {
                txbIdSolicitante.Visibility = Visibility.Visible;
                lblSolicitante.Visibility = Visibility.Visible;
                cmbEstadoAus.IsEnabled = true;
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
            if (comprobarFechas())
            {
                if (controladorAusencia.crearAusencia(txbRazon.Text, txbFechaInicioA.Text, txbFechaFinA.Text, txbDescripcionAus.Text,
                        txbJustificantePDF.Text))
                {
                    vaciarCampos();
                    cargarDTG(controladorAusencia.Usuario.IdEmpleado);
                }
            }
        }
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (ausenciaActual != null)
            {
                if (hayCambiostxt)
                {
                    if (comprobarFechas())
                    {
                        controladorAusencia.modificarAusencia(ausenciaActual);

                        hayCambiostxt = false;
                    }
                }

                if (cambioEstado)
                {
                    string IdAusencia = ausenciaActual["IdAusencia"].ToString();
                    string EstadoA = ausenciaActual["EstadoA"].ToString();

                    controladorAusencia.gestionarAusencia(IdAusencia, EstadoA);

                    cambioEstado = false;
                }
                cargarDTG(-1);
                MessageBox.Show("Datos guardados correctamente");
            }


        }

        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (dtgListaAus.SelectedItem != null)
            {
                DialogResult dr = System.Windows.Forms.MessageBox.Show("¿Eliminar la ausencia seleccionada?", "Eliminar Ausencia",
                    MessageBoxButtons.YesNo);

                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    controladorAusencia.borrarAusencia(dtAusencias.Rows[dtgListaAus.SelectedIndex]["IdAusencia"].ToString());
                    cargarDTG(-1);
                }
                else
                    return;

            }
            else
                System.Windows.MessageBox.Show("Seleccione una ausencia de la tabla");
        }

        /// <summary>
        /// Vacía el contenido de los elementos de la vista
        /// </summary>
        private void vaciarCampos()
        {
            txbRazon.Text = string.Empty;
            txbFechaInicioA.Text = string.Empty;
            txbFechaFinA.Text = string.Empty;
            cmbEstadoAus.SelectedIndex = 0;
            txbIdSolicitante.Text = string.Empty;
            txbDescripcionAus.Text = string.Empty;
            txbJustificantePDF.Text = string.Empty;

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
                if (ausenciaActual != null)
                {
                    string columna = ((System.Windows.Controls.TextBox)sender).Name.Substring(3);
                    ausenciaActual[columna] = ((System.Windows.Controls.TextBox)sender).Text;
                }
                hayCambiostxt = true;
            }
        }


        private void cmbEstadoAus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!dblClic)
            {
                cambioEstado = true;
                ausenciaActual["EstadoA"] = cmbEstadoAus.SelectedIndex + 1;
            }
        }

        private void dtgListaAus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dtgListaAus.SelectedItem == null)
            {
                return;
            }
            else
            {
                dblClic = true;
                hayCambiostxt = false;
                cambioEstado = false;

                contAusencia = dtgListaAus.SelectedIndex;
                ausenciaActual = dtAusencias.Copy().Rows[contAusencia];
                cargarAusencia(contAusencia);
            }
        }

        private void cargarAusencia(int posicion)
        {
            CultureInfo culturaEspañola = new CultureInfo("es-ES");

            txbRazon.Text = ausenciaActual["Razon"].ToString();
            txbFechaInicioA.Text = ausenciaActual["FechaInicioA"].ToString().Split(' ')[0];//Separo la fecha de la hora
            txbFechaFinA.Text = ausenciaActual["FechaFinA"].ToString().Split(' ')[0];
            txbDescripcionAus.Text = ausenciaActual["DescripcionAus"].ToString();
            cmbEstadoAus.SelectedIndex = Convert.ToInt32(ausenciaActual["EstadoA"].ToString()) - 1;
            txbIdSolicitante.Text = ausenciaActual["IdSolicitante"].ToString();
            txbJustificantePDF.Text = ausenciaActual["JustificantePDF"].ToString();

            dblClic = false;
        }

        private bool comprobarFechas()
        {
            bool correctas = false;
            CultureInfo culturaEspañola = new CultureInfo("es-ES");

            if (DateTime.TryParseExact(txbFechaInicioA.Text, "d", culturaEspañola, DateTimeStyles.None, out DateTime FechaInicioA))
            {
                if (DateTime.TryParseExact(txbFechaFinA.Text, "d", culturaEspañola, DateTimeStyles.None, out DateTime FechaFinA))
                    correctas = true;
                else
                {
                    MessageBox.Show("Introduzca la fecha fin con formato dd/MM/yyyy.");
                }
            }
            else
            {
                MessageBox.Show("Introduzca la fecha incio con formato dd/MM/yyyy.");
            }

            return correctas;
        }


    }
}
