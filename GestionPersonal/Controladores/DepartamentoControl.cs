using GestionPersonal.Controladores;
using GestionPersonal.Utiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestionPersonal
{
    public  class DepartamentoControl : Controlador
    {
        DataTable dtDepas;
        BusquedaEmpleadoControlador controladorBusqueda;

        public DepartamentoControl(VentanaControlador ventanaControl) : base(ventanaControl) 
        {
            ventanaActiva = new Departamentos(this);
            ventanaActiva.Show();
        }

        private void cargarDepartamentos()
        {
            dtDepas = Listar.listarDepartamentos();
        }

        /// <summary>
        /// Devuelve el DataTable de Departamentos sin filtro
        /// </summary>
        /// <returns></returns>
        public DataTable listaDepartamentos()
        {
            cargarDepartamentos();
            return dtDepas;
        }

        /// <summary>
        /// Devuelve un DataTable que filtra el DataTable principal de Departamentos
        /// </summary>
        /// <param name="filtro">string con el filtro que se quiere aplicar</param>
        /// <returns></returns>
        public DataTable listaDepartamentos(string filtro)
        {
            return Listar.filtrarTabla(dtDepas, filtro);
        }

        /// <summary>
        /// Devuelve un DataTable con los Empleados pertenecientes al Departamento indicado
        /// </summary>
        /// <param name="SIdDepartamento">Id del Departamento en formato string</param>
        /// <returns></returns>
        public DataTable listaEmpleadosDepartamento(string SIdDepartamento)
        {
            int.TryParse(SIdDepartamento, out int IdDepartamento);
            string filtro = $" IdDepartamento = {IdDepartamento}";
            DataTable dtEmpleados = Listar.listarEmpleados();
            return Listar.filtrarTabla(dtEmpleados, filtro);
        }

        public bool crearDepartamento(string NombreD, string DescripcionD)
        {
            bool creado = true;
            if (NombreD != string.Empty && DescripcionD != string.Empty)
            {
                Departamento nuevoDepartamento = new Departamento()
                {
                    NombreD = NombreD,
                    DescripcionD = DescripcionD
                };
                nuevoDepartamento.insertarDepartamento(this.Usuario.IdEmpleado);
            }
            else
            {
                creado = false;
                MessageBox.Show("Rellene todos los campos necesarios.");
            }

            return creado;
        }

        public void modificarDepartamento(DataRow departamentoModif)
        {
            int.TryParse(departamentoModif["IdDepartamento"].ToString(), out int IdDepartamento);
            string NombreD = departamentoModif["NombreD"].ToString();
            string DescripcionD = departamentoModif["DescripcionD"].ToString();

            Departamento departamentoModificado = new Departamento()
            {
                IdDepartamento = IdDepartamento,
                NombreD = NombreD,
                DescripcionD = DescripcionD
            };

            departamentoModificado.updateDepartamento(this.Usuario.IdEmpleado);
            
        }

        /// <summary>
        /// Convierte el parámetro SIdDepartemento a int y llama al modelo de departamento para eliminar dicho departamento
        /// </summary>
        /// <param name="SIdDepartamento"></param>
        public void eliminarDepartamento(string SIdDepartamento)
        {
            int.TryParse(SIdDepartamento, out int IdDepartamento);
            Departamento departamentoBorrado = new Departamento()
            {
                IdDepartamento = IdDepartamento
            };
            departamentoBorrado.deleteDepartamento(this.Usuario.IdEmpleado);
        }

        public bool asignarJefe(string SIdEmpleado, string SIdDepartamento, string NombreD)
        {
            bool exito = false;

            int.TryParse(SIdEmpleado, out int IdEmpleado);
            int.TryParse(SIdDepartamento, out int IdDepartamento);
            Departamento departamento = new Departamento()
            {
                IdDepartamento = IdDepartamento
            };

            if (!departamento.comprobarJefe(IdEmpleado))
            {
                departamento.asignarJefe(IdEmpleado, this.Usuario.IdEmpleado);
                exito = true;
                MessageBox.Show("Jefe de Departamento asignado con éxito");
                informarAsignacion(IdEmpleado, NombreD);
            }
            else
                MessageBox.Show("El empleado seleccionado ya ejerce como jefe de otro departamento.");

            return exito;
        }

        private void informarAsignacion(int IdEmpleado, string NombreD)
        {
            string correo = EnviarMail.obtenerMail(IdEmpleado);
            EnviarMail.nuevoJefe(correo, NombreD);
        }

        public void prepararFiltroEmpleado()
        {
            ventanaControl.bloquearVActual();
            controladorBusqueda = new BusquedaEmpleadoControlador(this)
            {
                dniBusqueda = string.Empty
            };
        }
        
        public void aniadirEmpleado(string SIdDepartamento, string NombreD)
        {
            if(controladorBusqueda.dniBusqueda != string.Empty)
            {
                int.TryParse(SIdDepartamento, out int IdDepartamento);

                Departamento departamento = new Departamento()
                {
                    IdDepartamento = IdDepartamento
                };

                departamento.addEmpleado(controladorBusqueda.dniBusqueda, this.Usuario.IdEmpleado);

                MessageBox.Show("Empleado añadido correctamente.");

                informarAdicion(controladorBusqueda.dniBusqueda, NombreD);
            }
        }

        private void informarAdicion(string DNI, string NombreD)
        {
            string correo = EnviarMail.obtenerMail(DNI);
            EnviarMail.nuevoDepartamento(correo, NombreD);
        }
    }
}
