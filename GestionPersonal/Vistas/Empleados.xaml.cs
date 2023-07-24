using GestionPersonal.Controladores;
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
    /// Lógica de interacción para Empleados.xaml
    /// </summary>
    public partial class Empleados : Window
    {
        private readonly EmpleadoControl controladorEmpleado;
        DataTable dtEmpleados = new DataTable();
        int contEmpleado;
        DataRow empleadoActual;
        bool hayCambios = false;
        bool dblClic = false;
        bool cambioEstado = false;

        public Empleados(EmpleadoControl controladorEmpleado)
        {
            this.controladorEmpleado = controladorEmpleado;

            InitializeComponent();

            cargarRol();

            cargarDTG(string.Empty);

            empleadoActual = dtEmpleados.NewRow();

            cargarCombos();
        }

        /// <summary>
        /// Carga los elementos de la vista según el rol del usuario manejando la aplicación.
        /// </summary>
        private void cargarRol()
        {
            if (controladorEmpleado.Usuario.rol == TipoEmpleado.Gestor)
                cmbEstadoE.IsEnabled = true;
        }

        /// <summary>
        /// Carga el listado de empleados en función del filtro que se le indique. Si el filtro = string.Empty,
        /// carga un listado de todos los empleados del sistema
        /// </summary>
        /// <param name="filtro"></param>
        private void cargarDTG(string filtro)
        {
            if (filtro == string.Empty)
            {
                dtEmpleados = controladorEmpleado.listaEmpleados();
            }
            else
            {
                dtEmpleados = controladorEmpleado.listaEmpleados(filtro);
            }

            dtgEmpleados.ItemsSource = null;
            dtgEmpleados.ItemsSource = eliminarColumnas(dtEmpleados).DefaultView;
        }

        /// <summary>
        /// Devuelve un DataTable compuesto solamente por las columnas relevantes para el usuario.
        /// </summary>
        /// <param name="dt">DataTable de Empleados cuyas columnas se quieren reducir.</param>
        /// <returns></returns>
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

            return dtShow;
        }

        /// <summary>
        /// Carga los ComboBox de Rol y Estado.
        /// </summary>
        private void cargarCombos()
        {
            cmbRol.ItemsSource = Enum.GetValues(typeof(TipoEmpleado));
            cmbEstadoE.ItemsSource = Enum.GetValues(typeof(EstadoEmpleado));
        }

        /// <summary>
        /// Llama al método de vuelta al menú del controlador.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            controladorEmpleado.volverMenu();
        }

        /// <summary>
        /// Si, al querer cerrar la ventana, hay cambios pendientes, pide una confirmación de salida.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (hayCambios)
            {
                DialogResult dr = MessageBox.Show("Hay cambios sin guardar, ¿quiere salir?", "Exit", MessageBoxButtons.YesNo);
                if (dr == System.Windows.Forms.DialogResult.No)
                    e.Cancel = true;
            }
        }

        /// <summary>
        /// Al hacer clic, si no hay un empleado cargado, llama al controlador para crear un nuevo empleado con
        /// los datos del formulario y, si se crea correctamente, resetea el formulario.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            if (dtgEmpleados.SelectedItem == null)
            {
                if (controladorEmpleado.crearEmpleado(txbNombreE.Text, txbApellido.Text, txbUsuario.Text, txbDNI.Text,
                    txbNumSS.Text, txbTlf.Text, txbCorreoE.Text))
                {
                    cargarDTG(string.Empty);
                    vaciarCampos();
                }
            }
            else
                MessageBox.Show("No puede haber ningún empleado seleccionado a la hora de crear uno nuevo");

        }

        /// <summary>
        /// Al hacer clic, si hay una ausencia cargada cuyos datos han sido modificados, llama al controlador para
        /// que la actualice.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (hayCambios && empleadoActual["IdEmpleado"].ToString() != string.Empty)
            {
                controladorEmpleado.modificarEmpleado(empleadoActual);
                hayCambios = false;
                cargarDTG(string.Empty);
                empleadoActual = dtEmpleados.Copy().Rows[contEmpleado];
            }

            if (cambioEstado && empleadoActual["IdEmpleado"].ToString() != string.Empty)
            {
                controladorEmpleado.gestionarEmpleado(empleadoActual["IdEmpleado"].ToString(), empleadoActual["EstadoE"].ToString());
                cambioEstado= false;
                cargarDTG(string.Empty);
                empleadoActual = dtEmpleados.Copy().Rows[contEmpleado];
            }
        }

        /// <summary>
        /// Guarda los cambios del Textbox localmente. El atributo Name del elemento cambiado ha de tener sus 3 primeras letras
        /// descartables y lo demás ha de coincidir con el nombre del atributo en el datatable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cambioEmpleadoTxb(object sender, TextChangedEventArgs e)
        {
            if (!dblClic)
            {
                if (empleadoActual["IdEmpleado"].ToString() != string.Empty)
                                                                           
                {
                    string columna = ((System.Windows.Controls.TextBox)sender).Name.Substring(3);
                    empleadoActual[columna] = ((System.Windows.Controls.TextBox)sender).Text;
                    hayCambios = true;
                }
            }
        }

        /// <summary>
        /// Guarda los cambios del ComboBox localmente. El atributo Name del elemento cambiado ha de tener sus 3 primeras letras
        /// descartables y lo demás ha de coincidir con el nombre del atributo en el datatable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!dblClic)
            {
                if (empleadoActual["IdEmpleado"].ToString() != string.Empty)
                                                                            
                {
                    string columna = ((System.Windows.Controls.ComboBox)sender).Name.Substring(3);
                    empleadoActual[columna] = ((System.Windows.Controls.ComboBox)sender).SelectedIndex + 1;
                    if (columna == "EstadoE")
                        cambioEstado = true;
                    else
                        hayCambios = true;
                } 
            }
        }

        /// <summary>
        /// Si hay un empleado cargado/seleccionado, llama al controlador para que lo elimine tras pedir una
        /// confirmación y resetea el formulario si es así.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (dtgEmpleados.SelectedItem != null)
            {
                DialogResult dr = System.Windows.Forms.MessageBox.Show("¿Eliminar el EMPLEADO seleccionado?", "Eliminar Empleado",
                    MessageBoxButtons.YesNo);

                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    controladorEmpleado.eliminarEmpleado(dtEmpleados.Rows[dtgEmpleados.SelectedIndex]["IdEmpleado"].ToString());
                    cargarDTG(string.Empty);
                    vaciarCampos();
                }
                else
                    return;

            }
            else
                System.Windows.MessageBox.Show("Seleccione un empleado en la tabla");
        }

        /// <summary>
        /// Al hacer doble clic en algún elemento del DataGrid, carga el empleado en el formulario.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtgEmpleados_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dtgEmpleados.SelectedItem == null)
                return;
            else
            {
                contEmpleado = dtgEmpleados.SelectedIndex; 
                empleadoActual = dtEmpleados.Copy().Rows[contEmpleado];
                cargarEmpleado();
            }
        }

        /// <summary>
        /// Carga los datos del empleado actual en el formulario.
        /// </summary>
        private void cargarEmpleado()
        {

            dblClic = true;

            txbNombreE.Text = empleadoActual["NombreE"].ToString();
            txbApellido.Text = empleadoActual["Apellido"].ToString();
            txbUsuario.Text = empleadoActual["Usuario"].ToString();
            txbDNI.Text = empleadoActual["DNI"].ToString();
            txbNumSS.Text = empleadoActual["NumSS"].ToString();
            txbTlf.Text = empleadoActual["Tlf"].ToString();
            txbCorreoE.Text = empleadoActual["CorreoE"].ToString();
            txbIdDepartamento.Text = empleadoActual["NombreD"].ToString();
            cmbRol.SelectedIndex = Convert.ToInt32(empleadoActual["Rol"]) - 1;
            cmbEstadoE.SelectedIndex = Convert.ToInt32(empleadoActual["EstadoE"]) - 1;

            dtgEmpleados.SelectedItem = null;
            hayCambios = false;
            cambioEstado = false;
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
            cargarDTG(string.Empty);
        }

        /// <summary>
        /// Vacía los elementos del formulario.
        /// </summary>
        private void vaciarCampos()
        {
            dblClic = true;

            empleadoActual = dtEmpleados.NewRow();

            txbNombreE.Text = "";
            txbApellido.Text = "";
            txbUsuario.Text = "";
            txbNumSS.Text = "";
            txbTlf.Text = "";
            txbCorreoE.Text = "";
            txbIdDepartamento.Text = "";
            cmbRol.SelectedIndex = -1;
            cmbEstadoE.SelectedIndex = -1;
            txbDNI.Text = "";

            dtgEmpleados.SelectedItem = null;
            hayCambios = false;
            cambioEstado = false;
            dblClic = false;

        }

        /// <summary>
        /// Al hacer clic, llama al controlador para que abra la ventana Filtro Empleado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFiltrarE_Click(object sender, RoutedEventArgs e)
        {
            controladorEmpleado.prepararFiltro();
        }

        /// <summary>
        /// Carga el DataGrid en base al filtro del controlador cada vez que la ventana se activa.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsEnabled)
            {
                cargarDTG(controladorEmpleado.filtro);
                vaciarCampos();
            }
        }


    }
}
