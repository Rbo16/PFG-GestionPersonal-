using GestionPersonal.Controladores;
using GestionPersonal.Controladores.Filtros;
using GestionPersonal.Utiles;
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

        public ContratoControl(VentanaControlador ventanaControl) : base(ventanaControl)
        {
            ventanaActiva = new Contratos(this);
            ventanaActiva.Show();
        }

        public Array devolverTipoContrato()
        {
            return Enum.GetValues(typeof(TipoContrato));
        }

        public DataTable listarContratos(int IdEmpleado)
        {
            dtContratos = Listar.listarContratos();

            string filtro = $"IdEmpleado = {IdEmpleado}";
            DataTable dtAux = dtContratos.Clone();

            DataRow[] filasFiltradas = dtContratos.Select(filtro);
            foreach(DataRow fila in filasFiltradas)
            {
                dtAux.ImportRow(fila);
            }

            return dtAux;
        }


        /// <summary>
        /// Recibe los strings con la información necesaria para la creación de un contrato, comprueba que estén correctos,
        /// y llama al modelo para insertarlo en la Base de Datos
        /// </summary>
        /// <param name="SHorasTrabajo">Horas diarias trabajadas</param>
        /// <param name="SHorasDescanso">Horas diarias de descanso</param>
        /// <param name="SHoraEntrada"> Hora de entrada diaria</param>
        /// <param name="SHoraSalida">Hora de salida diaria</param>
        /// <param name="SSalario">Salario anual</param>
        /// <param name="Puesto">Nombre del puesto dentro de la empresa</param>
        /// <param name="SVacacionesMes">Días de vacaciones por mes</param>
        /// <param name="Duracion">Duración del contrato</param>
        /// <param name="DocumentoPDF">Ruta en la que se encuenrta el PDF con la información del contrato</param>
        /// <param name="SIdEmpleado">Id del empleado poseedor del contrato</param>
        /// <param name="STipoContrato">Tipo de contrato</param>
        public bool crearContrato(string SHorasTrabajo, string SHorasDescanso, string SHoraEntrada, string SHoraSalida,
            string SSalario, string Puesto, string SVacacionesMes, string Duracion, string SIdEmpleado, string STipoContrato)
        {
            bool correcto = true;
            CultureInfo culturaEspañola = new CultureInfo("es-ES");

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
                else if (Duracion.Split(' ')[0] != "Indefinido" && !int.TryParse(Duracion.Split(' ')[0], out int NDuracion))
                {
                    correcto = false;
                    MessageBox.Show("Introduzca la duración como un entero.");
                }
                else
                {
                    string DocumentoPDF = "porcrear";//METODO PARA CREAR EL PDF

                    float.TryParse(SHoraSalida, out float HorasTrabajo);
                    float.TryParse(SHorasDescanso, out float HorasDescanso);
                    float.TryParse(SVacacionesMes, out float VacacionesMes);
                    int.TryParse(SIdEmpleado, out int IdEmpleado);
                    TipoContrato.TryParse(STipoContrato, out TipoContrato tipoContrato);
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
        }

        public void prepararFiltro()
        {
            this.filtro = string.Empty;
            ventanaControl.bloquearVActual();
            FiltroContratoControl controladorFiltroC = new FiltroContratoControl(this);
        }

    }
}
