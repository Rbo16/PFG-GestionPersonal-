using GestionPersonal.Vistas;
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
        Window ventanaAnterior;

        public VentanaControlador()
        {
            LoginControlador loginControl = new LoginControlador(this);
            this.ventanaActual = loginControl.devolverVActiva();
            this.ventanaAnterior = ventanaActual;
        }

        public void ventanaMenu()
        {
            MenuControl controladorMenu = new MenuControl(this);
            this.ventanaActual = controladorMenu.devolverVActiva();
            this.ventanaAnterior = ventanaActual;
        }
        public void ventanaEmpleados()
        {
            EmpleadoControl controladorEmpleado = new EmpleadoControl(this);
            this.ventanaActual = controladorEmpleado.devolverVActiva();
            this.ventanaAnterior = ventanaActual;
        }
        public void ventanaAusencias()
        {
            AusenciaControl controladorAusencia = new AusenciaControl(this);
            this.ventanaActual = controladorAusencia.devolverVActiva();
            this.ventanaAnterior = ventanaActual;
        }
        public void ventanaContratos()
        {
            ContratoControl controladorContrato = new ContratoControl(this);
            this.ventanaActual = controladorContrato.devolverVActiva();
            this.ventanaAnterior = ventanaActual;
        }
        public void ventanaProyectos()
        {
            ProyectoControl controladorProyecto = new ProyectoControl(this);
            this.ventanaActual = controladorProyecto.devolverVActiva();
            this.ventanaAnterior = ventanaActual;
        }
        public void ventanaDepartamentos()
        {
            DepartamentoControl controladorPDepartamento = new DepartamentoControl(this);
            this.ventanaActual = controladorPDepartamento.devolverVActiva();
            this.ventanaAnterior = ventanaActual;
        }
    }
}
