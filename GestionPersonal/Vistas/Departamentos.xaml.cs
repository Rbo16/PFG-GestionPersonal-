using GestionPersonal.Utiles;
using GestionPersonal.Vistas;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MessageBox = System.Windows.Forms.MessageBox;

namespace GestionPersonal
{

    /// <summary>
    /// Lógica de interacción para Departamentos.xaml
    /// </summary>
    public partial class Departamentos : Window
    {
        private readonly DepartamentoControl controladorDepartamento;
        DataTable dtDepartamentos;
        DataTable dtEmpleadosDep;
        DataRow departamentoActual;
        bool dblClic;
        bool hayCambios;
        int contDepartamento;

        public Departamentos(DepartamentoControl controladorDepartamento)
        {
            this.controladorDepartamento = controladorDepartamento;

            InitializeComponent();

            cargarDTG(string.Empty);

            departamentoActual = dtDepartamentos.NewRow();
        }

        /// <summary>
        /// Carga el DataGrid de departamentos en base al filtro proporcionado. Si este= string.Empty, carga todos
        /// los departamentos del sistema.
        /// </summary>
        /// <param name="filtro"></param>
        private void cargarDTG(string filtro)
        {
            if (filtro == string.Empty)
            {
                dtDepartamentos = controladorDepartamento.listaDepartamentos();
            }
            else
            {
                dtDepartamentos = controladorDepartamento.listaDepartamentos(filtro);
            }

            dtgDep.ItemsSource = null;
            dtgDep.ItemsSource = eliminarColumnasDepartamento(dtDepartamentos).DefaultView;
        }

        /// <summary>
        /// Devuelve un DataTable compuesto solamente por las columnas relevantes para el usuario.
        /// </summary>
        /// <param name="dt">DataTable de departamento cuyas columnas se quieren reducir.</param>
        /// <returns></returns>
        private DataTable eliminarColumnasDepartamento(DataTable dt)
        {
            DataTable dtShow = dt.Copy();

            dtShow.Columns.Remove("IdDepartamento");
            dtShow.Columns.Remove("IdJefeDep");
            dtShow.Columns.Remove("IdEmpleado");
            dtShow.Columns.Remove("NombreE");
            dtShow.Columns.Remove("Apellido");
            dtShow.Columns.Remove("FechaUltModif");
            dtShow.Columns.Remove("IdModif");
            dtShow.Columns.Remove("Borrado");

            return dtShow;
        }
        /// <summary>
        /// Devuelve un DataTable compuesto solamente por las columnas relevantes para el usuario.
        /// </summary>
        /// <param name="dt">DataTable de empleados cuyas columnas se quieren reducir.</param>
        /// <returns></returns>
        private DataTable eliminarColumnasEmpleado(DataTable dt)
        {
            DataTable dtShow = dt.Copy();

            dtShow.Columns.Remove("IdEmpleado");
            dtShow.Columns.Remove("Contrasenia");
            dtShow.Columns.Remove("NumSS");
            dtShow.Columns.Remove("EstadoE");
            dtShow.Columns.Remove("DNI");
            dtShow.Columns.Remove("Rol");
            dtShow.Columns.Remove("Estado");
            dtShow.Columns.Remove("IdDepartamento");
            dtShow.Columns.Remove("FechaUltModif");
            dtShow.Columns.Remove("Borrado");
            dtShow.Columns.Remove("IdModif");
            dtShow.Columns.Remove("NombreD");

            return dtShow;
        }

        /// <summary>
        /// Al hacer clis, llama al controlador para que cree un departamento con los datos del formulario, si se
        /// crea exitosamente, se resetea el formulario.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            if (controladorDepartamento.crearDepartamento(txbNombreD.Text, txbDescripcionD.Text) == true)
            {
                
                cargarDTG(string.Empty);
                vaciarCampos();
            } 
        }

