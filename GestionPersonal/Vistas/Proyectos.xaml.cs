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

            cargarDTG(this.controladorProyecto.filtro);

            proyectoActual = dtProyectos.NewRow();
        }

        /// <summary>
        /// Carga los elementos del formulario en función del rol del Usuario.
        /// </summary>
        private void cargarRol()
        {
            if(controladorProyecto.Usuario.rol == TipoEmpleado.Basico)
            {
                this.controladorProyecto.filtro = $"IdEmpleado = {this.controladorProyecto.Usuario.IdEmpleado}";

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

        /// <summary>
        /// Devuelve un DataTable compuesto solamente por las columnas relevantes para el usuario.
        /// </summary>
        /// <param name="dt">DataTable de Proyectos cuyas columnas se quieren reducir.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Devuelve un DataTable compuesto solamente por las columnas relevantes para el usuario.
        /// </summary>
        /// <param name="dt">DataTable de Empleados cuyas columnas se quieren reducir.</param>
        /// <returns></returns>
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

        /// <summary>
        /// carga el ComboBox de prioridades.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPrioridad_Loaded(object sender, RoutedEventArgs e)
        {
            cmbPrioridad.ItemsSource = Enum.GetValues(typeof(TipoPrioridad));
        }
        
        /// <summary>
        /// Llama al método de vuelta al menú del controlador.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            controladorProyecto.volverMenu();
        }

        /// <summary>
        /// Al hacer clic, si no hay un proyecto cargado, llama al controlador para que cree el proyecto y, si
        /// se crea correctamente, resetea el formulario.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            if (controladorProyecto.crearProyecto(txbNombreP.Text, txbCliente.Text, txbFechaInicioP.Text, txbTiempo.Text, "Meses",
            txbPresupuesto.Text, cmbPrioridad.Text, txbDescripcionP.Text))
            {
                cargarDTG(string.Empty);
                vaciarCampos();
            } 
        }

        /// <summary>
        /// Al hacer clic, si hay un proyecto cargado cuyos datos han sido modificados, llama al controlador para
        /// que actualice dicho proyecto.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (proyectoActual["IdProyecto"].ToString() != string.Empty)
            {
                if (hayCambios)
                {
                    controladorProyecto.modificarProyecto(proyectoActual);
                    hayCambios = false;
                    cargarDTG(controladorProyecto.filtro);
                    proyectoActual = dtProyectos.Copy().Rows[contPro];
                }
            }
        }

        /// <summary>
        /// Guarda los cambios del Textbox localmente. El atributo Name del elemento cambiado ha de tener sus 3 primeras letras
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
        /// Guarda localmente cada vez que se cambia la unidad de Tiempo. Y si es indefinido, bloquea la parte
        /// numérica
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
        /// Guarda localmente cada vez que se cambian las fechas. El atributo Name del elemento cambiado ha de tener sus 3 primeras letras
        /// descartables y lo demás ha de coincidir con el nombre del atributo en el datatable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbFecha_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!dblClic)
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
        /// Guarda localmente cada vez que se cambia la Prioridad.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPrioridad_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!dblClic)
            {
                if (proyectoActual["IdProyecto"].ToString() != string.Empty)
                {
                    proyectoActual["Prioridad"] = cmbPrioridad.SelectedValue.ToString(); 
                    hayCambios = true;
                }
            }
        }

        /// <summary>
        /// Al hacer clic, si hay un proyecto cargado/seleccionado, llama al controlador para que lo elimine tras
        /// pedir confirmación y resetea el formulario si es así.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Al hacer doble clic en algún elemento del DataGrid, se carga el proyecto en el formulario.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// carga los datos del proyecto actual en el formulario.
        /// </summary>
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

        /// <summary>
        /// Caraga los empleados partícipes del proyecto actual.
        /// </summary>
        private void cargarEmpleadosProyecto()
        {
            dtEmpleadosProyecto = controladorProyecto.listaEmpleadosProyecto(proyectoActual["IdProyecto"].ToString());
            dtgEmpleadosPro.ItemsSource = null;
            dtgEmpleadosPro.ItemsSource = eliminarColumnasEmpleado(dtEmpleadosProyecto).DefaultView;
        }

        /// <summary>
        /// Llama al controlador para que abra la ventana Filtro Proyectos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFiltrarPro_Click(object sender, RoutedEventArgs e)
        {
            controladorProyecto.prepararFiltro();
        }

        /// <summary>
        /// Carga el DataGrid de proyectos en base al filtro del controlador cada vez que la ventana se activa, 
        ///  además de llamar al controlador para que añada al proyecto actual el empleado seleccionado en la 
        ///  ventana Busqueda Empleado si lo hay.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsEnabled)
            {
                cargarDTG(controladorProyecto.filtro);
                if(dtProyectos.Rows.Count != 0)
                    proyectoActual = dtProyectos.Copy().Rows[contPro];

                if (controladorProyecto.controladorBusqueda != null)
                {
                    controladorProyecto.aniadirEmpleado(proyectoActual["IdProyecto"].ToString(), proyectoActual["NombreP"].ToString());
                    cargarEmpleadosProyecto();
                }
                else
                    vaciarCampos();
            }
        }

        /// <summary>
        /// Llama al controlador para que abra la ventana de Busqueda Empleado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddE_Click(object sender, RoutedEventArgs e)
        {
            controladorProyecto.prepararFiltroEmpleado();
        }

        /// <summary>
        /// Vacía los elementos del formulario.
        /// </summary>
        private void vaciarCampos()
        {
            dblClic = true;  

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

            hayCambios = false;
            dblClic = false;

        }

        /// <summary>
        /// Resetea el formulario.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVacio_Click(object sender, RoutedEventArgs e)
        {
            vaciarCampos();
            cargarDTG(controladorProyecto.filtro);
        }
        
        /// <summary>
        /// Al hacer clic, si hay un empleado seleccionado, llama al controlador para que elimine su participación
        /// en el proyecto actual tras pedir confirmación.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEliminarE_Click(object sender, RoutedEventArgs e)
        {
            if(dtgEmpleadosPro.SelectedItem != null)
            {
                DialogResult dr = MessageBox.Show("Eliminar a " + dtEmpleadosProyecto.Rows[dtgEmpleadosPro.SelectedIndex]["NombreE"]
                   + " " + dtEmpleadosProyecto.Rows[dtgEmpleadosPro.SelectedIndex]["Apellido"] + " del proyecto",
                   "Eliminar empleado de proyecto", MessageBoxButtons.YesNo);
                if(dr == System.Windows.Forms.DialogResult.Yes)
                {
                    controladorProyecto.eliminarEmpleado(dtEmpleadosProyecto.Rows[dtgEmpleadosPro.SelectedIndex]["IdEmpleado"].ToString(),
                        dtProyectos.Rows[dtgPro.SelectedIndex]["IdProyecto"].ToString(), dtProyectos.Rows[dtgPro.SelectedIndex]["NombreP"].ToString());
                    cargarEmpleadosProyecto();
                }
            }
        }
    }
}
