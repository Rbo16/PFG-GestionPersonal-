using GestionPersonal.Utiles;
using GestionPersonal.Vistas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestionPersonal.Controladores
{
    public class RecuperacionControl : Controlador
    {
        public RecuperacionControl(VentanaControlador ventanaControl) : base(ventanaControl)
        {
            Recuperacion ventanaRecuperacion = new Recuperacion(this);
            ventanaRecuperacion.Show();
        }

        /// <summary>
        /// Genera una nueva contraseña para el empleado que tenga el correo indicado, si este existe, llama al 
        /// modelo Empleado para que actualice la contraseña en el sistema y llama a la clase EnviarMail para que
        /// esta informe de las nuevas credenciales al usuario via mail.
        /// </summary>
        /// <param name="correo"></param>
        /// <returns></returns>
        public bool enviarContraseña(string correo)
        {
            bool exito = false;
            string usuario = Querys.existeCorreo(correo);

            if (usuario != string.Empty) 
            {
                Empleado empleadoRecuperacion = new Empleado(0)
                {
                    CorreoE = correo,
                    Contrasenia = Password.Generate(12, 4)
                };

                EnviarMail.recuperarContrasenia(correo, usuario, empleadoRecuperacion.Contrasenia);

                empleadoRecuperacion.Contrasenia = ConvertidorHASH.GetHashString(empleadoRecuperacion.Contrasenia);

                empleadoRecuperacion.updateContrasenia();

                exito = true;
            }
            else
            {
                MessageBox.Show("No hay constancia de la existencia del correo indicado en el sistema.");
            }

            return exito;
        }
    }
}
