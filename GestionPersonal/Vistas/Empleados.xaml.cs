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
        int contEmpleado;//para indicar el registro del empleado en el datatable. SIGUIENTE-ANTERIOR
        DataRow empleadoActual;
        bool hayCambios = false; //con esta variable controlamos si ha habido cambios
        bool dblClic = false;//Solo se me ocurre esto para que controlar que haycambios no se active al cargar desde dtg

        public Empleados(EmpleadoControl controladorEmpleado)
        {
            this.controladorEmpleado = controladorEmpleado;

            InitializeComponent();

            cargarDTG(string.Empty);

            empleadoActual = dtEmpleados.NewRow();

            cargarCombos();
        }
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

        private void cargarCombos()
        {
            cmbRol.ItemsSource = controladorEmpleado.devolverEnum("TipoEmpleado");
            cmbEstadoE.ItemsSource = controladorEmpleado.devolverEnum("EstadoEmpleado");
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            controladorEmpleado.volverMenu();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (hayCambios)
            {
                DialogResult dr = MessageBox.Show("Hay cambios sin guardar, ¿quiere salir?", "Exit", MessageBoxButtons.YesNo);
                if (dr == System.Windows.Forms.DialogResult.No)
                    e.Cancel = true;
            }
        }

        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            if (dtgEmpleados.SelectedItem == null)
            {
                if (controladorEmpleado.crearEmpleado(txbNombreE.Text, txbApellido.Text, txbUsuario.Text, txbDNI.Text,
                    txbNumSS.Text, txbTlf.Text, txbCorreoE.Text))
                {
                    MessageBox.Show("Empleado creado correctamente.");
                    cargarDTG(string.Empty);
                    vaciarCampos();
                }
            }
            else
                MessageBox.Show("No puede haber ningún empleado seleccionado a la hora de crear uno nuevo");

        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (hayCambios && empleadoActual["IdEmpleado"].ToString() != string.Empty)//Condicion explicada en cambioEmpleadoTxb
            {
                controladorEmpleado.modificarEmpleado(empleadoActual);
                hayCambios = false;
                cargarDTG(string.Empty);
                empleadoActual = dtEmpleados.Copy().Rows[contEmpleado];
            }
        }

        /// <summary>
        /// Guarda los cambios del Textbox. El atributo Name del elemento cambiado ha de tener sus 3 primeras letras
        /// descartables y lo demás ha de coincidir con el nombre del atributo en el datatable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cambioEmpleadoTxb(object sender, TextChangedEventArgs e)
        {
            if (!dblClic)
            {
                if (empleadoActual["IdEmpleado"].ToString() != string.Empty)//Esto es para que al cargar los Txb después del dtg dobleclick, no haga esto.
                                                                            //Y para que solo lo haga cuando un empleado ha sido cargado, es decir, hay Id en el datarow
                {
                    string columna = ((System.Windows.Controls.TextBox)sender).Name.Substring(3);
                    empleadoActual[columna] = ((System.Windows.Controls.TextBox)sender).Text;
                    hayCambios = true;
                }
            }
        }


        private void cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!dblClic)
            {
                if (empleadoActual["IdEmpleado"].ToString() != string.Empty)//Esto es para que al cargar los Cmb después del dtg dobleclick, no haga esto.
                                                                            //Y para que solo lo haga cuando un empleado ha sido cargado, es decir, hay Id en el datarow
                {
                    string columna = ((System.Windows.Controls.ComboBox)sender).Name.Substring(3);
                    empleadoActual[columna] = ((System.Windows.Controls.ComboBox)sender).SelectedIndex + 1;
                    hayCambios = true;
                } 
            }
        }

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
                    MessageBox.Show("Empleado eliminado correctamente.");
                }
                else
                    return;

            }
            else
                System.Windows.MessageBox.Show("Seleccione un empleado en la tabla");
        }

        private void dtgEmpleados_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dtgEmpleados.SelectedItem == null)
                return;
            else
            {
                contEmpleado = dtgEmpleados.SelectedIndex; //Guardamos la fila por si luego queremos visualizar el siguiente empleado
                empleadoActual = dtEmpleados.Copy().Rows[contEmpleado];//Lo hago con un copy para que no actualize el dtg a medida que cambias los datos y no se pueda interpretar que se están guardando los cambios
                cargarEmpleado();
            }
        }

        //Metodo para cargar los datos del empleado a partir de su índice en la tabla
        private void cargarEmpleado()
        {
            hayCambios = false;
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

            dblClic = false;
        }


        private void btnVacio_Click(object sender, RoutedEventArgs e)
        {
            vaciarCampos();
            cargarDTG(string.Empty);
        }

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
            dblClic = false;

        }

        private void btnFiltrarE_Click(object sender, RoutedEventArgs e)
        {
            controladorEmpleado.prepararFiltro();
        }

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
