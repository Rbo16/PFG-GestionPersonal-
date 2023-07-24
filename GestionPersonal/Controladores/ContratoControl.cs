using GestionPersonal.Controladores;
using GestionPersonal.Controladores.Filtros;
using GestionPersonal.Utiles;
using GestionPersonal.Vistas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GestionPersonal
{
    public class ContratoControl : Controlador
    {
        DataTable dtContratos = new DataTable();
        public BusquedaEmpleadoControlador controladorBusqueda;

        public ContratoControl(VentanaControlador ventanaControl) : base(ventanaControl)
        {
            cargarContratos();
            ventanaActiva = new Contratos(this);
            ventanaActiva.Show();
        }

        /// <summary>
        /// Llama a la clase listar para obtener un DataTable de los contratos del sistema y lo asigna al DataTable
        /// principal del controlador.
        /// </summary>
        private void cargarContratos()
        {
            dtContratos = Listar.listarContratos();
        }


        /// <summary>
        /// Devuelve el DataTable de contratos sin filtro.
        /// </summary>
        /// <returns></returns>
        public DataTable listaContratos()
        {
            cargarContratos();
            return dtContratos;
        }

        /// <summary>
        /// Devuelve un DataTable que filtra el DataTable principal de Contratos a partir de la clase Listar.
        /// </summary>
        /// <param name="filtro">string con el filtro que se quiere aplicar.</param>
        /// <returns></returns>
        public DataTable listaContratos(string filtro)
        {
            return Listar.filtrarTabla(dtContratos, filtro);
        }

        /// <summary>
        /// Recibe los strings con la información necesaria para la creación de un contrato, comprueba que estén correctos,
        /// y llama al modelo para insertarlo en la Base de Datos. Además, invoca al método que avisa del alta del contrato.
        /// </summary>
        /// <param name="SHorasTrabajo">Horas diarias trabajadas.</param>
        /// <param name="SHorasDescanso">Horas diarias de descanso.</param>
        /// <param name="SHoraEntrada"> Hora de entrada diaria.</param>
        /// <param name="SHoraSalida">Hora de salida diaria.</param>
        /// <param name="SSalario">Salario anual.</param>
        /// <param name="Puesto">Nombre del puesto dentro de la empresa.</param>
        /// <param name="SVacacionesMes">Días de vacaciones por mes.</param>
        /// <param name="Duracion">Duración del contrato.</param>
        /// <param name="SIdEmpleado">Id del empleado poseedor del contrato.</param>
        /// <param name="STipoContrato">Tipo de contrato.</param>
        public bool crearContrato(string SHorasTrabajo, string SHorasDescanso, string SHoraEntrada, string SHoraSalida,
            string SSalario, string Puesto, string SVacacionesMes, string Duracion, string SIdEmpleado, string STipoContrato)
        {
            bool correcto = true;

            List<string> lCampos = new List<string>();

            lCampos.Add(Puesto);
            lCampos.Add(Duracion);
            lCampos.Add(SSalario);
            lCampos.Add(SHoraEntrada);
            lCampos.Add(SHoraSalida);
            lCampos.Add(STipoContrato);
            if (Duracion.Split(' ')[0] != "Indefinido" && Duracion.Split(' ').Count() == 1)
                lCampos.Add(string.Empty);//Forzamos el fallo si no ha indicado unidad en la duración.

            if (!camposVacios(lCampos))
            {
                //Si no hay comrpobación del campo, es que el usuario no tiene posibilidad de editarlo.
                if (!TimeSpan.TryParse(SHoraEntrada, out TimeSpan HoraEntrada))
                {
                    correcto = false;
                    MessageBox.Show("Introduzca la hora de entrada con formato HH:mm.");
                }
                else if (!TimeSpan.TryParse(SHoraSalida, out TimeSpan HoraSalida))
                {
                    correcto = false;
                    MessageBox.Show("Introduzca la hora de salida con formato HH:mm.");
                }
                else if (!float.TryParse(SSalario, out float Salario))
                {
                    correcto = false;
                    MessageBox.Show("Introduzca el salario como un decimal.");
                }
                else if (Duracion.Split(' ')[0].Trim() != "Indefinido" && !float.TryParse(Duracion.Split(' ')[0], out float NDuracion))
                {
                    correcto = false;
                    MessageBox.Show("Introduzca la duración como un decimal.");
                }
                else
                {
                    string DocumentoPDF = "porcrear";//METODO PARA CREAR EL PDF

                    float.TryParse(SHorasTrabajo, out float HorasTrabajo);
                    float.TryParse(SHorasDescanso, out float HorasDescanso);
                    float.TryParse(SVacacionesMes, out float VacacionesMes);
                    int.TryParse(SIdEmpleado, out int IdEmpleado);
                    TipoContrato.TryParse(STipoContrato, out TipoContrato tipoContrato);
                    if (Duracion.Split(' ')[0].Trim() == "Indefinido")
                    {
                        Duracion = Duracion.Trim();
                    }
                    Contrato nuevoContrato = new Contrato(0)
                    {
                        HorasTrabajo = HorasTrabajo,
                        HorasDescanso = HorasDescanso,
                        HoraEntrada = HoraEntrada,
                        HoraSalida = HoraSalida,
                        Salario = Salario,
                        Puesto = Puesto,
                        VacacionesMes = VacacionesMes,
                        Duracion = Duracion,
                        DocumentoPDF = DocumentoPDF,
                        IdEmpleado = IdEmpleado,
                        TipoContrato = tipoContrato
                    };
                    nuevoContrato.insertContrato(this.Usuario.IdEmpleado);

                    informarAlta(IdEmpleado);
                }
            }
            else
            {
                correcto = false;
                MessageBox.Show("Rellene todos los campos necesarios");
            }

            return correcto;
        }

        /// <summary>
        /// Comprueba que los nuevos datos del contrato estén completos y correctos, y llama al modelo Contrato
        /// para que actualice los datos en la BBDD. Además, invoca al método que avisa via mail de los cambios.
        /// </summary>
        /// <param name="contratoModif"></param>
        /// <returns></returns>
        public bool modificarContrato(DataRow contratoModif)
        {
            bool correcto = true;

            string Puesto = contratoModif["Puesto"].ToString();
            string Duracion = contratoModif["Duracion"].ToString();
            string SSalario = contratoModif["Salario"].ToString();
            string SHoraEntrada = contratoModif["HoraEntrada"].ToString();
            string SHoraSalida = contratoModif["HoraSalida"].ToString();
            string SHorasTrabajo = contratoModif["HorasTrabajo"].ToString();
            string SHorasDescanso = contratoModif["HorasDescanso"].ToString();
            string SIdEmpleado = contratoModif["IdEmpleado"].ToString();
            string SVacacionesMes = contratoModif["VacacionesMes"].ToString();
            string STipoContrato = contratoModif["TipoContrato"].ToString();

            List<string> lCampos = new List<string>();

            lCampos.Add(Puesto);
            lCampos.Add(Duracion);
            lCampos.Add(SSalario);
            lCampos.Add(SHoraEntrada);
            lCampos.Add(SHoraSalida);
            if (Duracion.Split(' ')[0] != "Indefinido" && Duracion.Split(' ').Count() == 1)
                lCampos.Add(string.Empty);//Forzamos el fallo si no ha indicado unidad en la duración.

            if (!camposVacios(lCampos))
            {
                //Si no hay comrpobación del campo, es que el usuario no tiene posibilidad de editarlo.
                if (!TimeSpan.TryParse(SHoraEntrada, out TimeSpan HoraEntrada))
                {
                    correcto = false;
                    MessageBox.Show("Introduzca la hora de entrada con formato HH:mm.");
                }
                else if (!TimeSpan.TryParse(SHoraSalida, out TimeSpan HoraSalida))
                {
                    correcto = false;
                    MessageBox.Show("Introduzca la hora de salida con formato HH:mm.");
                }
                else if (!float.TryParse(SSalario, out float Salario))
                {
                    correcto = false;
                    MessageBox.Show("Introduzca el salario como un decimal.");
                }
                else if (Duracion.Split(' ')[0] != "Indefinido" && !float.TryParse(Duracion.Split(' ')[0], out float NDuracion))
                {
                    correcto = false;
                    MessageBox.Show("Introduzca la duración como un decimal.");
                }
                else
                {
                    string DocumentoPDF = "porcrear";//METODO PARA CREAR EL PDF

                    int.TryParse(contratoModif["IdContrato"].ToString(), out int IdContrato);
                    float.TryParse(SHorasTrabajo, out float HorasTrabajo);
                    float.TryParse(SHorasDescanso, out float HorasDescanso);
                    float.TryParse(SVacacionesMes, out float VacacionesMes);
                    int.TryParse(SIdEmpleado, out int IdEmpleado);
                    TipoContrato.TryParse(STipoContrato, out TipoContrato tipoContrato);

                    Contrato nuevoContrato = new Contrato(IdContrato)
                    {
                        HorasTrabajo = HorasTrabajo,
                        HorasDescanso = HorasDescanso,
                        HoraEntrada = HoraEntrada,
                        HoraSalida = HoraSalida,
                        Salario = Salario,
                        Puesto = Puesto,
                        VacacionesMes = VacacionesMes,
                        Duracion = Duracion,
                        DocumentoPDF = DocumentoPDF,
                        IdEmpleado = IdEmpleado,
                        TipoContrato = tipoContrato
                    };
                    nuevoContrato.updateContrato(this.Usuario.IdEmpleado);

                    informarAuditoria(IdContrato, "CAMBIO");
                }
            }
            else
            {
                correcto = false;
                MessageBox.Show("Rellene todos los campos necesarios");
            }
            return correcto;
        }

        /// <summary>
        /// Llama al modelo para hacer el borrado lógico del contrato propocionado.
        /// </summary>
        /// <param name="SIdContrato">String con el Id del contrato a borrar.</param>
        public void eliminarContrato(string SIdContrato)
        {
            int.TryParse(SIdContrato, out int IdContrato);
            Contrato contratoEliminar = new Contrato(IdContrato);
            contratoEliminar.deleteContrato(this.Usuario.IdEmpleado);
            informarAuditoria(IdContrato, "BORRADO");
        }

        /// <summary>
        /// Llama al controlador de ventanas para que bloquee la ventana activa, mientras que invoca un contructor del
        /// controlador de la ventana FiltroContrato para abrir una de estas.
        /// </summary>
        public void prepararFiltro()
        {
            ventanaControl.bloquearVActual();
            FiltroContratoControl controladorFiltroC = new FiltroContratoControl(this);
        }

        public void prepararFiltroEmpleado()
        {
            ventanaControl.bloquearVActual();
            controladorBusqueda = new BusquedaEmpleadoControlador(this)
            {
                dniBusqueda = string.Empty
            };
        }

        /// <summary>
        /// Obtiene el correo del usuario cuyo contrato ha sido dado de alta y llama a la clase EnviarMail
        /// para que envíe un mail informando de dicho alta.
        /// </summary>
        /// <param name="IdEmpleado">Id del empleado cuyo contrato ha sido dado de alta.</param>
        private void informarAlta(int IdEmpleado)
        {
            string correo = EnviarMail.obtenerMail(IdEmpleado);
            EnviarMail.altaContrato(correo);
        }

        /// <summary>
        /// Obtiene el mail de la persona cuyo contrato ha sido alterado, así como las fechas del contrato
        /// para pasárseloa a la clase EnviarMail y que esta envíe un mail informando del cambio que ha ocurrido 
        /// </summary>
        /// <param name="IdContrato">Id del contrato afectado</param>
        /// <param name="tipoAuditoria">indica el tipo de cmabio, y por tanto de mail a enviar</param>
        private void informarAuditoria(int IdContrato, string tipoAuditoria)
        {
            DataTable contrato = Listar.filtrarTabla(dtContratos, $"IdContrato = {IdContrato}");

            string mail = EnviarMail.obtenerMail(Convert.ToInt32(contrato.Rows[0]["IdEmpleado"].ToString()));

            EnviarMail.auditoriaContrato(mail, tipoAuditoria, contrato.Rows[0]["FechaAlta"].ToString(),
                contrato.Rows[0]["FechaBaja"].ToString());
        }
    }
}
