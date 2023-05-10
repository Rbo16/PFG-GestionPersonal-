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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GestionPersonal
{
    /// <summary>
    /// Lógica de interacción para Empleados.xaml
    /// </summary>
    public partial class Empleados : Window
    {
        EmpleadoControl ControladorE = new EmpleadoControl();
        DataTable dtEmpleados = new DataTable();
        int contEmpleado;

        public Empleados()
        {
            InitializeComponent();
            cargarDTG();
            cargarCombos();
        }

        private void cargarDTG()
        {
            dtEmpleados = ControladorE.listarEmpleados();
            dtgEmpleados.ItemsSource = dtEmpleados.DefaultView;
        }

        private void cargarCombos()
        {
            cmbRol.ItemsSource = ControladorE.devolverEnum("TipoEmpleado");
            cmbEstadoE.ItemsSource = ControladorE.devolverEnum("EstadoEmpleado");
        }
        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dtgEmpleados_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dtgEmpleados.SelectedItem == null)
                return;
            else
            {
                contEmpleado = dtgEmpleados.SelectedIndex; //Guardamos la fila por si luego queremos visualizar el siguiente empleado
                cargarEmpleado(contEmpleado);
            }
        }

        //Metodo para cargar los datos del empleado a partir de su índice en la tabla
        private void cargarEmpleado(int posicion)
        {
            txbNombreE.Text = dtEmpleados.Rows[posicion]["NombreE"].ToString();
            txbApellido.Text = dtEmpleados.Rows[posicion]["Apellido"].ToString();
            txbUsuario.Text = dtEmpleados.Rows[posicion]["Usuario"].ToString();
            txbDNI.Text = dtEmpleados.Rows[posicion]["DNI"].ToString();
            txbNumSS.Text = dtEmpleados.Rows[posicion]["NumSS"].ToString();
            txbTlf.Text = dtEmpleados.Rows[posicion]["Tlf"].ToString();
            txbCorreoE.Text = dtEmpleados.Rows[posicion]["CorreoE"].ToString();

            txbDepartamento.Text = dtEmpleados.Rows[posicion]["IdDepartamento"].ToString(); 
            //Estaría bien indicar el NOMBRE DEL DEPA

            cmbRol.SelectedIndex = Convert.ToInt32(dtEmpleados.Rows[posicion]["Rol"]) - 1;
            cmbEstadoE.SelectedIndex = Convert.ToInt32(dtEmpleados.Rows[posicion]["EstadoE"]) - 1;
        }
    }
}
