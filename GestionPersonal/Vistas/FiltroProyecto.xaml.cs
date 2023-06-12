using GestionPersonal.Controladores.Filtros;
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
    /// Lógica de interacción para FiltroProyecto.xaml
    /// </summary>
    public partial class FiltroProyecto : Window
    {
        private FiltroProyectoControl controladorFiltroP;

        public FiltroProyecto(FiltroProyectoControl controladorFiltroP)
        {
            this.controladorFiltroP = controladorFiltroP;
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            controladorFiltroP.volver();
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
