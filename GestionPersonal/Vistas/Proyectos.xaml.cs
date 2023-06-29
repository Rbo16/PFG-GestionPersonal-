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
    /// Lógica de interacción para Proyectos.xaml
    /// </summary>
    public partial class Proyectos : Window
    {
        private readonly ProyectoControl controladorProyecto;
        DataTable dtProyectos;
        DataTable dtEmpleadosProyecto;
        DataRow proyectoActual;
        bool hayCambios;
        bool dblClic;
        int contPro;

        public Proyectos(ProyectoControl controladorProyecto)
        {
            this.controladorProyecto = controladorProyecto;

            InitializeComponent();

            cargarRol();

            this.controladorProyecto.filtro = $"IdEmpleado = {this.controladorProyecto.Usuario.IdEmpleado}";

            cargarDTG(this.controladorProyecto.filtro);

            proyectoActual = dtProyectos.NewRow();
        }

        private void cargarRol()
        {
            if(controladorProyecto.Usuario.rol == TipoEmpleado.Basico)
            {
                txbNombreP.IsReadOnly= true;
                txbCliente.IsReadOnly= true;
                txbDescripcionP.IsReadOnly= true;
                txbFechaFinP.IsEnabled = false;
                txbFechaInicioP.IsEnabled = false;
                txbPresupuesto.IsReadOnly= true;
                txbTiempo.IsReadOnly= true;
                cmbDuracion.IsEnabled= false;
                cmbPrioridad.IsEnabled= false;
                btnAddE.Visibility= Visibility.Hidden;
                btnCrear.Visibility= Visibility.Hidden;
                btnEliminarE.Visibility= Visibility.Hidden;
                btnGuardar.Visibility= Visibility.Hidden;
                btnBorrar.Visibility= Visibility.Hidden;
            }
        }

        /// <summary>
        /// Carga el listado de proyectos en función del filtro que se le indique. Si el filtro = string.Empty,
        /// carga un listado de todos los proyectos del sistema
        /// </summary>
        /// <param name="filtro"></param>
        private void cargarDTG(string filtro)
        {
            if (filtro == string.Empty)
            {
                dtProyectos = controladorProyecto.listaProyectos();
            }
            else
            {
                dtProyectos = controladorProyecto.listaProyectos(filtro);
            }

            dtgPro.ItemsSource = null;
            dtgPro.ItemsSource = eliminarColumnasProyecto(dtProyectos).DefaultView;

        }

        private DataTable eliminarColumnasProyecto(DataTable dt)
        {
            DataTable dtShow = dt.Copy();

            dtShow.Columns.Remove("IdProyecto");
            dtShow.Columns.Remove("Tiempo");
            dtShow.Columns.Remove("Presupuesto");
            dtShow.Columns.Remove("DescripcionP");
            dtShow.Columns.Remove("FechaUltModif");
            dtShow.Columns.Remove("IdModif");
            dtShow.Columns.Remove("Borrado");

            return dtShow;
        }

        private DataTable eliminarColumnasEmpleado(DataTable dt)
        {
            DataTable dtShow = dt.Copy();

            dtShow.Columns.Remove("IdProyecto");
            dtShow.Columns.Remove("IdEmpleado");
            dtShow.Columns.Remove("Contrasenia");
            dtShow.Columns.Remove("NumSS");
            dtShow.Columns.Remove("EstadoE");
            dtShow.Columns.Remove("DNI");
            dtShow.Columns.Remove("Rol");
            dtShow.Columns.Remove("IdDepartamento");
            dtShow.Columns.Remove("FechaUltModif");
            dtShow.Columns.Remove("Borrado");
            dtShow.Columns.Remove("IdModif");

            return dtShow;
        }

        /// <summary>
        /// Carga las distinatas unidades de tiempo que tendrá el ComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDuracion_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> lDuracion = new List<string>();

            lDuracion.Add("Días");
            lDuracion.Add("Meses");
            lDuracion.Add("Años");
            lDuracion.Add("Indefinido");

            cmbDuracion.ItemsSource = lDuracion;
        }
        private void cmbPrioridad_Loaded(object sender, RoutedEventArgs e)
        {
            cmbPrioridad.ItemsSource = Enum.GetValues(typeof(TipoPrioridad));
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            controladorProyecto.volverMenu();
        }

        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            if (controladorProyecto.crearProyecto(txbNombreP.Text, txbCliente.Text, txbFechaInicioP.Text, txbTiempo.Text, "Meses",
            txbPresupuesto.Text, cmbPrioridad.Text, txbDescripcionP.Text))
            {
                MessageBox.Show("Proyecto creado córrectamente");
                cargarDTG(string.Empty);
                vaciarCampos();
            } 
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (proyectoActual["IdProyecto"].ToString() != string.Empty)
            {
                if (hayCambios)
                {
                    controladorProyecto.modificarProyecto(proyectoActual);
                    hayCambios = false;
                    MessageBox.Show("Datos guardados correctamente");
                    cargarDTG(controladorProyecto.filtro);
                    proyectoActual = dtProyectos.Copy().Rows[contPro];
                }
            }
        }

        /// <summary>
        /// Guarda los cambios del Textbox. El atributo Name del elemento cambiado ha de tener sus 3 primeras letras
        /// descartables y lo demás ha de coincidir con el nombre del atributo en el datatable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cambioTxb(object sender, TextChangedEventArgs e)
        {
            if (!dblClic)//Esto es para que al cargar los Txb después del dtg dobleclick, no haga esto.
            {
                if (proyectoActual["IdProyecto"].ToString() != string.Empty)
                {

                    string columna = ((System.Windows.Controls.TextBox)sender).Name.Substring(3);
                    proyectoActual[columna] = ((System.Windows.Controls.TextBox)sender).Text;
                    if (columna == "Tiempo")
                        proyectoActual[columna] += " " + cmbDuracion.Text;
                    hayCambios = true;
                }   
            }
        }
        /// <summary>
        /// Guarda en el DataRow actual cada vez que se cambia la unidad de Tiempo. Y si es indefinido, bloquea la parte numérica
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
                        txbTiempo.Text = string.Empty;
                        txbTiempo.IsEnabled = false;
                    }
                    else
                    {
                        txbTiempo.IsEnabled = true;
                        proyectoActual["Tiempo"] = txbTiempo.Text + " " + cmbDuracion.SelectedValue.ToString();
                    }
                    hayCambios = true;
                }            
            }
        }

        /// <summary>
        /// Guarda en el DataRow actual cada vez que se cambian las fechas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbFecha_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!dblClic)//Esto es para que al cargar los Txb después del dtg dobleclick, no haga esto.
            {
                if (proyectoActual["IdProyecto"].ToString() != string.Empty)
                {
                    string columna = ((System.Windows.Controls.DatePicker)sender).Name.Substring(3);
                    proyectoActual[columna] = ((System.Windows.Controls.DatePicker)sender).SelectedDate;
                    hayCambios = true;
                }
                
            }
        }

        /// <summary>
        /// Guarda en el DataRow actual cada vez que se cambia la Prioridad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPrioridad_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!dblClic)//Esto es para que al cargar los Txb después del dtg dobleclick, no haga esto.
            {
                if (proyectoActual["IdProyecto"].ToString() != string.Empty)
                {
                    proyectoActual["Prioridad"] = cmbPrioridad.SelectedValue.ToString(); 
                    hayCambios = true;
                }
            }
        }

        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (dtgPro.SelectedItem != null)
            {
                DialogResult dr = MessageBox.Show("¿Eliminar el PROYECTO seleccionada?", "Eliminar Proyecto",
                    MessageBoxButtons.YesNo);

                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    controladorProyecto.borrarProyecto(dtProyectos.Rows[dtgPro.SelectedIndex]["IdProyecto"].ToString());
                    cargarDTG(controladorProyecto.filtro);
                    vaciarCampos();
                }
                else
                    return;
            }
        }

        private void dtgPro_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dtgPro.SelectedItem == null)
            {
                return;
            }
            else
            {
                contPro = dtgPro.SelectedIndex;
                proyectoActual = dtProyectos.Copy().Rows[contPro];
                cargarProyecto();

                cargarEmpleadosProyecto();
            }
        }

        private void cargarProyecto()
        {
            dblClic = true;
            hayCambios = false;

            txbNombreP.Text = proyectoActual["NombreP"].ToString();
            txbCliente.Text = proyectoActual["Cliente"].ToString();
            txbFechaInicioP.Text = proyectoActual["FechaInicioP"].ToString();
            txbFechaFinP.Text = proyectoActual["FechaFinP"].ToString();
            txbTiempo.Text = proyectoActual["Tiempo"].ToString().Split(' ')[0];
            cmbDuracion.Text = proyectoActual["Tiempo"].ToString().Split(' ')[1];
            txbPresupuesto.Text = proyectoActual["Presupuesto"].ToString();
            cmbPrioridad.SelectedIndex = Convert.ToInt32(proyectoActual["Presupuesto"].ToString()) - 1;
            txbDescripcionP.Text = proyectoActual["DescripcionP"].ToString();

            dblClic = false;
        }

        private void cargarEmpleadosProyecto()
        {
            dtEmpleadosProyecto = controladorProyecto.listaEmpleadosProyecto(proyectoActual["IdProyecto"].ToString());
            dtgEmpleadosPro.ItemsSource = null;
            dtgEmpleadosPro.ItemsSource = eliminarColumnasEmpleado(dtEmpleadosProyecto).DefaultView;
        }

        private void btnFiltrarPro_Click(object sender, RoutedEventArgs e)
        {
            controladorProyecto.prepararFiltro();
        }

        private void Window_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsEnabled)
            {
                cargarDTG(controladorProyecto.filtro);

                if (controladorProyecto.controladorBusqueda != null)
                    controladorProyecto.aniadirEmpleado(proyectoActual["IdProyecto"].ToString());
                else
                    vaciarCampos();
            }
        }

        private void btnAddE_Click(object sender, RoutedEventArgs e)
        {
            controladorProyecto.prepararFiltroEmpleado();
        }

        private void vaciarCampos()
        {
            dblClic = true;
            hayCambios = false;

            proyectoActual = dtProyectos.NewRow();

            txbNombreP.Text = string.Empty;
            txbCliente.Text = string.Empty;
            txbFechaInicioP.Text = string.Empty;
            txbFechaFinP.Text = string.Empty;
            txbTiempo.Text = string.Empty;
            cmbDuracion.SelectedIndex = -1;
            txbPresupuesto.Text = string.Empty;
            cmbPrioridad.SelectedIndex = -1;
            txbDescripcionP.Text = string.Empty;

            controladorProyecto.controladorBusqueda = null;

            dtgEmpleadosPro.ItemsSource = null;
            dtgPro.SelectedItem= null;

            dblClic = false;

        }

        private void btnVacio_Click(object sender, RoutedEventArgs e)
        {
            vaciarCampos();
            cargarDTG(string.Empty);
        }
    }
}
