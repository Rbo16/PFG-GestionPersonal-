﻿using GestionPersonal.Vistas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GestionPersonal.Controladores
{
    public class VentanaControlador
    {
        public Empleado Usuario { get; set; }

        Window ventanaActual;

        public VentanaControlador()
        {
            LoginControlador loginControl = new LoginControlador(this);
            this.ventanaActual = loginControl.devolverVActiva();
        }

        public void ventanaMenu()
        {
            MenuControl controladorMenu = new MenuControl(this);
            ventanaActual.Close();
            this.ventanaActual = controladorMenu.devolverVActiva();
        }
        public void ventanaEmpleados()
        {
            EmpleadoControl controladorEmpleado = new EmpleadoControl(this);
            ventanaActual.Close();
            this.ventanaActual = controladorEmpleado.devolverVActiva();
        }
        public void ventanaAusencias()
        {
            AusenciaControl controladorAusencia = new AusenciaControl(this);
            ventanaActual.Close();
            this.ventanaActual = controladorAusencia.devolverVActiva();
        }
        public void ventanaContratos()
        {
            ContratoControl controladorContrato = new ContratoControl(this);
            ventanaActual.Close();
            this.ventanaActual = controladorContrato.devolverVActiva();
        }
        public void ventanaProyectos()
        {
            ProyectoControl controladorProyecto = new ProyectoControl(this);
            ventanaActual.Close();
            this.ventanaActual = controladorProyecto.devolverVActiva();
        }
        public void ventanaDepartamentos()
        {
            DepartamentoControl controladorPDepartamento = new DepartamentoControl(this);
            ventanaActual.Close();
            this.ventanaActual = controladorPDepartamento.devolverVActiva();
        }

        private void cerrarVentanaAnt()
        {

        }
    }
}