﻿using GestionPersonal.Controladores.Filtros;
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
    /// Lógica de interacción para FiltroContrato.xaml
    /// </summary>
    public partial class FiltroContrato : Window
    {
        FiltroContratoControl controladorFiltroC;

        private string[] contenidoFiltro = new string[4];

        public FiltroContrato(FiltroContratoControl controladorFiltroC)
        {
            this.controladorFiltroC= controladorFiltroC;
            InitializeComponent();

            cargarListas();

            comprobarRol();
        }

        /// <summary>
        /// Carga el ComboBox de Tipo Contrato.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTipoContrato_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> lTipoContrato= new List<string>();

            lTipoContrato.Add("Parcial Mañana");
            lTipoContrato.Add("Parcial Tarde");
            lTipoContrato.Add("Completa");
            lTipoContrato.Add("Prácticas");
            
            cmbTipoContrato.ItemsSource= lTipoContrato;
        }

        /// <summary>
        /// Comprueba el rol del usuario para rellenar el campo DNI con el suyo mismo en caso de que sea Básico.
        /// </summary>
        private void comprobarRol()
        {
            if (this.controladorFiltroC.controladorContrato.Usuario.rol == TipoEmpleado.Basico)
            {
                txbPoseedor.Text = this.controladorFiltroC.controladorContrato.Usuario.DNI;
                txbPoseedor.IsEnabled = false;
                contenidoFiltro[0] = txbPoseedor.Text;
            }
            else if(this.controladorFiltroC.controladorContrato.controladorBusqueda != null)
            {
                txbPoseedor.Text = this.controladorFiltroC.controladorContrato.controladorBusqueda.dniBusqueda;
                contenidoFiltro[0] = txbPoseedor.Text;
            }
        }

        /// <summary>
        /// Carga como vacío el contenido de la lista que contendrá los filtros de cada elemento por el que se
        /// puede filtrar.
        /// </summary>
        private void cargarListas()
        {
            contenidoFiltro[0] = "";
            contenidoFiltro[1] = "";
            contenidoFiltro[2] = "";
            contenidoFiltro[3] = "";
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

            if (contenidoFiltro[0].Trim() != "")
                filtro += "DNI like '%" + contenidoFiltro[0] + "%' AND ";

            if (contenidoFiltro[1] != "")
                filtro += "TipoContrato = " + contenidoFiltro[1] + " AND ";

            if (contenidoFiltro[2] != "")
                filtro += "FechaAlta > '" + contenidoFiltro[2] + "' AND ";

            if (contenidoFiltro[3] != "")
                filtro += "FechaBaja < '" + contenidoFiltro[3] + "' AND ";

            if (filtro == string.Empty)
            {
                MessageBox.Show("Debe especificar al menos un campo");
            }
            else
            {
                filtro = filtro.Remove(filtro.Length - 4);
                controladorFiltroC.asignarFiltro(filtro);
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
            controladorFiltroC.volver();
        }

        /// <summary>
        /// Guarda el contenido del TextBox en la lista de contenidos del filtro cada vez que se actualiza.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbPoseedor_TextChanged(object sender, TextChangedEventArgs e)
        {
            contenidoFiltro[0] = txbPoseedor.Text;
        }

        /// <summary>
        /// Guarda el contenido del ComboBox en la lista de contenidos del filtro cada vez que se actualiza.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTipoContrato_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            contenidoFiltro[1] = (cmbTipoContrato.SelectedIndex + 1).ToString();
        }

        /// <summary>
        /// Guarda el contenido del DatePicker en la lista de contenidos del filtro cada vez que se actualiza.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpFechaDesde_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            contenidoFiltro[2] = dtpFechaDesde.SelectedDate.ToString();
        }


        /// <summary>
        /// Guarda el contenido del DatePicker en la lista de contenidos del filtro cada vez que se actualiza.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpFechaHasta_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            contenidoFiltro[3] = dtpFechaHasta.SelectedDate.ToString();
        }


    }
}
