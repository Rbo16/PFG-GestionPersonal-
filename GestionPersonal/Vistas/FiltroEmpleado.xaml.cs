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
    /// Lógica de interacción para FiltroEmpleado.xaml
    /// </summary>
    public partial class FiltroEmpleado : Window
    {
        FiltroEmpleadoControl controladorFiltroE;

        private List<string> nombresFiltro = new List<string>();
        private string[] contenidoFiltro = new string[7];


        public FiltroEmpleado(FiltroEmpleadoControl controladorFiltroE)
        {
            this.controladorFiltroE= controladorFiltroE;
            InitializeComponent();
            cargarListas();
        }

        private void cargarListas() 
        {
            nombresFiltro.Add("NombreE");
            nombresFiltro.Add("Apellido");
            nombresFiltro.Add("DNI");
            nombresFiltro.Add("Usuario");
            nombresFiltro.Add("EstadoE");
            nombresFiltro.Add("NombreD");
            nombresFiltro.Add("Rol");

            contenidoFiltro[0] = "";
            contenidoFiltro[1] = "";
            contenidoFiltro[2] = "";
            contenidoFiltro[3] = "";
            contenidoFiltro[4] = "";
            contenidoFiltro[5] = "";
            contenidoFiltro[6] = "";
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            string filtro = string.Empty;

            for(int i = 0; i < nombresFiltro.Count; i++)
            {
                if (contenidoFiltro[i].Trim() != "")
                    filtro += nombresFiltro[i] + " like '%" + contenidoFiltro[i] + "%' AND ";
            }

            if (filtro == string.Empty)
            {
                MessageBox.Show("Debe especificar al menos un campo");
            }
            else
            {
                //filtro = "WHERE " + filtro;
                filtro = filtro.Remove(filtro.Length - 4);
                controladorFiltroE.asignarFiltro(filtro);
                this.Close();
            }
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            controladorFiltroE.volver();
        }

        private void txbNombreE_TextChanged(object sender, TextChangedEventArgs e)
        {
            contenidoFiltro[0] = txbNombreE.Text;
        }
        private void txbApellido_TextChanged(object sender, TextChangedEventArgs e)
        {
            contenidoFiltro[1] = txbApellido.Text;
        }

        private void txbDNI_TextChanged(object sender, TextChangedEventArgs e)
        {
            contenidoFiltro[2] = txbDNI.Text;
        }

        private void txbUsuario_TextChanged(object sender, TextChangedEventArgs e)
        {
            contenidoFiltro[3] = txbUsuario.Text;
        }

        private void cmbEstadoE_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            contenidoFiltro[4] = cmbEstadoE.SelectedValue.ToString();
        }

        private void cmbDepartamento_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            contenidoFiltro[5] = cmbDepartamento.SelectedValue.ToString();
        }

        private void cmbRol_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            contenidoFiltro[6] = cmbRol.SelectedValue.ToString();
        }



    }
}
