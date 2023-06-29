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
using static System.Net.Mime.MediaTypeNames;
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
        private bool dblClic;
        private bool hayCambios;
        int contContrato;

        public Contratos(ContratoControl controladorContrato)
        {
            this.controladorContrato = controladorContrato;

            this.controladorContrato.filtro = $"IdEmpleado = {this.controladorContrato.Usuario.IdEmpleado}";

            InitializeComponent();

            cargarRol();

            txbNombreE.Text = this.controladorContrato.Usuario.Apellido + ", " + this.controladorContrato.Usuario.NombreE;
            txbIdEmpleado.Text = this.controladorContrato.Usuario.IdEmpleado.ToString();

            cargarDTG(this.controladorContrato.filtro);
            contratoActual = dtContratos.NewRow();

        }

        /// <summary>
        /// Carga el listado de ausencias en función del filtro que se le indique. Si el filtro = string.Empty,
        /// carga un listado de todas las ausencias del sistema
        /// </summary>
        /// <param name="filtro"></param>
        private void cargarDTG(string filtro)
        {
            if (filtro == string.Empty)
            {
                dtContratos = controladorContrato.listaContratos();
            }
            else
            {
                dtContratos = controladorContrato.listaContratos(filtro);
            }

            dtgContratos.ItemsSource = null;
            dtgContratos.ItemsSource = eliminarColumnas(dtContratos).DefaultView;

        }

        private DataTable eliminarColumnas(DataTable dt)
        {
            DataTable dtShow = dt.Copy();
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

            dtShow.Columns.Remove("TipoContrato");
            dtShow.Columns.Remove("NombreE");
            dtShow.Columns.Remove("Apellido");
            dtShow.Columns.Remove("DNI");

            return dtShow;
        }


        private void cargarRol()
        {
            if (controladorContrato.Usuario.rol != TipoEmpleado.Basico)
            {
                txbPuesto.IsReadOnly = false;
                txbSalario.IsReadOnly = false;
                txbDuracion.IsReadOnly = false;
                cmbDuracion.IsEnabled = true;
                txbHoraEntrada.IsReadOnly = false;
                txbHoraSalida.IsReadOnly = false;
                cmbTipoContrato.IsEnabled = true;
            }
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

        /// <summary>
        /// Carga las distinatas unidades de tiempo que tendrá el ComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDuracion_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> lDuracion = new List<string>();

            lDuracion.Add("Meses");
            lDuracion.Add("Años");
            lDuracion.Add("Indefinido");

            cmbDuracion.ItemsSource = lDuracion;
        }

        /// <summary>
        /// Guarda en el DataRow actual cada vez que se cambia la unidad de duración. Y si es indefinido, bloquea la parte numérica
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDuracion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!dblClic)
            {
                if (cmbDuracion.SelectedIndex != -1)
                {
                    if (cmbDuracion.SelectedItem.ToString() == "Indefinido")
                    {
                        txbDuracion.Text = string.Empty;
                        txbDuracion.IsEnabled = false;
                    }
                    else
                    {
                        txbDuracion.IsEnabled = true;
                        contratoActual["Duracion"] = txbDuracion.Text + " " + cmbDuracion.SelectedValue.ToString();
                    }
                    hayCambios = true;
                }
            }

        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            controladorContrato.volverMenu();
        }

        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            if (controladorContrato.crearContrato(txbHorasTrabajo.Text, txbHorasDescanso.Text, txbHoraEntrada.Text,
                txbHoraSalida.Text, txbSalario.Text, txbPuesto.Text, txbVacacionesMes.Text, txbDuracion.Text + " "
                + cmbDuracion.Text, txbIdEmpleado.Text, cmbTipoContrato.Text))
            {
                MessageBox.Show("Contrato creado con éxito.");
                cargarDTG(controladorContrato.filtro);
                vaciarCampos();
            }
                
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (contratoActual["IdContrato"].ToString() != string.Empty)
            {
                if (hayCambios)
                {
                    if (controladorContrato.modificarContrato(contratoActual))
                    {
                        cargarDTG(controladorContrato.filtro);
                        contratoActual = dtContratos.Copy().Rows[contContrato];
                        hayCambios = false;
                        MessageBox.Show("Datos guardados correctamente");
                    }
                }
                
            }
        }

        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (dtgContratos.SelectedItem != null)
            {
                DialogResult dr = MessageBox.Show("¿Eliminar el CONTRATO seleccionado?", "Eliminar Contrato",
                    MessageBoxButtons.YesNo);

                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    controladorContrato.eliminarContrato(dtContratos.Rows[dtgContratos.SelectedIndex]["IdContrato"].ToString());
                    cargarDTG(controladorContrato.filtro);
                    vaciarCampos();
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
            if (cmbTipoContrato.SelectedIndex != -1)
            {
                if (contratoActual["IdContrato"].ToString() != string.Empty)
                {
                    string columna = ((System.Windows.Controls.ComboBox)sender).Name.Substring(3);
                    contratoActual[columna] = (TipoContrato)(((System.Windows.Controls.ComboBox)sender).SelectedIndex + 1);
                    hayCambios = true;
                }

                if (cmbTipoContrato.SelectedItem.ToString() == "Parcial Mañana" || cmbTipoContrato.SelectedItem.ToString() == "Parcial Tarde")
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

        /// <summary>
        /// Guarda los cambios del Textbox. El atributo Name del elemento cambiado ha de tener sus 3 primeras letras
        /// descartables y lo demás ha de coincidir con el nombre del atributo en el datatable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cambioContratoTxb(object sender, TextChangedEventArgs e)
        {
            if (!dblClic)//Esto es para que al cargar los Txb después del dtg dobleclick, no haga esto.
            {
                if (contratoActual["IdContrato"].ToString() != string.Empty)
                {
                    string columna = ((System.Windows.Controls.TextBox)sender).Name.Substring(3);
                    contratoActual[columna] = ((System.Windows.Controls.TextBox)sender).Text;
                    if (columna == "Duracion")
                        contratoActual[columna] += " " + cmbDuracion.Text;
                    hayCambios = true;
                }
            }
        }
        private void cmbDuracion_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (!dblClic)//Esto es para que al cargar los Txb después del dtg dobleclick, no haga esto.
            {
                if (contratoActual["IdContrato"].ToString() != string.Empty)
                {
                    string columna = ((System.Windows.Controls.ComboBox)sender).Name.Substring(3);
                    string valor = ((System.Windows.Controls.ComboBox)sender).SelectedValue.ToString();
                    if (valor == "Indefinido")
                        contratoActual[columna] = valor;
                    else
                        contratoActual[columna] = txbDuracion.Text + " " + valor;
                    hayCambios = true;
                }
            }
        }

        private void btnFiltrarCont_Click(object sender, RoutedEventArgs e)
        {
            controladorContrato.prepararFiltro();
        }

        private void Window_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsEnabled)
            {
                cargarDTG(controladorContrato.filtro);
                vaciarCampos();
            }
        }

        private void vaciarCampos()
        {
            dblClic= true;

            contratoActual = dtContratos.NewRow();

            txbNombreE.Text = string.Empty;
            txbPuesto.Text = string.Empty;
            txbSalario.Text = string.Empty;
            cmbDuracion.SelectedValue = -1;
            txbDuracion.Text = string.Empty;
            txbFechaAlta.Text = string.Empty;
            txbFechaBaja.Text = string.Empty;
            txbHoraEntrada.Text = string.Empty;
            txbHoraSalida.Text = string.Empty;
            txbHorasTrabajo.Text = string.Empty;
            txbHorasDescanso.Text = string.Empty;
            txbIdEmpleado.Text = string.Empty;
            txbVacacionesMes.Text = string.Empty;
            cmbTipoContrato.SelectedIndex = - 1;

            dblClic = false;
            hayCambios = false;

            dtgContratos.SelectedItem = null;
        }

        private void chkActivo_Checked(object sender, RoutedEventArgs e)
        {
            string filtroActivo = controladorContrato.filtro;
            if (filtroActivo != string.Empty)
                filtroActivo += " AND ";
            filtroActivo += " Activo = 1";
            cargarDTG(filtroActivo);
        }

        private void chkActivo_Unchecked(object sender, RoutedEventArgs e)
        {
            cargarDTG(controladorContrato.filtro);
        }

        private void dtgContratos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dtgContratos.SelectedItem != null)
            {
                contContrato = dtgContratos.SelectedIndex;
                contratoActual = dtContratos.Copy().Rows[contContrato];
                cargarContrato();
            }
        }

        private void cargarContrato()
        {
            hayCambios= false;
            dblClic= true;

            txbNombreE.Text = contratoActual["Apellido"].ToString() + ", " + contratoActual["NombreE"].ToString();
            txbPuesto.Text = contratoActual["Puesto"].ToString();
            txbSalario.Text = contratoActual["Salario"].ToString();
            if (contratoActual["Puesto"].ToString() == "Indefinido")
                cmbDuracion.SelectedValue = "Indefinido";
            else
            {
                txbDuracion.Text = contratoActual["Duracion"].ToString().Split(' ')[0];
                cmbDuracion.SelectedValue = contratoActual["Duracion"].ToString().Split(' ')[1];
            }

            txbFechaAlta.Text = contratoActual["FechaAlta"].ToString();
            txbFechaBaja.Text = contratoActual["FechaBaja"].ToString();
            txbHoraEntrada.Text = contratoActual["HoraEntrada"].ToString();
            txbHoraSalida.Text = contratoActual["HoraSalida"].ToString();
            txbHorasTrabajo.Text = contratoActual["HorasTrabajo"].ToString();
            txbHorasDescanso.Text = contratoActual["HorasDescanso"].ToString();
            txbIdEmpleado.Text = contratoActual["IdEmpleado"].ToString();
            txbVacacionesMes.Text = contratoActual["VacacionesMes"].ToString();
            cmbTipoContrato.SelectedIndex = Convert.ToInt32(contratoActual["TipoContrato"].ToString()) - 1;

            dblClic = false;
        }

        private void btnVacio_Click(object sender, RoutedEventArgs e)
        {
            cargarDTG(controladorContrato.filtro);
            vaciarCampos();
        }
    }
}
