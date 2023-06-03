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

namespace GestionPersonal.Vistas
{
    /// <summary>
    /// Lógica de interacción para Empleados.xaml
    /// </summary>
    public partial class Empleados : Window
    {
        private readonly EmpleadoControl controladorEmpleado;
        DataTable dtEmpleados = new DataTable();
        int contEmpleado;//para indicar el registro del empleado en el datatable
        DataRow empleadoActual;
        bool hayCambios = false; //con esta variable controlamos si ha habido cambios
        bool dblClic = false;//Solo se me ocurre esto para que controlar que haycambios no se active al cargar desde dtg

        public Empleados(EmpleadoControl controladorEmpleado)
        {
            this.controladorEmpleado = controladorEmpleado;
            InitializeComponent();
            cargarDTG();
            cargarCombos();
        }
        private void cargarDTG()
        {
            dtEmpleados = controladorEmpleado.listaEmpleados();
            dtgEmpleados.ItemsSource = dtEmpleados.DefaultView;
            empleadoActual = dtEmpleados.NewRow(); //Sacamos el formato de la fila
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
                DialogResult dr = System.Windows.Forms.MessageBox.Show("Hay cambios sin guardar, ¿quiere salir?", "Exit", MessageBoxButtons.YesNo);
                if (dr == System.Windows.Forms.DialogResult.No)
                    e.Cancel = true;
            }
        }

        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            if (dniCorrecto())
            {
                if (numssCorrecto())
                {
                    controladorEmpleado.crearEmpleado(txbNombreE.Text, txbApellido.Text, txbUsuario.Text, txbDNI.Text,
                txbNumSS.Text, txbTlf.Text, txbCorreoE.Text, txbIdDepartamento.Text, "1");//IdModif
                    cargarDTG();
                }
                else System.Windows.MessageBox.Show("El número de la seguridad social ha de tener 12 dígitos.");
            }
            else System.Windows.MessageBox.Show("El DNI ha de tener 9 caracteres.");
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (hayCambios && empleadoActual["IdEmpleado"].ToString() != string.Empty)//Condicion explicada en cambioEmpleadoTxb
            {
                if (dniCorrecto())
                {
                    if (numssCorrecto())
                    {
                        controladorEmpleado.modificarEmpleado(empleadoActual);
                        hayCambios = false;
                        cargarDTG();
                    }
                    else System.Windows.MessageBox.Show("El número de la seguridad social ha de tener 12 dígitos.");
                }
                else System.Windows.MessageBox.Show("El DNI ha de tener 9 caracteres.");
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
                }
                hayCambios = true;
            }
        }

        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (dtgEmpleados.SelectedItem != null)
            {
                DialogResult dr = System.Windows.Forms.MessageBox.Show("¿Eliminar el usuario " + dtEmpleados.Rows[dtgEmpleados.SelectedIndex]["Usuario"].ToString() + "?", "Eliminar empleado",
                    MessageBoxButtons.YesNo);

                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    controladorEmpleado.eliminarEmpleado(dtEmpleados.Rows[dtgEmpleados.SelectedIndex]["Usuario"].ToString());
                    cargarDTG();
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
                dblClic = true;
                hayCambios = false;

                contEmpleado = dtgEmpleados.SelectedIndex; //Guardamos la fila por si luego queremos visualizar el siguiente empleado
                empleadoActual = dtEmpleados.Copy().Rows[contEmpleado];//Lo hago con un copy para que no actualize el dtg a medida que cambias los datos y no se pueda interpretar que se están guardando los cambios
                cargarEmpleado();
            }
        }

        //Metodo para cargar los datos del empleado a partir de su índice en la tabla
        private void cargarEmpleado()
        {
            txbNombreE.Text = empleadoActual["NombreE"].ToString();
            txbApellido.Text = empleadoActual["Apellido"].ToString();
            txbUsuario.Text = empleadoActual["Usuario"].ToString();
            txbDNI.Text = empleadoActual["DNI"].ToString();
            txbNumSS.Text = empleadoActual["NumSS"].ToString();
            txbTlf.Text = empleadoActual["Tlf"].ToString();
            txbCorreoE.Text = empleadoActual["CorreoE"].ToString();

            txbIdDepartamento.Text = empleadoActual["IdDepartamento"].ToString();
            //Estaría bien indicar el NOMBRE DEL DEPA

            cmbRol.SelectedIndex = Convert.ToInt32(empleadoActual["Rol"]) - 1;
            cmbEstadoE.SelectedIndex = Convert.ToInt32(empleadoActual["EstadoE"]) - 1;

            dblClic = false;
        }

        private bool dniCorrecto()
        {
            bool correcto = true;
            if (txbDNI.Text.Length != 9) correcto = false;
            return correcto;
        }
        private bool numssCorrecto()
        {
            bool correcto = true;
            if (txbNumSS.Text.Length != 12) correcto = false;
            return correcto;
        }

        private void btnVacio_Click(object sender, RoutedEventArgs e)
        {

            empleadoActual = empleadoActual = dtEmpleados.NewRow();

            txbNombreE.Text = "";
            txbApellido.Text = "";
            txbUsuario.Text = "";
            txbNumSS.Text = "";
            txbTlf.Text = "";
            txbCorreoE.Text = "";
            txbIdDepartamento.Text = "";
            //Estaría bien indicar el NOMBRE DEL DEPA
            cmbRol.SelectedIndex = -1;
            cmbEstadoE.SelectedIndex = -1;
            txbDNI.Text = "";

            dtgEmpleados.SelectedItem = null;
            hayCambios = false;

            cargarDTG();
        }
    }
}
