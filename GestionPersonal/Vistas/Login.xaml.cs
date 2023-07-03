using GestionPersonal.Controladores;
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

namespace GestionPersonal.Vistas
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private readonly LoginControlador controladorLogin;

        public Login(LoginControlador controladorLogin)
        {
            InitializeComponent();
            this.controladorLogin = controladorLogin;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            controladorLogin.iniciarSesion(txbUsuario.Text, txbContraseña.Password);
        }

        private void btnCContra_Click(object sender, RoutedEventArgs e)
        {
            controladorLogin.abrirRecuperacion();
        }
    }
}
