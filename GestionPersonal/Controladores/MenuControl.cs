using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPersonal.Controladores
{
    public class MenuControl : Controlador
    {
        public MenuControl(VentanaControlador ventanaControl) : base(ventanaControl) 
        {
            ventanaActiva = new Menu(this);
            ventanaActiva.Show();
        }

        /// <summary>
        /// Llama al controlador de ventanas para que abra la ventana de empleados.
        /// </summary>
        public void abrirEmpleados()
        {
            ventanaControl.ventanaEmpleados();
        }

        /// <summary>
        /// Llama al controlador de ventanas para que abra la ventana de ausencias.
        /// </summary>
        public void abrirAusencias()
        {
            ventanaControl.ventanaAusencias();
        }

        /// <summary>
        /// Llama al controlador de ventanas para que abra la ventana de contratos.
        /// </summary>
        public void abrirContratos()
        {
            ventanaControl.ventanaContratos();
        }

        /// <summary>
        /// Llama al controlador de ventanas para que abra la ventana de proyectos.
        /// </summary>
        public void abrirProyectos()
        {
            ventanaControl.ventanaProyectos();
        }

        /// <summary>
        /// Llama al controlador de ventanas para que abra la ventana de departamentos.
        /// </summary>
        public void abrirDepartamentos()
        {
            ventanaControl.ventanaDepartamentos();
        }

        /// <summary>
        /// Llama al controlador de ventanas para que abra la ventana de auditorias.
        /// </summary>
        public void abrirAuditorias()
        {
            ventanaControl.ventanaAuditorias();
        }

        /// <summary>
        /// Llama al controlador de ventanas para que cierre la sesión actual.
        /// </summary>
        public void logout()
        {
            ventanaControl.logout();
        }

        /// <summary>
        /// Llama al controlador de ventanas para que abra la ventana de información del perfil actual.
        /// </summary>
        public void abrirPerfil()
        {
            ventanaControl.ventanaPerfil();
        }
    }
}
