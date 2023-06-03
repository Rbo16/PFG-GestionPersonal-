using GestionPersonal.Controladores;
using GestionPersonal.Vistas;
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
        private readonly MenuControl controladorMenu;

        public Menu(MenuControl controladorMenu)
        {
            this.controladorMenu = controladorMenu;
            InitializeComponent();
            cargarRol();
        }

        private void cargarRol()
        {
            if (controladorMenu.Usuario.rol != TipoEmpleado.Basico)
            {
                btnAuditorias.Visibility = Visibility.Visible;
                btnEmpleados.Visibility = Visibility.Visible;
                btnDepartamentos.Visibility = Visibility.Visible;
            }
        }

        private void btnEmpleados_Click(object sender, RoutedEventArgs e)
        {
            this.controladorMenu.abrirEmpleados();
        }

        private void btnAusencias_Click(object sender, RoutedEventArgs e)
        {
            this.controladorMenu.abrirAusencias();
        }

        private void btnProyectos_Click(object sender, RoutedEventArgs e)
        {
            this.controladorMenu.abrirProyectos();
        }

        private void btnDepartamentos_Click(object sender, RoutedEventArgs e)
        {
            this.controladorMenu.abrirDepartamentos();
        }

        private void btnContratos_Click(object sender, RoutedEventArgs e)
        {
            this.controladorMenu.abrirContratos();
        }
    }
}
