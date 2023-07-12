using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Configuration;

namespace GestionPersonal.Utiles
{
    public static class EnviarMail
    {
        private static SqlConnection conexionSQL;
        private static string cadenaConexion = ConfigurationManager.ConnectionStrings["GestionPersonal.Properties.Settings.masterConnectionString"].ConnectionString;

        static string mail = "tfg.demorob@gmail.com";
        static string pass = "euxhqfvgayiuleam";

        /// <summary>
        /// Obtiene de la BBDD el correo electrónico del empleado indicado.
        /// </summary>
        /// <param name="IdEmpleado">Id del empleado cuyo correo se desea obtener.</param>
        /// <returns>Correo electrónico del empleado en formato string.</returns>
        public static string obtenerMail(int IdEmpleado)
        {
            string correo = "";
            try
            {
                string consulta = "SELECT CorreoE FROM Empleado WHERE IdEmpleado = @IdEmpleado";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);
                comando.Parameters.Add("@IdEmpleado", SqlDbType.Int);
                comando.Parameters["@IdEmpleado"].Value = IdEmpleado;

                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    correo = reader.GetString(0);
                }
                return correo;
            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Empleado.ObtenerMail(ID)]");
                return string.Empty;
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// Obtiene de la BBDD el correo electrónico del empleado indicado.
        /// </summary>
        /// <param name="DNI">DNI del empleado del que se quiere obtener el correo.</param>
        /// <returns>Correo electrónico del empleado en formato string.</returns>
        public static string obtenerMail(string DNI)
        {
            string correo = "";
            try
            {
                string consulta = "SELECT CorreoE FROM Empleado WHERE DNI = @DNI";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);
                comando.Parameters.Add("@DNI", SqlDbType.NVarChar);
                comando.Parameters["@DNI"].Value = DNI;

                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    correo = reader.GetString(0);
                }
                return correo;
            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Empleado.ObtenerMail(DNI)]");
                return string.Empty;
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// Envía un mensaje al usuario cuyo correo se ha especificado informando de la resolución de
        /// la ausencia especificada
        /// </summary>
        /// <param name="mailTo">Correo destino.</param>
        /// <param name="razon">Motivo de la ausencia</param>
        /// <param name="FechaInicioA">Fecha inicio de la ausencia.</param>
        /// <param name="FechaFinA">FechaFin de la ausencia.</param>
        /// <param name="EstadoA">Resolución de la ausencia.</param>
        public static async void altaAusencia(string mailTo, string razon, string FechaInicioA, string FechaFinA, string EstadoA)
        {
             
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(mail, pass);


 
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("tfg.demorob@gmail.com");
            mailMessage.To.Add(mailTo);
            mailMessage.Subject = "Resolución ausencia";
            mailMessage.Body = "Su ausencia con motivo: " + razon + " con fechas " + FechaInicioA + " - " + FechaFinA +
                " ha sido " + EstadoA + ".";

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el correo: " + ex.Message);
            }
            finally
            {

                mailMessage.Dispose();
            }
        }

        /// <summary>
        /// Envía un mensaje al usuario cuyo correo se ha especificado informando de que hay una nueva ausencia en el sistema.
        /// </summary>
        /// <param name="mailTo">Correo destino.</param>
        public static async void solicitudAusencia(string mailTo)
        {
             
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(mail, pass);

 
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("tfg.demorob@gmail.com");
            mailMessage.To.Add(mailTo);
            mailMessage.Subject = "Nueva ausencia";
            mailMessage.Body = "Se ha solicitado una ausencia nueva. Consulte el apartado de ausencias para gestionarla.";

            try
            {
                
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el correo: " + ex.Message);
            }
            finally
            {

                mailMessage.Dispose();
            }
        }

        /// <summary>
        /// Envía un mensaje al usuario cuyo correo se ha especificado señalando su retiro  del proyecto indicado.
        /// </summary>
        /// <param name="mailTo">Correo destino.</param>
        /// <param name="proyecto">Nombre del proyecto.</param>
        public static async void retiroParticipacion(string mailTo, string proyecto)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(mail, pass);

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("tfg.demorob@gmail.com");
            mailMessage.To.Add(mailTo);
            mailMessage.Subject = "Retirada de proyecto";
            mailMessage.Body = "Ha sido retirado del proyecto " + proyecto;

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el correo: " + ex.Message);
            }
            finally
            {
                mailMessage.Dispose();
            }
        }

        /// <summary>
        /// Envía un mensaje al usuario cuyo correo se ha especificado señalando su adición al proyecto indicado.
        /// </summary>
        /// <param name="mailTo">Correo destino.</param>
        /// <param name="proyecto">Nombre del proyecto.</param>
        public static async void nuevaParticipacion(string mailTo, string proyecto)
        {
             
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(mail, pass);


 
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("tfg.demorob@gmail.com");
            mailMessage.To.Add(mailTo);
            mailMessage.Subject = "Adición a proyecto";
            mailMessage.Body = "Ha sido añadido al proyecto " + proyecto;

            try
            {
                
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el correo: " + ex.Message);
            }
            finally
            {

                mailMessage.Dispose();
            }
        }

        /// <summary>
        /// Envía un mensaje al usuario cuyo correo se ha especificado informando de su nuevo estatus de jefe del departamento indicado.
        /// </summary>
        /// <param name="mailTo">Correo destino.</param>
        /// <param name="departamento">Nombre del departamento.</param>
        public static async void nuevoJefe(string mailTo, string departamento)
        {
             
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(mail, pass);


 
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("tfg.demorob@gmail.com");
            mailMessage.To.Add(mailTo);
            mailMessage.Subject = "Actualización jefe departamento";
            mailMessage.Body = "Ha sido asignado como jefe del departamento " + departamento;

            try
            {
                
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el correo: " + ex.Message);
            }
            finally
            {

                mailMessage.Dispose();
            }
        }

        /// <summary>
        /// Envía un mensaje al usuario cuyo correo se ha especificado señalando su adición al departamento indicado.
        /// </summary>
        /// <param name="mailTo">Correo destino.</param>
        /// <param name="departamento">Nombre del departamento</param>
        public static async void nuevoDepartamento(string mailTo, string departamento)
        {
             
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(mail, pass);


 
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("tfg.demorob@gmail.com");
            mailMessage.To.Add(mailTo);
            mailMessage.Subject = "Adición a departamento";
            mailMessage.Body = "Ha sido añadido al departamento " + departamento;

            try
            {
                
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el correo: " + ex.Message);
            }
            finally
            {

                mailMessage.Dispose();
            }
        }

        /// <summary>
        /// Envía un mensaje al usuario cuyo correo se ha especificado indicado señalando el alta de un nuevo contrato.
        /// </summary>
        /// <param name="mailTo">Correo destino.</param>
        public static async void altaContrato(string mailTo)
        {
             
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(mail, pass);


 
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("tfg.demorob@gmail.com");
            mailMessage.To.Add(mailTo);
            mailMessage.Subject = "Alta contrato";
            mailMessage.Body = "Se ha dado de alta un nuevo contrato para usted.";

            try
            {
                
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el correo: " + ex.Message);
            }
            finally
            {

                mailMessage.Dispose();
            }
        }

        /// <summary>
        /// Envía un mensaje al usuario cuyo correo se ha especificado informando de que se han producido cambios
        /// en un contrato especificado.
        /// </summary>
        /// <param name="mailTo">Correo destino.</param>
        /// <param name="tipoAuditoria">Tipo de cambio realizado.</param>
        /// <param name="FechaAlta">Fecha del alta del contrato</param>
        /// <param name="FechaFin">Fecha de fin del contrato.</param>
        public static async void auditoriaContrato(string mailTo, string tipoAuditoria, string FechaAlta, string FechaFin)
        {
             
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(mail, pass);


 
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("tfg.demorob@gmail.com");
            mailMessage.To.Add(mailTo);
            mailMessage.Subject = "Modificación/Borrado contrato";
            mailMessage.Body = "Se ha producido un" + tipoAuditoria + " en su contrato con fechas " + FechaAlta 
                + " - " + FechaFin +".";

            try
            {
                
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el correo: " + ex.Message);
            }
            finally
            {

                mailMessage.Dispose();
            }
        }

        /// <summary>
        /// Envía un mensaje al usuario cuyo correo se ha especificado indicando sus credenciales de acceso al sistema.
        /// </summary>
        /// <param name="mailTo">Correo destino.</param>
        /// <param name="Usuario"></param>
        /// <param name="Contrasenia"></param>
        public static async void nuevoEmpleado(string mailTo, string Usuario, string Contrasenia)
        {
             
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(mail, pass);


 
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("tfg.demorob@gmail.com");
            mailMessage.To.Add(mailTo);
            mailMessage.Subject = "Credenciales";
            mailMessage.Body = "Se ha creado su usuario para acceder al sistema. \n\nUsuario: " + Usuario +
                "\nContraseña: "+ Contrasenia +"\n\nPróximamente le llegará un mail con informándole si ha sido autorizado o no.";

            try
            {
                
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el correo: " + ex.Message);
            }
            finally
            {

                mailMessage.Dispose();
            }
        }

        /// <summary>
        /// Envía un mensaje al usuario cuyo correo se ha especificado señalando una actualización en su estado 
        /// en el sistema.
        /// </summary>
        /// <param name="mailTo">Correo destino.</param>
        /// <param name="EstadoE">Nuevo estado del usuario.</param>
        public static async void altaEmpleado(string mailTo, string EstadoE)
        {
             
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(mail, pass);


 
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("tfg.demorob@gmail.com");
            mailMessage.To.Add(mailTo);
            mailMessage.Subject = "Sistema Empresa";
            mailMessage.Body = "Se ha resuelto su estado de acceso al sistema como : " + EstadoE;

            try
            {
                
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el correo: " + ex.Message);
            }
            finally
            {

                mailMessage.Dispose();
            }
        }

        /// <summary>
        /// Envía un mensaje al usuario cuyo correo se ha especificado indicando las nuevas credenciales 
        /// de acceso al sistema.
        /// </summary>
        /// <param name="mailTo">Correo destino.</param>
        /// <param name="Usuario"></param>
        /// <param name="Contrasenia"></param>
        public static async void recuperarContrasenia(string mailTo,string Usuario, string Contrasenia)
        {
             
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(mail, pass);


 
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("tfg.demorob@gmail.com");
            mailMessage.To.Add(mailTo);
            mailMessage.Subject = "Nueva contraseña";
            mailMessage.Body = "Sus nuevas credenciales son:\n\n\tUsuario: " + Usuario + "\n\tContraseña:" + Contrasenia;

            try
            {
                
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el correo: " + ex.Message);
            }
            finally
            {

                mailMessage.Dispose();
            }
        }

    }
}
