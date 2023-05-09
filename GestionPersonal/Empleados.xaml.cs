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

        public Empleados()
        {
            InitializeComponent();
            cargarDTG();
        }

        private void cargarDTG()
        {
            dtEmpleados = ControladorE.listarEmpleados();
            dtgEmpleados.ItemsSource = dtEmpleados.DefaultView;
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            InteraccionBBDD miBBDD = new InteraccionBBDD();
            DataTable dtEmpleados = miBBDD.consultaSelect("SELECT * FROM Empleado");
            dtgEmpleados.ItemsSource = dtEmpleados.DefaultView;
        }
    }
}