        /// <summary>
        /// Al hacer clic, si hay un departamento cargado cuyos datos se han modificado, llama al controlador para
        /// que actualice los cambios.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (departamentoActual["IdDepartamento"].ToString() != string.Empty)
            {
                if (hayCambios)
                {
                    controladorDepartamento.modificarDepartamento(departamentoActual);
                    cargarDTG(string.Empty);
                    departamentoActual = dtDepartamentos.Copy().Rows[contDepartamento];
                    hayCambios = false;
                } 
            }
        }

        /// <summary>
        /// Al hacer clic, si hay algún departamento cargado/seleccionado, llama al controlador para que lo
        /// elimine tras pedir una confirmación y resetea el formulario si es así.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (dtgDep.SelectedItem != null)
            {
                //if (dtEmpleadosDep.Rows.Count == 0)
                //{
                    DialogResult dr = MessageBox.Show("¿Eliminar el DEPARTAMENTO seleccionado?", "Eliminar Departamento",
                   MessageBoxButtons.YesNo);

                    if (dr == System.Windows.Forms.DialogResult.Yes)
                    {
                        controladorDepartamento.eliminarDepartamento(dtDepartamentos.Rows[dtgDep.SelectedIndex]["IdDepartamento"].ToString());
                        cargarDTG(string.Empty);
                        vaciarCampos();
                    }
                    else
                        return;
                //}
                //else
                    //MessageBox.Show("El departamento no puede contener ningún empleado a la hora de ser eliminado.");
            }
            else
                System.Windows.MessageBox.Show("Seleccione un departamento de la tabla");
        }

        /// <summary>
        /// Llama al método de vuelta al menú del controlador.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            controladorDepartamento.volverMenu();
        }

        /// <summary>
        /// al hacer doble clic en un elemento del DataGrid de departamentos, este se carga en el formulario.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtgDep_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dtgDep.SelectedItem != null)
            {
                contDepartamento = dtgDep.SelectedIndex;
                departamentoActual = dtDepartamentos.Copy().Rows[contDepartamento];
                cargarDepartamento();

                cargarEmpleadosDepartamento();
            }
        }

        /// <summary>
        /// Carga los datos del departamento actual en el formulario.
        /// </summary>
        private void cargarDepartamento()
        {
            hayCambios = false;
            dblClic = true;

            txbNombreD.Text = departamentoActual["NombreD"].ToString();
            txbDescripcionD.Text = departamentoActual["DescripcionD"].ToString();
            txbJefe.Text = departamentoActual["NombreE"].ToString() + " " + departamentoActual["Apellido"].ToString();

            dblClic = false;
        }

        /// <summary>
        /// Carga el DataGrid de empleados de un departamento.
        /// </summary>
        private void cargarEmpleadosDepartamento()
        {
            dtEmpleadosDep = controladorDepartamento.listaEmpleadosDepartamento(departamentoActual["IdDepartamento"].ToString());
            dtgEmpleadosDep.ItemsSource = null;
            dtgEmpleadosDep.ItemsSource = eliminarColumnasEmpleado(dtEmpleadosDep).DefaultView;
        }

        /// <summary>
        /// Guarda los cambios del TextBox en la fila referente al departamento actual. El atributo Name del elemento
        /// cambiado ha de tener sus 3 primeras letras descartables y lo demás ha de coincidir con el nombre del atributo
        /// en el datatable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!dblClic)//Esto es para que al cargar los Txb después del dtg dobleclick, no haga esto.
            {
                if (departamentoActual["IdDepartamento"].ToString() != string.Empty)
                {
                    string columna = ((System.Windows.Controls.TextBox)sender).Name.Substring(3);
                    departamentoActual[columna] = ((System.Windows.Controls.TextBox)sender).Text;
                }
                hayCambios = true;
            }
        }

        /// <summary>
        /// Cuando el Textbox obtiene el foco, se elimina su contenido. Esto solo ocurre la primera vez que recibe el foco.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbBucarDep_GotFocus(object sender, RoutedEventArgs e)
        {
            txbBucarDep.Text= string.Empty;
            txbBucarDep.GotFocus -= txbBucarDep_GotFocus;
            txbBucarDep.TextChanged += txbBucarDep_TextChanged;
        }

        /// <summary>
        /// Muestra en el DataGrid los departamentos cuyo nombre coincida con el contenido del Textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbBucarDep_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filtro = $"NombreD like '%{txbBucarDep.Text}%'";
            cargarDTG(filtro);
        }

        /// <summary>
        /// Llama al controlador para que abra la ventana Busqueda Empleado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddE_Click(object sender, RoutedEventArgs e)
        {
            controladorDepartamento.prepararBusquedaEmpleado();
        }

        /// <summary>
        /// Cada vez que la ventana se activa, se añade el empleado seleccionado en la ventana Busqueda Empleado, si
        /// es que lo hay.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsEnabled)
            {
                controladorDepartamento.aniadirEmpleado(departamentoActual["IdDepartamento"].ToString(), departamentoActual["NombreD"].ToString());
                cargarEmpleadosDepartamento();
            }
        }

        /// <summary>
        /// Si hay un empleado seleccionado, pregunta si se desea asignar dicho empleado como jefe. Si la respuesta
        /// es "Sí", llama al controlador para que actualice el jefe de departamento, y también lo actualiza localmente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnJefe_Click(object sender, RoutedEventArgs e)
        {
            if (dtgEmpleadosDep.SelectedItem != null)
            {
                DialogResult dr = MessageBox.Show("¿Desea nombrar a " + dtEmpleadosDep.Rows[dtgEmpleadosDep.SelectedIndex]["NombreE"].ToString() +
                    " " + dtEmpleadosDep.Rows[dtgEmpleadosDep.SelectedIndex]["Apellido"].ToString() + " como jefe del departamento " +
                    departamentoActual["NombreD"].ToString() + "?", "Confirmación", MessageBoxButtons.YesNo);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    if(controladorDepartamento.asignarJefe(dtEmpleadosDep.Rows[dtgEmpleadosDep.SelectedIndex]["IdEmpleado"].ToString(),
                        departamentoActual["IdDepartamento"].ToString(), departamentoActual["NombreD"].ToString()))
                    {
                        dblClic = true;
                        cargarDTG(string.Empty);
                        departamentoActual["NombreE"] = dtEmpleadosDep.Rows[dtgEmpleadosDep.SelectedIndex]["NombreE"].ToString();
                        departamentoActual["Apellido"] = dtEmpleadosDep.Rows[dtgEmpleadosDep.SelectedIndex]["Apellido"].ToString();
                        cargarDepartamento();
                    }
                        
                }
            }
            else
                MessageBox.Show("Debe seleccionar un empleado.");
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
            departamentoActual = dtDepartamentos.NewRow();
            
            txbBucarDep.Text = "Buscar departamento...";
            txbBucarDep.GotFocus+= txbBucarDep_GotFocus;

            txbDescripcionD.Text= string.Empty;
            txbNombreD.Text= string.Empty;
            txbJefe.Text= string.Empty;

            dtgDep.SelectedItem = null;
            dtgEmpleadosDep.SelectedItem= null;
            dtgEmpleadosDep.ItemsSource = null;

            dblClic = false;
            hayCambios = false;
        }
    }
}
