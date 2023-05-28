using GestionPersonal.Controladores;
using GestionPersonal.Vistas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace GestionPersonal
{
    public class EmpleadoControl : Controlador
    {
        DataTable dtEmpleadosCif = new DataTable();
        public EmpleadoControl(VentanaControlador ventanaControl) : base(ventanaControl)
        {
            ventanaActiva = new Empleados(this);
            ventanaActiva.Show();
        }
        
        private void cargarEmpleados()
        {
            //Mostramos una tabla en la que no se muestren las contaseñas
            dtEmpleadosCif = Usuario.listadoEmpleados();
            dtEmpleadosCif.Columns.Remove("Contrasenia");
        }


        /// <summary>
        /// Devuelve el DataTable de Empleados sin filtro
        /// </summary>
        /// <returns></returns>
        public DataTable listaEmpleados() 
        {
            cargarEmpleados();
            return dtEmpleadosCif;
        }

        /// <summary>
        /// Devuelve un DataTable que filtra el DataTable principal de Empleados
        /// </summary>
        /// <param name="filtro">string con el filtro que se quiere aplicar</param>
        /// <returns></returns>
        public DataTable listaEmpleados(string filtro)
        {

            DataTable dtAux = dtEmpleadosCif.Clone();

            DataRow [] filasFiltradas = dtEmpleadosCif.Select(filtro);
            foreach(DataRow fila in filasFiltradas)
            {
                dtAux.ImportRow(fila);
            }

            return dtAux;
        }

        /// <summary>
        /// Devuelve un array con los elementos del tipo enumerado de Empleado que se indique
        /// </summary>
        /// <param name="NombreEnum">TipoEmpleado o EstadoEmpleado</param>
        /// <returns></returns>
        public Array devolverEnum(string NombreEnum)
        {
            Array array = Array.Empty<Array>();
            if (NombreEnum == "TipoEmpleado")
                array = Enum.GetValues(typeof(TipoEmpleado));
            if (NombreEnum == "EstadoEmpleado")
                array = Enum.GetValues(typeof(EstadoEmpleado));
            return array;
        }
        public void crearEmpleado(string NombreE, string Apellido, string Usuario, string DNI, 
            string NumSS, string Tlf, string CorreoE, string IdDepartamento, string IdModif)
        {
            List<string> listaCampos = new List<string>();
            //Guardamos los campos para comprobar que no estén vacíos

            listaCampos.Add(NombreE);
            listaCampos.Add(Apellido);
            listaCampos.Add(Usuario);
            listaCampos.Add(DNI);
            listaCampos.Add(NumSS);
            listaCampos.Add(Tlf);
            listaCampos.Add(CorreoE);
            listaCampos.Add(IdDepartamento);//Se comprobará que se pueda pasar a int

            if (!camposVacios(listaCampos))
            {
                int IdDepa = 0;
                try
                {
                    IdDepa = Convert.ToInt32(IdDepartamento);//Comprobamos de IdDep sea un número 
                }
                catch
                {
                    MessageBox.Show("El Id del Departamento ha de ser un número");
                    return;
                }

                Empleado nuevoEmpleado = new Empleado(NombreE,Apellido, Usuario, DNI, NumSS, Tlf, CorreoE, IdDepa,
                    this.Usuario.IdEmpleado);
                nuevoEmpleado.insertEmpleado();
                MessageBox.Show("Empleado creado correctamente");
            }
            else
            {
                MessageBox.Show("Rellene todos los campos necesarios");
            }

        }


        public void eliminarEmpleado(string UsuarioBorrado)
        {
            Usuario.deleteEmpleado(UsuarioBorrado, this.Usuario.IdEmpleado);
            MessageBox.Show("Empleado eliminado correctamente");
        }

        public void modificarEmpleado(DataRow empleadoModif)
        {
            int IdEmpleado = Convert.ToInt32(empleadoModif["IdEmpleado"].ToString());
            string NombreE = empleadoModif["NombreE"].ToString();
            string Apellido = empleadoModif["Apellido"].ToString();
            string Usuario = empleadoModif["IdEmpleado"].ToString();
            TipoEmpleado.TryParse(empleadoModif["Rol"].ToString(), out TipoEmpleado Rol);
            EstadoEmpleado.TryParse(empleadoModif["EstadoE"].ToString(), out EstadoEmpleado EstadoE);
            string DNI = empleadoModif["DNI"].ToString();
            string NumSS = empleadoModif["NumSS"].ToString();
            string Tlf = empleadoModif["Tlf"].ToString();
            string CorreoE = empleadoModif["CorreoE"].ToString();
            int IdDepartamento = Convert.ToInt32(empleadoModif["IdDepartamento"].ToString());

            Empleado empleadoModificado = new Empleado(IdEmpleado, NombreE, Apellido, Usuario, Rol, EstadoE, DNI, 
                NumSS, Tlf, CorreoE, IdDepartamento, this.Usuario.IdEmpleado);

            empleadoModificado.updateEmpleado();

            MessageBox.Show("Cambios guardados correctamente.");
            
        }
    }
}
