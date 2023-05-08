using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPersonal
{
    internal class Proyecto
    {
        private int Idproyecto { get; set; }
        private string NombreP { get; set; }
        private string Cliente { get; set; }
        private string Tiempo { get; set; }
        private DateTime FechaInicioP { get; set; }
        private DateTime FechaFinP { get; set; }
        private int Presupuesto { get; set; }
        Prioridad Prioridad { get; set; }
        private string DescripcionP { get; set; }
        public Proyecto() { }
    }
    enum Prioridad
    {
        Urgente, Alta, Moderada, Baja
    }
}
