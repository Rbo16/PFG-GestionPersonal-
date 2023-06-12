using GestionPersonal.Controladores;
using GestionPersonal.Controladores.Filtros;
using GestionPersonal.Utiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GestionPersonal
{
    public class AusenciaControl : Controlador
    {
        private DataTable dtAusencias;

        public AusenciaControl(VentanaControlador ventanaControl) : base(ventanaControl)
        {
            ventanaActiva = new Ausencias(this);
            ventanaActiva.Show();
        }


        /// <summary>
        /// Método para crear la ausencia e insertarla en la BBDD
        /// </summary>
        /// <param name="Razon">Razón de la ausencia</param>
        /// <param name="InicioA">Fecha de inicio de la ausencia</param>
        /// <param name="FinA">Fecha fin de la ausencia</param>
        /// <param name="DescripcionAus">Descripción de la ausencia</param>
        /// <param name="JustificantePDF">Ruta donde se ubica el justificante de la ausencia</param>
        public bool crearAusencia(string Razon, string InicioA, string FinA, string DescripcionAus, string JustificantePDF) 
        {
            bool creado = true;

            List<string> listaCampos = new List<string>();

            listaCampos.Add(Razon);
            listaCampos.Add(InicioA);
            listaCampos.Add(FinA);

            if (!camposVacios(listaCampos))
            {
                if(!DateTime.TryParse(InicioA, out DateTime FechaInicioA))
                {
                    creado = false;
                    MessageBox.Show("Introduzca la fecha inicio con formato dd/mm/aaaa");//Voy a poner DatePicker en verdad
                }
                else if (!DateTime.TryParse(InicioA, out DateTime FechaFinA))
                {
                    creado = false;
                    MessageBox.Show("Introduzca la fecha fin con formato dd/mm/aaaa");//Voy a poner DatePicker en verdad
                }
                else if (FechaInicioA < FechaFinA) 
                {
                    Ausencia nuevaAusencia = new Ausencia(0)
                    {
                        Razon = Razon,
                        FechaInicioA = FechaInicioA,
                        FechaFinA = FechaFinA,
                        DescripcionAus = DescripcionAus,
                        JustificantePDF = JustificantePDF,
                        IdSolicitante = Usuario.IdEmpleado 
                    };
                    nuevaAusencia.insertAusencia();

                    MessageBox.Show("Ausencia solicitada correctamente.");
                    creado = true;
                }
                else
                {
                    MessageBox.Show("La fecha fin ha de ser mayor que la fecha incicio.");
                }
            }
            else
            {
                MessageBox.Show("Rellene todos los campos necesarios.");
            }

            return creado;
        }


        public void modificarAusencia(DataRow ausenciaModificada)
        {
            int IdAusencia = Convert.ToInt32(ausenciaModificada["IdAusencia"].ToString());
            string Razon = ausenciaModificada["Razon"].ToString();
            DateTime.TryParse(ausenciaModificada["FechaInicioA"].ToString(), out DateTime FechaInicioA);
            DateTime.TryParse(ausenciaModificada["FechaFinA"].ToString(), out DateTime FechaFinA);
            string DescripcionAus = ausenciaModificada["DescripcionAus"].ToString();
            string JustificantePDF = ausenciaModificada["JustificantePDF"].ToString();
            EstadoAusencia.TryParse(ausenciaModificada["EstadoA"].ToString(), out EstadoAusencia EstadoA);
            int IdSolicitante = Convert.ToInt32(ausenciaModificada["IdSolicitante"].ToString());

            Ausencia ausenciaMod = new Ausencia(IdAusencia)
            {
                Razon = Razon,
                FechaInicioA = FechaInicioA,
                FechaFinA = FechaFinA,
                DescripcionAus = DescripcionAus,
                JustificantePDF = JustificantePDF,
                EstadoA = EstadoA,
                IdSolicitante = IdSolicitante//ESTO NO CREO QUE HAGA FALTA PORQUE SERÁ READONLY 
            };

            ausenciaMod.updateAusencia(this.Usuario.IdEmpleado);

        }
        
        public void gestionarAusencia(string SIdAusencia, string SEstadoA)
        {
            int EstadoA = Convert.ToInt32(SEstadoA);
            int IdAusencia = Convert.ToInt32(SIdAusencia);

            Ausencia ausenciaGestion = new Ausencia(IdAusencia)
            {
                EstadoA = (EstadoAusencia)EstadoA,
                IdAutorizador = this.Usuario.IdEmpleado
            };
            
            ausenciaGestion.updateAutorizador();
        }


        public void borrarAusencia(string SIdAusencia)
        {
            int IdAusencia = Convert.ToInt32(SIdAusencia);
            Ausencia ausenciaBorrada = new Ausencia(IdAusencia);
            ausenciaBorrada.deleteAusencia(this.Usuario.IdEmpleado);
        }

        /// <summary>
        /// Devuelve un DataTable con las ausencias del empleado indicado, o un listado general si IdSolicitante = -1.
        /// </summary>
        /// <param name="IdSolicitante"></param>
        /// <returns></returns>
        public DataTable listarAusencias(int IdSolicitante)
        {

            dtAusencias = Listar.listarAusencias();
            DataTable dtAux = new DataTable();
            if (IdSolicitante != -1)
            {
                dtAux = dtAusencias.Clone();
                string filtro = $"IdSolicitante = {IdSolicitante}";

                DataRow[] filasFiltradas = dtAusencias.Select(filtro);
                foreach (DataRow fila in filasFiltradas)
                {
                    dtAux.ImportRow(fila);
                }
                return dtAux;
            }
            else
            {
                return dtAusencias;
            }
        }

        /// <summary>
        /// Devuelve un array con los elementos del tipo enumerado EstadoAusencia
        /// </summary>
        /// <returns></returns>
        public Array devolverEstadosA()
        {
            return Enum.GetValues(typeof(EstadoAusencia));
        }

        public void prepararFiltro()
        {
            this.filtro = string.Empty;
            ventanaControl.bloquearVActual();
            FiltroAusenciaControl controladorFiltroA = new FiltroAusenciaControl(this);
        }
    }
}
