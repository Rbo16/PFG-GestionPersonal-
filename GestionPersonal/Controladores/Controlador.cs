using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GestionPersonal.Controladores
{
    public class Controlador
    {
        protected Window ventanaActiva;
        protected VentanaControlador ventanaControl;
        public Empleado Usuario { get; set; } = null; //Usuario manejando la app

        public Controlador(VentanaControlador ventanaControl) 
        { 
            this.ventanaControl = ventanaControl;
            this.Usuario= ventanaControl.Usuario;
        }
        public Window devolverVActiva()
        {
            if (ventanaActiva == null)
            {
                throw new Exception("ERROR[Controller.GetActiveWindow]: Ninguna ventanta ha sido configurada para el controlador - " + GetType());
            }
            return ventanaActiva;
        }
    }
}
