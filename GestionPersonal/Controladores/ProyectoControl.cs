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

        public ProyectoControl(VentanaControlador ventanaControl) : base(ventanaControl)
        {
            ventanaActiva = new Proyectos(this);
            ventanaActiva.Show();
        }

        public DataTable listarProyectos(int IdSolicitante)
        {
            dtProyectos = Listar.listarProyectos();

            if(IdSolicitante != -1)
            {
                DataTable dtAux = dtProyectos.Clone();
                DataRow[] filasFiltradas = dtProyectos.Select($"IdEmpleado = {IdSolicitante}");

                foreach(DataRow fila in filasFiltradas)
                {
                    dtAux.ImportRow(fila);
                }

                return dtAux;

            }
            else
            {
                return dtProyectos;
            }             
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
    }
}
