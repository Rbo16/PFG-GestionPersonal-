using GestionPersonal.Utiles;
using GestionPersonal.Vistas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace GestionPersonal.Controladores
{
    public class LoginControlador : Controlador
    {
        public LoginControlador(VentanaControlador ventanaControl) : base(ventanaControl)
        {
            ventanaActiva = new Login(this);
            ventanaActiva.Show();
        }

        /// <summary>
        /// Comprueba que los campos usuario y contraseña estén rellenos y, si lo están, llama al modelo Empleado
        /// para que compruebe las credenciales de inicio de sesión con la BBDD.
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="contrasenia"></param>
        public void iniciarSesion(string usuario, string contrasenia)
        {
            
            if (usuario == "")
            {
                MessageBox.Show("Introduzca un usuario");
            }
            else if (contrasenia == "")
            {
                MessageBox.Show("Introduzca la contraseña");
            }
            else
            {
                Usuario = new Empleado(0)
                {
                    Usuario = usuario,
                    Contrasenia= ConvertidorHASH.GetHashString(contrasenia),
                };
                if (Usuario.iniciarSesion())
                {
                    ventanaControl.Usuario = Usuario;
                    ventanaControl.ventanaMenu();
                }
                else
                {
                    MessageBox.Show("El usuario o la contraseña son erroneas.");//A lo mejor mételo en en makelogin y que indique cuál es incorrecto
                }
            }
        }
        
        /// <summary>
        /// Llama al controlador de ventanas para que abra la ventana de recuperación de contraseña.
        /// </summary>
        public void abrirRecuperacion()
        {
            ventanaControl.ventanaRecuperacion();
        }
    }
}
