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
    /// Lógica de interacción para Departamentos.xaml
    /// </summary>
    public partial class Departamentos : Window
    {
        private readonly DepartamentoControl controladorDepartamento;
        public Departamentos(DepartamentoControl controladorDepartamento)
        {
            this.controladorDepartamento = controladorDepartamento;
            InitializeComponent();
        }

        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            controladorDepartamento.volverMenu();
        }
    }
}
