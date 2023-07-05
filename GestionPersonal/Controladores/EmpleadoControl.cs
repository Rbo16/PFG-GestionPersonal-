using GestionPersonal.Controladores;
using GestionPersonal.Utiles;
using GestionPersonal.Vistas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace GestionPersonal
{
    public class EmpleadoControl : Controlador
    {
        DataTable dtEmpleadosCif = new DataTable();

        public EmpleadoControl(VentanaControlador ventanaControl) : base(ventanaControl)
        {
            cargarEmpleados();
            ventanaActiva = new Empleados(this);
            ventanaActiva.Show();
        }

        /// <summary>
        /// Llama a la clase Listar para obtener un DataTable con los empleados del sistema, lo asigna al DataTable
        /// principal del controlador y le quita la contraseña.
        /// </summary>
        private void cargarEmpleados()
        {
            dtEmpleadosCif = Listar.listarEmpleados();
            dtEmpleadosCif.Columns.Remove("Contrasenia");
        }


        /// <summary>
        /// Devuelve el DataTable de Empleados sin filtro.
        /// </summary>
        /// <returns></returns>
        public DataTable listaEmpleados() 
        {
            cargarEmpleados();
            return dtEmpleadosCif;
        }

        /// <summary>
        /// Devuelve un DataTable que filtra el DataTable principal de Empleados a partir de la clase Listar.
        /// </summary>
        /// <param name="filtro">string con el filtro que se quiere aplicar</param>
        /// <returns></returns>
        public DataTable listaEmpleados(string filtro)
        {
            return Listar.filtrarTabla(dtEmpleadosCif, filtro);
        }

        /// <summary>
        /// Comprueba que los campos necesarios para la creación de un empleado estén completos y correctos, para
        /// depués llamar al Modelo y que este lo inserte en la BBDD.Es aquí donde de crea la contraseña, se cifra,
        /// y se llama a la clase EnviarMail para que se la envíe al nuevo empleado via mail.
        /// </summary>
        /// <param name="NombreE">Nombre del empleado.</param>
        /// <param name="Apellido">Apellido del empleado.</param>
        /// <param name="Usuario">Udsuario del empleado.</param>
        /// <param name="DNI">DNI del empleado.</param>
        /// <param name="NumSS">Número de la Seguridad Social del empleado.</param>
        /// <param name="Tlf">Teléfono de contacto del empleado.</param>
        /// <param name="CorreoE">Correo electrónico del empleado.</param>
        /// <returns></returns>
        public bool crearEmpleado(string NombreE, string Apellido, string Usuario, string DNI, 
            string NumSS, string Tlf, string CorreoE)
        {
            bool creado = false;

            List<string> listaCampos = new List<string>();
            //Guardamos los campos para comprobar que no estén vacíos

            listaCampos.Add(NombreE);
            listaCampos.Add(Apellido);
            listaCampos.Add(Usuario);
            listaCampos.Add(DNI);
            listaCampos.Add(NumSS);
            listaCampos.Add(Tlf);
            listaCampos.Add(CorreoE);

            if (!camposVacios(listaCampos))
            {
                if (comprobarCaracteres(DNI,NumSS))
                { 
                    Empleado nuevoEmpleado = new Empleado(0)
                    {
                        NombreE = NombreE,
                        Apellido = Apellido,
                        Usuario = Usuario,
                        Contrasenia = Password.Generate(12, 4),
                        DNI = DNI,
                        NumSS = NumSS,
                        Tlf = Tlf,
                        CorreoE = CorreoE,
                    };

                    if (nuevoEmpleado.existeDNI())
                    {
                        MessageBox.Show("Ya existe un empleado con ese DNI.");
                    }
                    else if (nuevoEmpleado.existeNumSS())
                    {
                        MessageBox.Show("Ya existe un empleado con ese Número de Seguridad Social.");
                    }
                    else if (nuevoEmpleado.existeUsuario())
                    {
                        MessageBox.Show("Ya existe un empleado con ese Usuario.");
                    }
                    else if (nuevoEmpleado.existeCorreo())
                    {
                        MessageBox.Show("Ya existe un empleado con ese Correo Electrónico.");
                    }
                    else
                    {
                        EnviarMail.nuevoEmpleado(CorreoE, Usuario, nuevoEmpleado.Contrasenia);

                        nuevoEmpleado.Contrasenia = ConvertidorHASH.GetHashString(nuevoEmpleado.Contrasenia);

                        nuevoEmpleado.insertEmpleado(this.Usuario.IdEmpleado);

                        creado = true;

                        MessageBox.Show("Empleado creado correctamente.");
                    }
                }   
            }
            else
            {
                MessageBox.Show("Rellene todos los campos necesarios.");
            }

            return creado;
        }

        /// <summary>
        /// Convierte el string SIdEmpleado a int para proposcionárselo al modelo Empleado y que este lo elimine del
        /// sistema.
        /// </summary>
        /// <param name="SIdEmpleado"></param>
        public void eliminarEmpleado(string SIdEmpleado)
        {
            int.TryParse(SIdEmpleado, out int IdEmpelado);
            Empleado empleadoBorrado = new Empleado(IdEmpelado);
            empleadoBorrado.deleteEmpleado(this.Usuario.IdEmpleado);
            MessageBox.Show("Empleado eliminado correctamente.");
        }

        /// <summary>
        /// Comprueba que los datos del empleado están completos y correctos para llamar al modelo Empleado y que este
        /// actualice los cambios en la BBDD.
        /// </summary>
        /// <param name="empleadoModif">DataRow con los datos del empleado a modificar</param>
        public void modificarEmpleado(DataRow empleadoModif)
        {
            List<string> lCampos = new List<string>();

            for(int i = 1; i < empleadoModif.Table.Rows.Count; i++)
            {
                lCampos.Add(empleadoModif[i].ToString());
            }
            if(!camposVacios(lCampos)) 
            {
                int.TryParse(empleadoModif["IdEmpleado"].ToString(), out int IdEmpleado);
                string NombreE = empleadoModif["NombreE"].ToString();
                string Apellido = empleadoModif["Apellido"].ToString();
                string Usuario = empleadoModif["Usuario"].ToString();
                TipoEmpleado.TryParse(empleadoModif["Rol"].ToString(), out TipoEmpleado rol);
                EstadoEmpleado.TryParse(empleadoModif["EstadoE"].ToString(), out EstadoEmpleado EstadoE);
                string DNI = empleadoModif["DNI"].ToString();
                string NumSS = empleadoModif["NumSS"].ToString();
                string Tlf = empleadoModif["Tlf"].ToString();
                string CorreoE = empleadoModif["CorreoE"].ToString();

                if (comprobarCaracteres(DNI, NumSS))
                {
                    Empleado empleadoModificado = new Empleado(IdEmpleado)
                    {
                        NombreE = NombreE,
                        Apellido = Apellido,
                        Usuario = Usuario,
                        rol = rol,
                        EstadoE = EstadoE,
                        DNI = DNI,
                        NumSS = NumSS,
                        Tlf = Tlf,
                        CorreoE = CorreoE
                    };

                    empleadoModificado.updateEmpleado(this.Usuario.IdEmpleado);

                    MessageBox.Show("Cambios guardados correctamente.");
                }
            }
            else
            {
                MessageBox.Show("Rellene todos los campos necesarios");
            }
        }

        /// <summary>
        /// Convierte los parámetros a su tipo adecuado para proporcionárselos al modelo Empleado y que este realice
        /// las acciones necesarias en la BBDD respecto al cambio de estado de un empleado. También invoca al método 
        /// que informa de dicha actualización.
        /// </summary>
        /// <param name="SIdEmpleado">String del id del empleado cuyo estado ha sido modificado.</param>
        /// <param name="SEstadoE">String del nuevo estado del empleado.</param>
        public void gestionarEmpleado(string SIdEmpleado, string SEstadoE)
        {
            int EstadoE = Convert.ToInt32(SEstadoE);
            int IdEmpleado = Convert.ToInt32(SIdEmpleado);

            Empleado empleadoGestion = new Empleado(IdEmpleado)
            {
                EstadoE = (EstadoEmpleado)EstadoE,
            };

            empleadoGestion.updateEstado(this.Usuario.IdEmpleado);

            informarAutorizacion(IdEmpleado, SEstadoE);
        }

        /// <summary>
        /// Obtiene el mail del empleado cuyo estado ha sido actualizado y llama a la clase Enviarmail para informarle
        /// de dicho cambio.
        /// </summary>
        /// <param name="IdEmpleado">id del empleado cuyo estado ha sido modificado </param>
        /// <param name="EstadoE">Nuevo estado del usuario</param>
        private void informarAutorizacion(int IdEmpleado, string EstadoE)
        {
            string correo = EnviarMail.obtenerMail(IdEmpleado);
            EnviarMail.altaEmpleado(correo, EstadoE);
        }

        /// <summary>
        /// Comprueba que el DNI indicado tenga 9 caracteres y el NumSS tenga 12.
        /// </summary>
        /// <param name="DNI">DNI</param>
        /// <param name="NumSS">Número de la Seguridad Social</param>
        /// <returns></returns>
        private bool comprobarCaracteres(string DNI, string NumSS)
        {
            bool result = true;

            if (DNI.Trim().Length != 9)
            {
                result = false;
                MessageBox.Show("El DNI ha de tener 9 caracteres.");//HAZ UN METODO PARA USARLO EN UPDATE
            }
            else if (NumSS.Trim().Length != 12)
            {
                result = false;
                MessageBox.Show("El Número de la Seguridad Social ha de tener 12 caracteres.");
            }

            return result;
        }

        /// <summary>
        /// Llama al controlador de ventanas para que bloquee la ventana activa, mientras que invoca un contructor del
        /// controlador de la ventana FiltroEmpleado para abrir una de estas.
        /// </summary>
        public void prepararFiltro()
        {
            this.filtro = string.Empty;
            ventanaControl.bloquearVActual();
            FiltroEmpleadoControl controladorFiltroE = new FiltroEmpleadoControl(this);
        }


    }
}
