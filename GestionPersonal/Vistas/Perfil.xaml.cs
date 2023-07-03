using GestionPersonal.Controladores;
using GestionPersonal.Utiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Lógica de interacción para Perfil.xaml
    /// </summary>
    public partial class Perfil : Window
    {
        PerfilControl controladorPerfil;

        public Perfil(PerfilControl controladorPerfil)
        {
            this.controladorPerfil = controladorPerfil;

            InitializeComponent();

            cargarUsuario();
        }

        private void cargarUsuario()
        {
            txbNombreE.Text = controladorPerfil.Usuario.NombreE;
            txbApellido.Text = controladorPerfil.Usuario.Apellido;
            txbUsuario.Text = controladorPerfil.Usuario.Usuario;
            txbDNI.Text = controladorPerfil.Usuario.DNI;
            txbNumSS.Text = controladorPerfil.Usuario.NumSS;
            txbTlf.Text = controladorPerfil.Usuario.Tlf;
            txbCorreoE.Text = controladorPerfil.Usuario.CorreoE;
            txbDepartamento.Text = controladorPerfil.nombreDepartamento();
            txbRol.Text = ((TipoEmpleado)controladorPerfil.Usuario.rol).ToString();
        }
    }
}
