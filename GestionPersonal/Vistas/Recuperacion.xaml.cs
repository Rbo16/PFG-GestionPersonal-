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
    /// Lógica de interacción para Recuperacion.xaml
    /// </summary>
    public partial class Recuperacion : Window
    {
        RecuperacionControl controladorRecuperacion;

        public Recuperacion(RecuperacionControl controladorRecuperacion)
        {
            this.controladorRecuperacion= controladorRecuperacion;
            InitializeComponent();
        }

        private void btnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            if(txbCorreo.Text.Trim() != string.Empty)
            {
                if (controladorRecuperacion.enviarContraseña(txbCorreo.Text))
                {
                    MessageBox.Show("Se ha enviado la nueva contraseña al correo indicado.");
                    this.Close();
                }
            }
        }
    }
}
