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
    /// Lógica de interacción para Proyectos.xaml
    /// </summary>
    public partial class Proyectos : Window
    {
        private readonly ProyectoControl controladorProyecto;

        public Proyectos(ProyectoControl controladorProyecto)
        {
            this.controladorProyecto = controladorProyecto;
            InitializeComponent();
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            controladorProyecto.volverMenu();
        }
    }
}
