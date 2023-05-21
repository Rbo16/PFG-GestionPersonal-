using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPersonal
{
    public class Contrato
    {
        private int IdContrato { get; set; }
        private float HorasTrabajo { get; set; }
        private float HorasDescanso { get; set; }
        private TimeSpan HoraEntrada { get; set; }
        private TimeSpan HoraSalida { get; set; }
        private float Salario { get; set; }
        private string Puesto { get; set; }
        private float VacacionesMes { get; set; }
        private DateTime FechaAlta { get; set; }
        private DateTime FechaBaja { get; set; }
        private string Duracion { get; set; }
        private bool Activo { get; set; }
        private string DocumentoPDF { get; set; }
        private int IdEmpleado { get; set; }
        TipoContrato TipoCOntrato { get; set; }

        public Contrato() { }
    }
    enum TipoContrato
    {
        ParcialManiana, ParcialTarde, Completa, Practicas
    }
}
