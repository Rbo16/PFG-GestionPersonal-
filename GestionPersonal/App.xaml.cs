﻿using GestionPersonal.Controladores;
using GestionPersonal.Vistas;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GestionPersonal
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            VentanaControlador ventanaControlador = new VentanaControlador();
        }
    }
}
