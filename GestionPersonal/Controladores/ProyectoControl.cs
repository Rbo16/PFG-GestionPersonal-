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

        private void cargarProyectos()
        {
            dtProyectos = Listar.listarProyectos();
        }

        private void cargarProyectosEmpleado()
        {
            dtProyectos = Listar.listarProyectosEmpleado();
        }

        /// <summary>
        /// Devuelve el DataTable de Proyectos sin filtro
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
        /// Devuelve un DataTable que filtra el DataTable principal de Proyectos
        /// </summary>
        /// <param name="filtro">string con el filtro que se quiere aplicar</param>
        /// <returns></returns>
        public DataTable listaProyectos(string filtro)
        {
            return Listar.filtrarTabla(dtProyectos, filtro);
        }




        /// <summary>
        /// Devuelve un DataTable con los Empleados que participan en el Proyecto indicado
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

        public bool crearProyecto(string NombreP, string Cliente, string SFechaInicioP, string SNTiempo, string Tiempo, string SPresupuesto,
            string SPrioridad, string DescripcionP)
        {
            bool creado = true;

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
                if(!DateTime.TryParse(SFechaInicioP, out DateTime FechaInicioP))
                    creado= false;//DatePicker
                else if(!int.TryParse(SNTiempo, out int NTiempo))
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
                    Tiempo = NTiempo + " " + Tiempo;
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
                }
            }
            else
            {
                creado= false;
                MessageBox.Show("Rellene todos los campos necesarios.");
            }

            return creado;

        }

        public void modificarProyecto(DataRow proyectoModif)
        {
            //LO MEJOR VA A SER CONTROLAR SI ESTÁN VACIOS EN LAS VISTAS

            int.TryParse(proyectoModif["IdProyecto"].ToString(), out int IdProyecto);
            string NombreP = proyectoModif["NombreP"].ToString();
            string Cliente = proyectoModif["Cliente"].ToString();
            DateTime.TryParse(proyectoModif["FechaInicioP"].ToString(), out DateTime FechaInicioP);
            string Tiempo = proyectoModif["Tiempo"].ToString();
            int.TryParse(proyectoModif["Presupuesto"].ToString(), out int Presupuesto);
            TipoPrioridad.TryParse(proyectoModif["Prioridad"].ToString(), out TipoPrioridad Prioridad);
            string DescripcionP = proyectoModif["DescripcionP"].ToString();

            Proyecto proyectoModificado = new Proyecto(IdProyecto)
            {
                NombreP= NombreP,
                Cliente= Cliente,
                FechaInicioP= FechaInicioP,
                Tiempo= Tiempo,
                Presupuesto= Presupuesto,
                Prioridad= Prioridad,
                DescripcionP= DescripcionP
            };

            proyectoModificado.updateProyecto(this.Usuario.IdEmpleado);
        }

        public void borrarProyecto(string SIdProyecto)
        {
            int IdProyecto = Convert.ToInt32(SIdProyecto);
            Proyecto proyectoBorrado = new Proyecto(IdProyecto);
            proyectoBorrado.deleteProyecto(this.Usuario.IdEmpleado);
        }

        public void prepararFiltro()
        {
            this.filtro = string.Empty;
            ventanaControl.bloquearVActual();
            FiltroProyectoControl controladorFiltroP = new FiltroProyectoControl(this);
        }

        public void prepararFiltroEmpleado()
        {
            ventanaControl.bloquearVActual();
            controladorBusqueda = new BusquedaEmpleadoControlador(this)
            {
                dniBusqueda = string.Empty
            };
        }

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
        public void eliminarEmpleado(string SIdEmpleado, string SIdProyecto, string NombreP)
        {
            int.TryParse(SIdProyecto, out int IdProyecto);
            int.TryParse(SIdEmpleado, out int IdEmpleado);

            Proyecto proyecto = new Proyecto(IdProyecto);
            proyecto.removeEmpleado(IdEmpleado, this.Usuario.IdEmpleado);
            MessageBox.Show("Empleado retirado del proyecto con éxtio.");

            informarEliminacion(IdEmpleado, NombreP);

        }

        private void informarEliminacion(int IdEmpleado, string NombreP)
        {
            string correo = EnviarMail.obtenerMail(IdEmpleado);
            EnviarMail.retiroParticipacion(correo, NombreP);
        }

        private void informarAdicion(string DNI, string NombreP)
        {
            string correo = EnviarMail.obtenerMail(DNI);
            EnviarMail.nuevaParticipacion(correo, NombreP);
        }
    }
}
