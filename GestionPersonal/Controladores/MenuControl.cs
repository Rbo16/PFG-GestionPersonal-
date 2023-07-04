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

        public void abrirEmpleados()
        {
            ventanaControl.ventanaEmpleados();
        }
        public void abrirAusencias()
        {
            ventanaControl.ventanaAusencias();
        }
        public void abrirContratos()
        {
            ventanaControl.ventanaContratos();
        }
        public void abrirProyectos()
        {
            ventanaControl.ventanaProyectos();
        }
        public void abrirDepartamentos()
        {
            ventanaControl.ventanaDepartamentos();
        }
        public void abrirAuditorias()
        {
            ventanaControl.ventanaAuditorias();
        }
        public void logout()
        {
            ventanaControl.logout();
        }
        public void abrirPerfil()
        {
            ventanaControl.ventanaPerfil();
        }
    }
}
