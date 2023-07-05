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

        /// <summary>
        /// Llama a la clase Listar para obtener un DataTable de auditorías de la tabla indicada, para después devolverlo.
        /// </summary>
        /// <param name="Tabla">Nombre de la tabla de la que se quiere visualizar la auditorías</param>
        /// <returns></returns>
        public DataTable listarAuditoria(string Tabla)
        {
            return Listar.listarAuditorias(Tabla);
        }
    }
}
