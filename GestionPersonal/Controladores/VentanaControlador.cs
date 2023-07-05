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

        public VentanaControlador()
        {
            LoginControlador loginControl = new LoginControlador(this);
            this.ventanaActual = loginControl.devolverVActiva();
        }

        /// <summary>
        /// Cierra la ventana activa actual y llama al controlador de la ventana Menú para abrir una de estas al
        /// mismo tiempo que la asigna como ventana activa.
        /// </summary>
        public void ventanaMenu()
        {
            MenuControl controladorMenu = new MenuControl(this);
            ventanaActual.Close();
            this.ventanaActual = controladorMenu.devolverVActiva();
        }

        /// <summary>
        /// Cierra la ventana activa actual y llama al controlador de la ventana Empleados para abrir una de estas al
        /// mismo tiempo que la asigna como ventana activa.
        /// </summary>
        public void ventanaEmpleados()
        {
            EmpleadoControl controladorEmpleado = new EmpleadoControl(this);
            ventanaActual.Close();
            this.ventanaActual = controladorEmpleado.devolverVActiva();
        }

        /// <summary>
        /// Cierra la ventana activa actual y llama al controlador de la ventana Ausencias para abrir una de estas al
        /// mismo tiempo que la asigna como ventana activa.
        /// </summary>
        public void ventanaAusencias()
        {
            AusenciaControl controladorAusencia = new AusenciaControl(this);
            ventanaActual.Close();
            this.ventanaActual = controladorAusencia.devolverVActiva();
        }

        /// <summary>
        /// Cierra la ventana activa actual y llama al controlador de la ventana Contratos para abrir una de estas al
        /// mismo tiempo que la asigna como ventana activa.
        /// </summary>
        public void ventanaContratos()
        {
            ContratoControl controladorContrato = new ContratoControl(this);
            ventanaActual.Close();
            this.ventanaActual = controladorContrato.devolverVActiva();
        }

        /// <summary>
        /// Cierra la ventana activa actual y llama al controlador de la ventana Proyectos para abrir una de estas al
        /// mismo tiempo que la asigna como ventana activa.
        /// </summary>
        public void ventanaProyectos()
        {
            ProyectoControl controladorProyecto = new ProyectoControl(this);
            ventanaActual.Close();
            this.ventanaActual = controladorProyecto.devolverVActiva();
        }

        /// <summary>
        /// Cierra la ventana activa actual y llama al controlador de la ventana Departamentos para abrir una de estas al
        /// mismo tiempo que la asigna como ventana activa.
        /// </summary>
        public void ventanaDepartamentos()
        {
            DepartamentoControl controladorDepartamento = new DepartamentoControl(this);
            ventanaActual.Close();
            this.ventanaActual = controladorDepartamento.devolverVActiva();
        }

        /// <summary>
        /// Cierra la ventana activa actual y llama al controlador de la ventana Auditorias para abrir una de estas al
        /// mismo tiempo que la asigna como ventana activa.
        /// </summary>
        public void ventanaAuditorias()
        {
            AuditoriaControl controladorAuditoria = new AuditoriaControl(this);
            ventanaActual.Close();
            this.ventanaActual = controladorAuditoria.devolverVActiva();
        }

        /// <summary>
        /// Cierra la ventana activa actual y llama al controlador de la ventana Login para abrir una de estas al
        /// mismo tiempo que la asigna como ventana activa.
        /// </summary>
        public void logout()
        {
            LoginControlador loginControl = new LoginControlador(this);
            ventanaActual.Close();
            this.ventanaActual = loginControl.devolverVActiva();
        }

        /// <summary>
        /// LLama al controlador de la ventana Perfil para que abra una de estas.
        /// </summary>
        public void ventanaPerfil()
        {
            PerfilControl controladorPerfil = new PerfilControl(this);
        }

        /// <summary>
        /// LLama al controlador de la ventana Perfil para que abra una de estas.
        /// </summary>
        public void ventanaRecuperacion()
        {
            RecuperacionControl controladorRecuperacion = new RecuperacionControl(this);
        }

        /// <summary>
        /// Bloquea la ventana activa actual.
        /// </summary>
        public void bloquearVActual()
        {
            ventanaActual.IsEnabled= false;
        }
        
        /// <summary>
        /// Desbloquea la ventana activa actual
        /// </summary>
        public void desbloquearVActual()
        {
            ventanaActual.IsEnabled= true;
            ventanaActual.Show();
        }
    }
}
