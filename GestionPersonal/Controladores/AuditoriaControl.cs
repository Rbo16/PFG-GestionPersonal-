using GestionPersonal.Utiles;
using GestionPersonal.Vistas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPersonal.Controladores
{
    public class AuditoriaControl : Controlador
    {
        public AuditoriaControl(VentanaControlador ventanaControl) :base(ventanaControl) 
        {
            ventanaActiva = new Auditorias(this);
            ventanaActiva.Show();
        }

        public DataTable listarAuditoria(string Tabla)
        {
            return Listar.listarAuditorias(Tabla);
        }
    }
}
