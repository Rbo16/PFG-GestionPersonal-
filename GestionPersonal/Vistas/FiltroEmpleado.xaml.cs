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

        /// <summary>
        /// Carga como vacío el contenido de la lista que contendrá los filtros de cada elemento por el que se
        /// puede filtrar y carga otra lista con los nombres del campo que filtra cada uno de esos elementos.
        /// </summary>
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

        /// <summary>
        /// Comprueba que se haya especificado al menos un campo, forma la cadena que servirá como filtro
        /// y se la asigna al controlador. Después, cierra la ventana.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Al cerrar la ventana, invoca al controlador para que lo gestione.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            controladorFiltroE.volver();
        }

        /// <summary>
        /// Guarda el contenido del TextBox en la lista de contenidos del filtro cada vez que se actualiza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbNombreE_TextChanged(object sender, TextChangedEventArgs e)
        {
            contenidoFiltro[0] = txbNombreE.Text;
        }

        /// <summary>
        /// Guarda el contenido del TextBox en la lista de contenidos del filtro cada vez que se actualiza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbApellido_TextChanged(object sender, TextChangedEventArgs e)
        {
            contenidoFiltro[1] = txbApellido.Text;
        }

        /// <summary>
        /// Guarda el contenido del TextBox en la lista de contenidos del filtro cada vez que se actualiza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbDNI_TextChanged(object sender, TextChangedEventArgs e)
        {
            contenidoFiltro[2] = txbDNI.Text;
        }

        /// <summary>
        /// Guarda el contenido del TextBox en la lista de contenidos del filtro cada vez que se actualiza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbUsuario_TextChanged(object sender, TextChangedEventArgs e)
        {
            contenidoFiltro[3] = txbUsuario.Text;
        }

        /// <summary>
        /// Guarda el contenido del ComboBox en la lista de contenidos del filtro cada vez que se actualiza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbEstadoE_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            contenidoFiltro[4] = cmbEstadoE.SelectedValue.ToString();
        }

        /// <summary>
        /// Guarda el contenido del ComboBox en la lista de contenidos del filtro cada vez que se actualiza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDepartamento_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            contenidoFiltro[5] = cmbDepartamento.SelectedValue.ToString();
        }

        /// <summary>
        /// Guarda el contenido del ComboBox en la lista de contenidos del filtro cada vez que se actualiza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbRol_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            contenidoFiltro[6] = cmbRol.SelectedValue.ToString();
        }

        private void cmbRol_Loaded(object sender, RoutedEventArgs e)
        {
            cmbRol.ItemsSource = Enum.GetValues(typeof(TipoEmpleado));
        }

        private void cmbEstadoE_Loaded(object sender, RoutedEventArgs e)
        {
            cmbEstadoE.ItemsSource = Enum.GetValues(typeof(EstadoEmpleado));
        }
    }
}
