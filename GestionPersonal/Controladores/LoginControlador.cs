using GestionPersonal.Utiles;
using GestionPersonal.Vistas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace GestionPersonal.Controladores
{
    public class LoginControlador : Controlador
    {
        int intento = 0;
        string usuarioIntento;
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
                if(usuarioIntento != usuario)
                {
                    intento = 0;
                }

                if (intento == 0)
                {
                    usuarioIntento = usuario;
                    intento++;
                }

                Usuario = new Empleado(0)
                {
                    Usuario = usuario,
                    Contrasenia= ConvertidorHASH.GetHashString(contrasenia),
                };
                if (Usuario.iniciarSesion())
                {
                    ventanaControl.Usuario = Usuario;
                    ventanaControl.ventanaMenu();
                    intento = 0;
                }
                else
                {
                    if(intento == 3)
                    {
                        Querys.bloquearUsuario(usuarioIntento);
                        MessageBox.Show("El límite de intentos ha sido excedido, contacte a un administrador para que" +
                            "le autorice de nuevo el acceso al sistema.");
                    }
                    else
                    {
                         intento++;
                         MessageBox.Show("El usuario o la contraseña son erroneos.");
                    }
                    
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
