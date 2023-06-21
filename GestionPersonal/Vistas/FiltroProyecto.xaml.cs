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
    /// Lógica de interacción para FiltroProyecto.xaml
    /// </summary>
    public partial class FiltroProyecto : Window
    {
        private FiltroProyectoControl controladorFiltroP;
        private string[] contenidoFiltro = new string[5];

        public FiltroProyecto(FiltroProyectoControl controladorFiltroP)
        {
            this.controladorFiltroP = controladorFiltroP;
            InitializeComponent();
            cargarListas();
        }

        private void cmbEstadoP_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> lEstadoS = new List<string>();

            lEstadoS.Add("Próximo");
            lEstadoS.Add("En curso");
            lEstadoS.Add("Finalizado");

            cmbEstadoP.ItemsSource = lEstadoS;
        }

        private void cargarListas()
        {
            contenidoFiltro[0] = "";
            contenidoFiltro[1] = "";
            contenidoFiltro[2] = "";
            contenidoFiltro[3] = "";
            contenidoFiltro[4] = "";
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            string filtro = string.Empty;

            if (contenidoFiltro[0].Trim() != "")
                filtro += "NombreP like '%" + contenidoFiltro[0] + "%' AND ";

            if (contenidoFiltro[1] != "")
                filtro += "Cliente like '%" + contenidoFiltro[1] + "%' AND ";

            if (contenidoFiltro[2] != "")
            {
                string fechaActual = DateTime.Now.ToShortDateString();
                if (contenidoFiltro[2] == "En curso")
                {
                    filtro += $"FechaFinP > '{fechaActual}' AND FechaInicioP < '{fechaActual}' AND ";
                }
                else if (contenidoFiltro[2] == "Finalizado")
                {
                    filtro += $"FechaFinP < '{fechaActual}' AND ";
                }
                else if (contenidoFiltro[2] == "Próximo")
                {
                    filtro += $"FechaInicioP > '{fechaActual}' AND ";
                }
            }

            if (contenidoFiltro[3] != "")
                filtro += $"FechaInicioP > '{contenidoFiltro[3]}' AND ";

            if (contenidoFiltro[4] != "")
                filtro += $"FechaFinP > '{contenidoFiltro[4]}' AND ";


            if (filtro == string.Empty)
            {
                MessageBox.Show("Debe especificar al menos un campo");
            }
            else
            {
                filtro = filtro.Remove(filtro.Length - 4);
                controladorFiltroP.asignarFiltro(filtro);
                this.Close();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            controladorFiltroP.volver();
        }

        private void txbProyecto_TextChanged(object sender, TextChangedEventArgs e)
        {
            contenidoFiltro[0] = txbProyecto.Text;
        }

        private void txbCiente_TextChanged(object sender, TextChangedEventArgs e)
        {
            contenidoFiltro[1] = txbCiente.Text;
        }

        private void cmbEstadoP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            contenidoFiltro[2] = cmbEstadoP.SelectedValue.ToString();
        }

        private void dtpFechaDesde_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            contenidoFiltro[3] = dtpFechaDesde.SelectedDate.ToString();
        }

        private void dtpFechaHasta_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            contenidoFiltro[4] = dtpFechaHasta.SelectedDate.ToString();
        }


    }
}
