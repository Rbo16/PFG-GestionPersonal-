using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPersonal
{
    internal class Ausencia
    {
        private int IdAusencia { get; set; }
        private string Razon { get; set; }
        private DateTime FechaInicio { get; set; }
        private DateTime FechaFin { get; set; }
        private string DescripcionAus { get; set; }
        private string JustificantePDF { get; set; }
        private int IdSolicitante { get; set; }
        private int IdGestor { get; set; }

        public Ausencia()
        {

        }
    }
    enum estadoAusencia
    {
        Aceptada, Rechazada, Pendiente
    }
}
