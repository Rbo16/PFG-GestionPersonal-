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

        /// <summary>
        /// Método para que el controlador de ventanas genere la ventana de menú principal
        /// </summary>
        public void volverMenu()
        {
            ventanaControl.ventanaMenu();
        }

        /// <summary>
        /// Comprueba que ningún elemento de una lista de strings tenga valor string.Empty 
        /// </summary>
        /// <param name="listaCampos">Lista con los campos a comrpobar</param>
        /// <returns></returns>
        public bool camposVacios(List<string> listaCampos)
        {
            bool vacio = false;

            for (int i = 0; i < listaCampos.Count(); i++)
            {
                if (listaCampos[i].Equals(string.Empty))
                    vacio = true;
            }

            return vacio;
        }
    }
}
