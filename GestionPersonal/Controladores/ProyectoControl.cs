using GestionPersonal.Controladores;
using GestionPersonal.Controladores.Filtros;
using GestionPersonal.Utiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GestionPersonal
{
    public class ProyectoControl : Controlador
    {
        DataTable dtProyectos;
        public BusquedaEmpleadoControlador controladorBusqueda;

        public ProyectoControl(VentanaControlador ventanaControl) : base(ventanaControl)
        {
            listaProyectos();
            ventanaActiva = new Proyectos(this);
            ventanaActiva.Show();
        }

        /// <summary>
        /// Carga la lista de Proyectos llamando a su método correspondiente de la clase Listar.
        /// </summary>
        private void cargarProyectos()
        {
            dtProyectos = Listar.listarProyectos();
        }

        /// <summary>
        /// Carga la lista de Empleados que participan en al menos un proyecto llamando a su método correspondiente de la clase Listar.
        /// </summary>
        private void cargarProyectosEmpleado()
        {
            dtProyectos = Listar.listarProyectosEmpleado();
        }

        /// <summary>
        /// Devuelve el DataTable de Proyectos sin filtro. Si el rol del usuario es básico, devuelve los proyectos
        /// los que participa el mismo. Si no, devuelve todas las participaciones de empleados en proyectos.
        /// </summary>
        /// <returns></returns>
        public DataTable listaProyectos()
        {
            if(Usuario.rol == TipoEmpleado.Basico)
            {
                cargarProyectosEmpleado();
            }
            else
            {
                cargarProyectos();
            }   
            return dtProyectos;
        }


        /// <summary>
        /// Devuelve un DataTable que filtra el DataTable principal de Proyectos mediante la clase Listar.
        /// </summary>
        /// <param name="filtro">string con el filtro que se quiere aplicar</param>
        /// <returns></returns>
        public DataTable listaProyectos(string filtro)
        {
            return Listar.filtrarTabla(dtProyectos, filtro);
        }




        /// <summary>
        /// Devuelve un DataTable con los Empleados que participan en el Proyecto indicado.
        /// </summary>
        /// <param name="SIdProyecto">Id del Proyecto en formato string</param>
        /// <returns></returns>
        public DataTable listaEmpleadosProyecto(string SIdProyecto)
        {
            int.TryParse(SIdProyecto, out int IdProyecto);
            string filtro = $" IdProyecto = {IdProyecto}";
            DataTable dtEmpleados = Listar.listarParticipacionProyectos();
            return Listar.filtrarTabla(dtEmpleados, filtro);
        }

        /// <summary>
        /// Comprueba que los campos necesarios para la creación de un proyecto estén completos y correctos y, si es
        /// así, llama al modelo Proyecto para que lo inserte en la BBDD.
        /// </summary>
        /// <param name="NombreP">Nombre del proyecto</param>
        /// <param name="Cliente">Cliente del proyecto</param>
        /// <param name="SFechaInicioP">string de la fecha de inicio del proyecto</param>
        /// <param name="SNTiempo">Número para indicar las unidades de tiempo destinadas al proyecto</param>
        /// <param name="Tiempo">Unidad de tiempo</param>
        /// <param name="SPresupuesto">Presupuesto dedicado al proyecto</param>
        /// <param name="SPrioridad">Prioridad del proyecto</param>
        /// <param name="DescripcionP">Descripción del proyecto</param>
        /// <returns></returns>
        public bool crearProyecto(string NombreP, string Cliente, string SFechaInicioP, string SNTiempo, string Tiempo, string SPresupuesto,
            string SPrioridad, string DescripcionP)
        {
            bool creado = true;

            List<string> lCampos = new List<string>();
            lCampos.Add(NombreP);
            lCampos.Add(Cliente);
            lCampos.Add(SFechaInicioP);
            
            lCampos.Add(Tiempo);
            if(Tiempo != "Indefinido")
                lCampos.Add(SNTiempo);

            lCampos.Add(SPresupuesto);
            lCampos.Add(DescripcionP);
            lCampos.Add(SPrioridad);

            if (!camposVacios(lCampos))
            {
                if(!DateTime.TryParse(SFechaInicioP, out DateTime FechaInicioP))
                    creado= false;//DatePicker
                else if(Tiempo != "Indefinido" && !int.TryParse(SNTiempo, out int NTiempo))
                {
                    creado = false;
                    MessageBox.Show("Introduzca el tiempo como un entero");
                }
                else if (!int.TryParse(SPresupuesto, out int Presupuesto))
                {
                    creado = false;
                    MessageBox.Show("Introduzca el presupuesto como un entero");
                }
                else
                {
                    if(Tiempo != "Indefinido")
                    {
                        Tiempo = SNTiempo + " " + Tiempo;
                    }
                    TipoPrioridad.TryParse(SPrioridad, out TipoPrioridad Prioridad);

                    Proyecto nuevoProyecto = new Proyecto(0)
                    {
                        NombreP = NombreP,
                        Cliente = Cliente,
                        FechaInicioP = FechaInicioP,
                        Tiempo= Tiempo,
                        Presupuesto = Presupuesto,
                        Prioridad = Prioridad,
                        DescripcionP = DescripcionP
                    };

                    nuevoProyecto.insertarProyecto(this.Usuario.IdEmpleado);

                    MessageBox.Show("Proyecto creado correctamente");
                }
            }
            else
            {
                creado= false;
                MessageBox.Show("Rellene todos los campos necesarios.");
            }

            return creado;

        }


        /// <summary>
        /// Comprueba que los datso de un proyecto estén correctos y completos y, si es así, llama al modelo Proyecto
        /// para que actualice los datos en la BBDD.
        /// </summary>
        /// <param name="proyectoModif">DataRow con la información del proyecto a modificar.</param>
        public void modificarProyecto(DataRow proyectoModif)
        {

            int.TryParse(proyectoModif["IdProyecto"].ToString(), out int IdProyecto);
            string NombreP = proyectoModif["NombreP"].ToString();
            string Cliente = proyectoModif["Cliente"].ToString();
            string SFechaInicioP = proyectoModif["FechaInicioP"].ToString();
            string SNTiempo = proyectoModif["Tiempo"].ToString().Split(' ')[0];
            string Tiempo = proyectoModif["Tiempo"].ToString().Split(' ')[1];
            string SPresupuesto = proyectoModif["Presupuesto"].ToString();
            string SPrioridad= proyectoModif["Prioridad"].ToString();
            string DescripcionP = proyectoModif["DescripcionP"].ToString();

            List<string> lCampos = new List<string>();
            lCampos.Add(NombreP);
            lCampos.Add(Cliente);
            lCampos.Add(SFechaInicioP);
            lCampos.Add(SNTiempo);
            lCampos.Add(SPresupuesto);
            lCampos.Add(DescripcionP);
            lCampos.Add(SPrioridad);

            if (!camposVacios(lCampos))
            {
                if (!int.TryParse(SNTiempo, out int NTiempo))
                {
                    MessageBox.Show("Introduzca el tiempo como un entero");
                }
                else if (!int.TryParse(SPresupuesto, out int Presupuesto))
                {
                    MessageBox.Show("Introduzca el presupuesto como un entero");
                }
                else
                {
                    Tiempo = NTiempo + " " + Tiempo;
                    DateTime.TryParse(SFechaInicioP, out DateTime FechaInicioP);
                    TipoPrioridad.TryParse(SPrioridad, out TipoPrioridad Prioridad);

                    Proyecto proyectoModificado = new Proyecto(IdProyecto)
                    {
                        NombreP = NombreP,
                        Cliente = Cliente,
                        FechaInicioP = FechaInicioP,
                        Tiempo = Tiempo,
                        Presupuesto = Presupuesto,
                        Prioridad = Prioridad,
                        DescripcionP = DescripcionP
                    };

                    proyectoModificado.updateProyecto(this.Usuario.IdEmpleado);

                    MessageBox.Show("Cambios guardados correctamente.");
                }
            }
            else
            {
                MessageBox.Show("Rellene todos los campos necesarios.");
            }

        }

        /// <summary>
        /// Convierte el SIdProyecto a int y se lo proporciona al modelo para que este lo elimine del sistema.
        /// </summary>
        /// <param name="SIdProyecto"></param>
        public void borrarProyecto(string SIdProyecto)
        {
            int IdProyecto = Convert.ToInt32(SIdProyecto);
            Proyecto proyectoBorrado = new Proyecto(IdProyecto);
            proyectoBorrado.deleteProyecto(this.Usuario.IdEmpleado);
            MessageBox.Show("Proyecto eliminado correctamente");
        }

        /// <summary>
        /// Llama al controlador de ventanas para que bloquee la ventana activa, mientras que invoca un contructor del
        /// controlador de la ventana FiltroProyecto para abrir una de estas.
        /// </summary>
        public void prepararFiltro()
        {
            this.filtro = string.Empty;
            ventanaControl.bloquearVActual();
            FiltroProyectoControl controladorFiltroP = new FiltroProyectoControl(this);
        }

        /// <summary>
        /// Llama al controlador de ventanas para que bloquee la ventana activa, mientras que invoca un contructor del
        /// controlador de la ventana BusquedaEmpleado para abrir una de estas.
        /// </summary>
        public void prepararFiltroEmpleado()
        {
            ventanaControl.bloquearVActual();
            controladorBusqueda = new BusquedaEmpleadoControlador(this)
            {
                dniBusqueda = string.Empty
            };
        }

        /// <summary>
        /// Añade el emlpeado indicado en la ventana de BusquedaEmpleado al proyecto indicado si no participa ya en él
        /// e invoca al método que informa de su adición.
        /// </summary>
        /// <param name="SIdProyecto">String del id del proyecto al que queremos añadir el empleado.</param>
        /// <param name="NombreP">Nombre del proyecto.</param>
        public void aniadirEmpleado(string SIdProyecto, string NombreP)
        {
            if (controladorBusqueda.dniBusqueda != string.Empty)
            {
                int.TryParse(SIdProyecto, out int IdProyecto);

                Proyecto proyecto = new Proyecto(IdProyecto);

                if (!proyecto.Participa(controladorBusqueda.dniBusqueda))
                {
                    proyecto.addEmpleado(controladorBusqueda.dniBusqueda, this.Usuario.IdEmpleado);
                    
                    MessageBox.Show("Empleado añadido correctamente.");

                    informarAdicion(controladorBusqueda.dniBusqueda, NombreP);
                }
                else
                    MessageBox.Show("El empleado seleccionado ya participa en ese proyecto");
            }
        }

        /// <summary>
        /// Convierte los ids proporcionados a int para proporcionárselos al modelo Proyecto y que este elimine la
        /// participación del empleado indicado en el proyecto indicado. Además, invoca al método que informa de 
        /// la eliminación.
        /// </summary>
        /// <param name="SIdEmpleado">String del id del empleado cuya participación queremos eliminar.</param>
        /// <param name="SIdProyecto">String del id del proyecto del cual queremos eliminar la participación.</param>
        /// <param name="NombreP">Nombre del proyecto.</param>
        public void eliminarEmpleado(string SIdEmpleado, string SIdProyecto, string NombreP)
        {
            int.TryParse(SIdProyecto, out int IdProyecto);
            int.TryParse(SIdEmpleado, out int IdEmpleado);

            Proyecto proyecto = new Proyecto(IdProyecto);
            proyecto.removeEmpleado(IdEmpleado, this.Usuario.IdEmpleado);
            MessageBox.Show("Empleado retirado del proyecto con éxtio.");

            informarEliminacion(IdEmpleado, NombreP);

        }

        /// <summary>
        /// Obtiene el mail del empleado cuya participación ha sido eliminada y llama a la clase Enviar mail para informarle
        /// de su eliminación via mail.
        /// </summary>
        /// <param name="IdEmpleado">Id del empleado cuya participación se quiere eliminar.</param>
        /// <param name="NombreP">Nombre del proyecto.</param>
        private void informarEliminacion(int IdEmpleado, string NombreP)
        {
            string correo = EnviarMail.obtenerMail(IdEmpleado);
            EnviarMail.retiroParticipacion(correo, NombreP);
        }

        /// <summary>
        /// Obtiene el mail del empleado añadido al proyecto y llama a la clase Enviar mail para informarle
        /// de su adición via mail.
        /// </summary>
        /// <param name="DNI">DNI del empleado cuya participación ha sido creada.</param>
        /// <param name="NombreP">Nombre del proyecto.</param>
        private void informarAdicion(string DNI, string NombreP)
        {
            string correo = EnviarMail.obtenerMail(DNI);
            EnviarMail.nuevaParticipacion(correo, NombreP);
        }
    }
}
