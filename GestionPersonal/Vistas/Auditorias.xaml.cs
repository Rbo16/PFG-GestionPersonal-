using GestionPersonal.Controladores;
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

namespace GestionPersonal.Vistas
{
    /// <summary>
    /// Lógica de interacción para Auditorias.xaml
    /// </summary>
    public partial class Auditorias : Window
    {
        AuditoriaControl controladorAuditoria;

        public Auditorias(AuditoriaControl controladorAuditoria)
        {
            this.controladorAuditoria = controladorAuditoria;

            InitializeComponent();
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            string nombreTabla = ((Button)sender).Name.Substring(3);

            DataTable dtAuditoria = controladorAuditoria.listarAuditoria(nombreTabla);

            dtgAuditorias.ItemsSource = null;
            dtgAuditorias.ItemsSource = dtAuditoria.DefaultView;
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            controladorAuditoria.volverMenu();
        }
    }
}
