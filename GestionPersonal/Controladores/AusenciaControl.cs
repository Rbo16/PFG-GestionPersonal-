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

        private void cargarAusencias()
        {
            dtAusencias = Listar.listarAusencias();
        }


        /// <summary>
        /// Devuelve el DataTable de Ausencias sin filtro
        /// </summary>
        /// <returns></returns>
        public DataTable listaAusencias()
        {
            cargarAusencias();
            return dtAusencias;
        }

        /// <summary>
        /// Devuelve un DataTable que filtra el DataTable principal de Ausencias
        /// </summary>
        /// <param name="filtro">string con el filtro que se quiere aplicar</param>
        /// <returns></returns>
        public DataTable listaAusencias(string filtro)
        {
            return Listar.filtrarTabla(dtAusencias, filtro);
        }

        /// <summary>
        /// Método para crear la ausencia e insertarla en la BBDD con estado "Pendiente" a informar a los Gestores
        /// </summary>
        /// <param name="Razon">Razón de la ausencia</param>
        /// <param name="FechaInicioA">Fecha de inicio de la ausencia</param>
        /// <param name="FechaFinA">Fecha fin de la ausencia</param>
        /// <param name="DescripcionAus">Descripción de la ausencia</param>
        /// <param name="JustificantePDF">Ruta donde se ubica el justificante de la ausencia</param>
        public bool crearAusencia(string Razon, DateTime? FechaInicioA, DateTime? FechaFinA, string DescripcionAus, string JustificantePDF) 
        {
            bool creado = true;

            List<string> listaCampos = new List<string>();

            listaCampos.Add(Razon);
            listaCampos.Add(FechaInicioA.ToString());
            listaCampos.Add(FechaFinA.ToString());

            if (!camposVacios(listaCampos))
            {

                if (FechaInicioA < FechaFinA) 
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

            informarAutorizacion(IdAusencia);
        }


        public void borrarAusencia(string SIdAusencia)
        {
            int IdAusencia = Convert.ToInt32(SIdAusencia);
            Ausencia ausenciaBorrada = new Ausencia(IdAusencia);
            ausenciaBorrada.deleteAusencia(this.Usuario.IdEmpleado);
        }

        private void informarAutorizacion(int IdAusencia)
        {
            DataTable ausencia = Listar.filtrarTabla(dtAusencias, $"IdAusencia = {IdAusencia}");

            string mail = EnviarMail.obtenerMail(Convert.ToInt32(ausencia.Rows[0]["IdSolicitante"].ToString()));

            EnviarMail.altaAusencia(mail, ausencia.Rows[0]["Razon"].ToString(),
                ausencia.Rows[0]["FechaInicioA"].ToString(), ausencia.Rows[0]["FechaFinA"].ToString(),
                ausencia.Rows[0]["EstadoA"].ToString());
        }

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
        /// Devuelve un array con los elementos del tipo enumerado EstadoAusencia
        /// </summary>
        /// <returns></returns>
        public Array devolverEstadosA()
        {
            return Enum.GetValues(typeof(EstadoAusencia));
        }

        public void prepararFiltro()
        {
            ventanaControl.bloquearVActual();
            FiltroAusenciaControl controladorFiltroA = new FiltroAusenciaControl(this);
        }

    }
}
