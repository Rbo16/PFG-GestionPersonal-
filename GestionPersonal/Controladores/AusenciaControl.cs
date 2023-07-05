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
            cargarAusencias();
            ventanaActiva = new Ausencias(this);
            ventanaActiva.Show();
        }

        /// <summary>
        /// Carga la lista de Ausencias  llamando a su método correspondiente de la clase Listar.
        /// </summary>
        private void cargarAusencias()
        {
            dtAusencias = Listar.listarAusencias();
        }


        /// <summary>
        /// Devuelve el DataTable de Ausencias sin filtro.
        /// </summary>
        /// <returns></returns>
        public DataTable listaAusencias()
        {
            cargarAusencias();
            return dtAusencias;
        }

        /// <summary>
        /// Devuelve un DataTable que filtra el DataTable principal de Ausencias a partir de la clase Listar.
        /// </summary>
        /// <param name="filtro">string con el filtro que se quiere aplicar.</param>
        /// <returns></returns>
        public DataTable listaAusencias(string filtro)
        {
            return Listar.filtrarTabla(dtAusencias, filtro);
        }

        /// <summary>
        /// Comprueba que todos los campos necesarios están completos y que la fecha de incio es menor o igual que
        /// la fecha fin para llamar al modelo Ausencia y así crear la ausencia e insertarla en la BBDD con estado "Pendiente". 
        /// También llama al método que informa a los gestores.
        /// </summary>
        /// <param name="Razon">Razón de la ausencia.</param>
        /// <param name="FechaInicioA">Fecha de inicio de la ausencia.</param>
        /// <param name="FechaFinA">Fecha fin de la ausencia.</param>
        /// <param name="DescripcionAus">Descripción de la ausencia.</param>
        /// <param name="JustificantePDF">Ruta donde se ubica el justificante de la ausencia.</param>
        public bool crearAusencia(string Razon, DateTime? FechaInicioA, DateTime? FechaFinA, string DescripcionAus, string JustificantePDF) 
        {
            bool creado = true;

            List<string> listaCampos = new List<string>();

            listaCampos.Add(Razon);
            listaCampos.Add(FechaInicioA.ToString());
            listaCampos.Add(FechaFinA.ToString());

            if (!camposVacios(listaCampos))
            {

                if (FechaInicioA <= FechaFinA) 
                {
                    Ausencia nuevaAusencia = new Ausencia(0)
                    {
                        Razon = Razon,
                        FechaInicioA = FechaInicioA,
                        FechaFinA = FechaFinA,
                        EstadoA = EstadoAusencia.Pendiente,
                        DescripcionAus = DescripcionAus,
                        JustificantePDF = JustificantePDF,
                        IdSolicitante = Usuario.IdEmpleado 
                    };
                    nuevaAusencia.insertAusencia();

                    MessageBox.Show("Ausencia solicitada correctamente.");
                    creado = true;

                    informarGestores();
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

        /// <summary>
        /// Comprueba que todos los campos necesarios están completos y que la fecha de incio es menor o igual que
        /// la fecha fin para llamar al modelo Ausencia y que este guarde las modificaciones.
        /// </summary>
        /// <param name="ausenciaModificada">DataRow que contiene los datos de la ausencia a modificar.</param>
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

            List<string> listaCampos = new List<string>();

            listaCampos.Add(Razon);
            listaCampos.Add(FechaInicioA.ToString());
            listaCampos.Add(FechaFinA.ToString());

            if (!camposVacios(listaCampos))
            {

                if (FechaInicioA < FechaFinA)
                {

                    Ausencia ausenciaMod = new Ausencia(IdAusencia)
                    {
                        Razon = Razon,
                        FechaInicioA = FechaInicioA,
                        FechaFinA = FechaFinA,
                        DescripcionAus = DescripcionAus,
                        JustificantePDF = JustificantePDF,
                        EstadoA = EstadoA,
                        IdSolicitante = IdSolicitante
                    };

                    ausenciaMod.updateAusencia(this.Usuario.IdEmpleado);
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
        }
        
        /// <summary>
        /// Convierte los strings a su tipo correspondiente, llama al modelo de Ausencia para que actualice los
        /// datos necesatios en la BBDD, y después llama al método que informa de la gestión por mail.
        /// </summary>
        /// <param name="SIdAusencia">El IdAusencia de la ausencia que se quiere gestionar.</param>
        /// <param name="SEstadoA">El estado que se le ha dado a la ausencia.</param>
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

            informarAutorizacion(IdAusencia);
        }

        /// <summary>
        /// Llama al modelo Ausencia para que elimine la ausencia del sistema.
        /// </summary>
        /// <param name="SIdAusencia">String de la ausencia a eliminar</param>
        public void borrarAusencia(string SIdAusencia)
        {
            int IdAusencia = Convert.ToInt32(SIdAusencia);
            Ausencia ausenciaBorrada = new Ausencia(IdAusencia);
            ausenciaBorrada.deleteAusencia(this.Usuario.IdEmpleado);
            MessageBox.Show("Ausencia eliminada con éxito");
        }

        /// <summary>
        /// Obtiene el correo, la razón, las fechas y el nuevo estado de la ausencia que se está gestionando, para
        /// pasarlos como parámetos al método que informa via mail de la actualización del estado de la ausencia.
        /// </summary>
        /// <param name="IdAusencia">El id de la ausencia que se está gestionando.</param>
        private void informarAutorizacion(int IdAusencia)
        {
            DataTable ausencia = Listar.filtrarTabla(dtAusencias, $"IdAusencia = {IdAusencia}");

            string mail = EnviarMail.obtenerMail(Convert.ToInt32(ausencia.Rows[0]["IdSolicitante"].ToString()));

            EnviarMail.altaAusencia(mail, ausencia.Rows[0]["Razon"].ToString(),
                ausencia.Rows[0]["FechaInicioA"].ToString(), ausencia.Rows[0]["FechaFinA"].ToString(),
                ausencia.Rows[0]["EstadoA"].ToString());
        }

        /// <summary>
        /// Obtiene el correo de los empleados con rol de Gestor y llama a la clase EnviarMail para que les informe
        /// de que hay nuevas ausencias por autorizar.
        /// </summary>
        private void informarGestores()
        {
            DataTable dtEmpleados = Listar.listarEmpleados();
            DataTable dtGestores = Listar.filtrarTabla(dtEmpleados, "rol = 2");

            foreach(DataRow dr in dtGestores.Rows)
            {
                string correo = dr["CorreoE"].ToString();
                EnviarMail.solicitudAusencia(correo);
            }
        }

        /// <summary>
        /// Llama al controlador de ventanas para que bloquee la ventana activa, mientras que invoca un contructor del
        /// controlador de la ventana FiltroAusencia para abrir una de estas.
        /// </summary>
        public void prepararFiltro()
        {
            ventanaControl.bloquearVActual();
            FiltroAusenciaControl controladorFiltroA = new FiltroAusenciaControl(this);
        }

    }
}
