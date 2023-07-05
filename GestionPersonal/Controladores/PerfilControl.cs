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
    public class PerfilControl : Controlador
    {
        public PerfilControl(VentanaControlador ventanaControlador) : base(ventanaControlador)
        {
            Perfil ventanaPerfil = new Perfil(this);
            ventanaPerfil.Show();
        }

        /// <summary>
        /// Devuelve el nombre del departamento en el que trabaja el usuario actual.
        /// </summary>
        /// <returns></returns>
        public string nombreDepartamento()
        {
            DataTable dt = Listar.filtrarTabla(Listar.listarEmpleados(), $"IdEmpleado = {this.Usuario.IdEmpleado}");
            return dt.Rows[0]["NombreD"].ToString();
        }
    }
}
