using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void btnCambioContra_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEmpleados_Click(object sender, RoutedEventArgs e)
        {
            Empleados menuEmpleados = new Empleados();
            menuEmpleados.Show();
        }

        private void btnAusencias_Click(object sender, RoutedEventArgs e)
        {
            Ausencias menuAusencias= new Ausencias();
            menuAusencias.Show();
        }

        private void btnProyectos_Click(object sender, RoutedEventArgs e)
        {
            Proyectos menuProyectos = new Proyectos();
            menuProyectos.Show();
        }

        private void btnDepartamentos_Click(object sender, RoutedEventArgs e)
        {
            Departamentos menuDepartamentos = new Departamentos();
            menuDepartamentos.Show();
        }
    }
}
