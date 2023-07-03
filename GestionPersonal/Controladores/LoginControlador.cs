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
        
        public void abrirRecuperacion()
        {
            ventanaControl.ventanaRecuperacion();
        }
    }
}
