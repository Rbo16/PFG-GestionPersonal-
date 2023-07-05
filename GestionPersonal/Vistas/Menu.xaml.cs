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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MessageBox = System.Windows.Forms.MessageBox;

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

        /// <summary>
        /// Carga los elementos del menú en función del rol del usuario que maneja la aplicación.
        /// </summary>
        private void cargarRol()
        {
            if (controladorMenu.Usuario.rol != TipoEmpleado.Basico)
            {
                btnAuditorias.Visibility = Visibility.Visible;
                btnEmpleados.Visibility = Visibility.Visible;
                btnDepartamentos.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Llama al controlador para que abra la ventana Empleados.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEmpleados_Click(object sender, RoutedEventArgs e)
        {
            this.controladorMenu.abrirEmpleados();
        }

        /// <summary>
        /// Llama al controlador para que abra la ventana Ausencias.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAusencias_Click(object sender, RoutedEventArgs e)
        {
            this.controladorMenu.abrirAusencias();
        }

        /// <summary>
        /// Llama al controlador para que abra la ventana Proyectos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProyectos_Click(object sender, RoutedEventArgs e)
        {
            this.controladorMenu.abrirProyectos();
        }

        /// <summary>
        /// Llama al controlador para que abra la ventana Departamentos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDepartamentos_Click(object sender, RoutedEventArgs e)
        {
            this.controladorMenu.abrirDepartamentos();
        }

        /// <summary>
        /// Llama al controlador para que abra la ventana Contratos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnContratos_Click(object sender, RoutedEventArgs e)
        {
            this.controladorMenu.abrirContratos();
        }

        /// <summary>
        /// Llama al controlador para que abra la ventana Auditorias.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAuditorias_Click(object sender, RoutedEventArgs e)
        {
            this.controladorMenu.abrirAuditorias();
        }

        /// <summary>
        ///Llama al controlador para que haga el logout del usuario tras pedir confirmación.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            DialogResult dr = MessageBox.Show("¿Cerrar sesión?", "Logout", MessageBoxButtons.YesNo);
            if(dr == System.Windows.Forms.DialogResult.Yes)
            {
                this.controladorMenu.logout();
            }
        }

        /// <summary>
        /// Llama al controlador para que abra la ventana Empleados.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPerfil_Click(object sender, RoutedEventArgs e)
        {
            this.controladorMenu.abrirPerfil();
        }
    }
}
