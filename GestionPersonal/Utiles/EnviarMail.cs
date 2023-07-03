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

        public static async void altaAusencia(string mailTo, string razon, string FechaInicioA, string FechaFinA, string EstadoA)
        {
            // Configuración del cliente SMTP
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(mail, pass);


            // Crear el mensaje de correo
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("tfg.demorob@gmail.com");
            mailMessage.To.Add(mailTo);
            mailMessage.Subject = "Resolución ausencia";
            mailMessage.Body = "Su ausencia con motivo: " + razon + " con fechas " + FechaInicioA + " - " + FechaFinA +
                " ha sido " + EstadoA + ".";

            try
            {
                // Enviar el correo
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el correo: " + ex.Message);
            }
            finally
            {
                // Clean up.
                mailMessage.Dispose();
            }
        }

        public static async void solicitudAusencia(string mailTo)
        {
            // Configuración del cliente SMTP
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(mail, pass);


            // Crear el mensaje de correo
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("tfg.demorob@gmail.com");
            mailMessage.To.Add(mailTo);
            mailMessage.Subject = "Nueva ausencia";
            mailMessage.Body = "Se ha solicitado una ausencia nueva. Consulte el apartado de ausencias para gestionarla.";

            try
            {
                // Enviar el correo
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el correo: " + ex.Message);
            }
            finally
            {
                // Clean up.
                mailMessage.Dispose();
            }
        }

        public static async void retiroParticipacion(string mailTo, string proyecto)
        {
            // Configuración del cliente SMTP
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(mail, pass);


            // Crear el mensaje de correo
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("tfg.demorob@gmail.com");
            mailMessage.To.Add(mailTo);
            mailMessage.Subject = "Retirada de proyecto";
            mailMessage.Body = "Ha sido retirado del proyecto " + proyecto;

            try
            {
                // Enviar el correo
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el correo: " + ex.Message);
            }
            finally
            {
                // Clean up.
                mailMessage.Dispose();
            }
        }

        public static async void nuevaParticipacion(string mailTo, string proyecto)
        {
            // Configuración del cliente SMTP
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(mail, pass);


            // Crear el mensaje de correo
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("tfg.demorob@gmail.com");
            mailMessage.To.Add(mailTo);
            mailMessage.Subject = "Adición a proyecto";
            mailMessage.Body = "Ha sido añadido al proyecto " + proyecto;

            try
            {
                // Enviar el correo
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el correo: " + ex.Message);
            }
            finally
            {
                // Clean up.
                mailMessage.Dispose();
            }
        }

        public static async void nuevoJefe(string mailTo, string departamento)
        {
            // Configuración del cliente SMTP
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(mail, pass);


            // Crear el mensaje de correo
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("tfg.demorob@gmail.com");
            mailMessage.To.Add(mailTo);
            mailMessage.Subject = "Actualización jefe departamento";
            mailMessage.Body = "Ha sido asignado como jefe del departamento " + departamento;

            try
            {
                // Enviar el correo
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el correo: " + ex.Message);
            }
            finally
            {
                // Clean up.
                mailMessage.Dispose();
            }
        }


        public static async void nuevoDepartamento(string mailTo, string departamento)
        {
            // Configuración del cliente SMTP
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(mail, pass);


            // Crear el mensaje de correo
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("tfg.demorob@gmail.com");
            mailMessage.To.Add(mailTo);
            mailMessage.Subject = "Adición a departamento";
            mailMessage.Body = "Ha sido añadido al departamento " + departamento;

            try
            {
                // Enviar el correo
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el correo: " + ex.Message);
            }
            finally
            {
                // Clean up.
                mailMessage.Dispose();
            }
        }

        public static async void altaContrato(string mailTo)
        {
            // Configuración del cliente SMTP
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(mail, pass);


            // Crear el mensaje de correo
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("tfg.demorob@gmail.com");
            mailMessage.To.Add(mailTo);
            mailMessage.Subject = "Alta contrato";
            mailMessage.Body = "Se ha dado de alta un nuevo contrato para usted.";

            try
            {
                // Enviar el correo
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el correo: " + ex.Message);
            }
            finally
            {
                // Clean up.
                mailMessage.Dispose();
            }
        }

        public static async void auditoriaContrato(string mailTo, string tipoAuditoria, string FechaInicioA, string FechaFinA)
        {
            // Configuración del cliente SMTP
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(mail, pass);


            // Crear el mensaje de correo
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("tfg.demorob@gmail.com");
            mailMessage.To.Add(mailTo);
            mailMessage.Subject = "Modificación/Borrado contrato";
            mailMessage.Body = "Se ha producido un" + tipoAuditoria + " en su contrato con fechas " + FechaInicioA + " - " + FechaFinA +".";

            try
            {
                // Enviar el correo
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el correo: " + ex.Message);
            }
            finally
            {
                // Clean up.
                mailMessage.Dispose();
            }
        }

        public static async void altaEmpleado(string mailTo, string EstadoE)
        {
            // Configuración del cliente SMTP
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(mail, pass);


            // Crear el mensaje de correo
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("tfg.demorob@gmail.com");
            mailMessage.To.Add(mailTo);
            mailMessage.Subject = "Sistema Empresa";
            mailMessage.Body = "Se ha resuelto su estado de acceso al sistema como : " + EstadoE;

            try
            {
                // Enviar el correo
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el correo: " + ex.Message);
            }
            finally
            {
                // Clean up.
                mailMessage.Dispose();
            }
        }
    }
}
