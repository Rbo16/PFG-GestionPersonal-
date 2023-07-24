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

        /// <summary>
        /// Carga la lista de Contratos llamando a su método correspondiente de la clase Listar.
        /// </summary>
        private void cargarDepartamentos()
        {
            dtDepas = Listar.listarDepartamentos();
        }

        /// <summary>
        /// Devuelve el DataTable de Departamentos sin filtro.
        /// </summary>
        /// <returns></returns>
        public DataTable listaDepartamentos()
        {
            cargarDepartamentos();
            return dtDepas;
        }

        /// <summary>
        /// Devuelve un DataTable que filtra el DataTable principal de Departamentos a partir de la clase Listar.
        /// </summary>
        /// <param name="filtro">string con el filtro que se quiere aplicar</param>
        /// <returns></returns>
        public DataTable listaDepartamentos(string filtro)
        {
            return Listar.filtrarTabla(dtDepas, filtro);
        }

        /// <summary>
        /// Devuelve un DataTable con los Empleados pertenecientes al Departamento indicado.
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

        /// <summary>
        /// Comprueba que los datos necesarios para crear un departamento estén completos y correctos para llamar 
        /// al modelo Departamento y que este lo inserte en la BBDD.
        /// </summary>
        /// <param name="NombreD"></param>
        /// <param name="DescripcionD"></param>
        /// <returns></returns>
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
                MessageBox.Show("Departamento creado con éxito");
            }
            else
            {
                creado = false;
                MessageBox.Show("Rellene todos los campos necesarios.");
            }

            return creado;
        }

        /// <summary>
        /// Comprueba que los datos del departamento indicado estén completos y correctos para llamar al modelo Departamento
        /// y que este lo actualice.
        /// </summary>
        /// <param name="departamentoModif">DataRow con los datos del Departamento a modificar.</param>
        public void modificarDepartamento(DataRow departamentoModif)
        {
            int.TryParse(departamentoModif["IdDepartamento"].ToString(), out int IdDepartamento);
            string NombreD = departamentoModif["NombreD"].ToString();
            string DescripcionD = departamentoModif["DescripcionD"].ToString();

            if (NombreD != string.Empty && DescripcionD != string.Empty)
            {
                Departamento departamentoModificado = new Departamento()
                {
                    IdDepartamento = IdDepartamento,
                    NombreD = NombreD,
                    DescripcionD = DescripcionD
                };

                departamentoModificado.updateDepartamento(this.Usuario.IdEmpleado);
                MessageBox.Show("Cambios guardados correctamente.");
            }
            else
            {
                MessageBox.Show("Rellene todos los campos necesarios.");
            }
        }

        /// <summary>
        /// Convierte el parámetro SIdDepartemento a int y llama al modelo de departamento para eliminar dicho departamento.
        /// </summary>
        /// <param name="SIdDepartamento">String del id del departamento a eliminar</param>
        public void eliminarDepartamento(string SIdDepartamento)
        {
            int.TryParse(SIdDepartamento, out int IdDepartamento);
            Departamento departamentoBorrado = new Departamento()
            {
                IdDepartamento = IdDepartamento
            };
            departamentoBorrado.deleteDepartamento(this.Usuario.IdEmpleado);
            MessageBox.Show("Departamento eliminado con éxito");
        }

        /// <summary>
        /// Convierte los parámetros a su tipo adecuado y comprueba que el empleado indicado no es jefe de ningún
        /// departamento. Si es así, llama al modelo para que actualice los datos correspondientes a la jefatura de
        /// un departamento e invoca al método que informa al jefe de su nuevo estatus via mail.
        /// </summary>
        /// <param name="SIdEmpleado">Strig del id del empleado que se pretende asignar como jefe.</param>
        /// <param name="SIdDepartamento">String del departamento al que se quiere asignar nuevo jefe.</param>
        /// <param name="NombreD">Nombre del departamento.</param>
        /// <returns></returns>
        public bool asignarJefe(string SIdEmpleado, string SIdDepartamento, string NombreD)
        {
            bool exito = false;

            int.TryParse(SIdEmpleado, out int IdEmpleado);
            int.TryParse(SIdDepartamento, out int IdDepartamento);
            Departamento departamento = new Departamento()
            {
                IdDepartamento = IdDepartamento
            };

                departamento.asignarJefe(IdEmpleado, this.Usuario.IdEmpleado);
                exito = true;
                MessageBox.Show("Jefe de Departamento asignado con éxito.");

                informarAsignacion(IdEmpleado, NombreD);


            return exito;
        }

        /// <summary>
        /// Obtiene el correo de la persona a la que se quiere informar de su nuevo estatus como jefe y llama a la clase
        /// EnviarMail para que realice dicha acción
        /// </summary>
        /// <param name="IdEmpleado">id del nuevo jefe</param>
        /// <param name="NombreD">Nombre del departamento</param>
        private void informarAsignacion(int IdEmpleado, string NombreD)
        {
            string correo = EnviarMail.obtenerMail(IdEmpleado);
            EnviarMail.nuevoJefe(correo, NombreD);
        }

        /// <summary>
        ///Llama al controlador de ventanas para que bloquee la ventana activa, mientras que invoca un contructor del
        /// controlador de la ventana BusquedaEmpleado para abrir una de estas.
        /// </summary>
        public void prepararBusquedaEmpleado()
        {
            ventanaControl.bloquearVActual();
            controladorBusqueda = new BusquedaEmpleadoControlador(this)
            {
                dniBusqueda = string.Empty
            };
        }
        
        /// <summary>
        /// Añade el emlpeado indicado en la ventana de BusquedaEmpleado al departamento indicado e invoca al
        /// método que informa de su adición.
        /// </summary>
        /// <param name="SIdDepartamento">String del id del departamento</param>
        /// <param name="NombreD">Nombre del departamento</param>
        public void aniadirEmpleado(string SIdDepartamento, string NombreD)
        {
            if(controladorBusqueda.dniBusqueda != string.Empty)
            {
                int.TryParse(SIdDepartamento, out int IdDepartamento);

                Departamento departamento = new Departamento()
                {
                    IdDepartamento = IdDepartamento
                };
                if (!Departamento.comprobarJefe(int.Parse(Querys.obtenerIdEmpleado(controladorBusqueda.dniBusqueda))))
                {
                    departamento.addEmpleado(controladorBusqueda.dniBusqueda, this.Usuario.IdEmpleado);

                    MessageBox.Show("Empleado añadido correctamente.");

                    informarAdicion(controladorBusqueda.dniBusqueda, NombreD);
                }
                else
                    MessageBox.Show("El empleado seleccionado ejerce como jefe de otro departamento.");

            }
        }

        /// <summary>
        /// Obtiene el mail del empleado añadido al departamento indicado y llama a la clase EnviarMail para que le 
        /// informe de la adición via mail.
        /// </summary>
        /// <param name="DNI"></param>
        /// <param name="NombreD"></param>
        private void informarAdicion(string DNI, string NombreD)
        {
            string correo = EnviarMail.obtenerMail(DNI);
            EnviarMail.nuevoDepartamento(correo, NombreD);
        }
    }
}
