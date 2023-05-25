using GestionPersonal.Controladores;
using GestionPersonal.Vistas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        
        public DataTable listarEmpleados()
        {
            //Mostramos una tabla en la que no se muestren las contaseñas
            dtEmpleadosCif = Usuario.listadoEmpleados(string.Empty);//OJO CON ESTO DEL USUARIO
            dtEmpleadosCif.Columns.Remove("Contrasenia");

            return dtEmpleadosCif;
        }

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

                Empleado nuevoEmpleado = new Empleado(NombreE,Apellido, Usuario, DNI, NumSS, Tlf, CorreoE, IdDepa);
                this.Usuario.insertEmpleado(nuevoEmpleado);
                MessageBox.Show("Empleado creado correctamente");
            }
            else
            {
                MessageBox.Show("Rellene todos los campos necesarios");
            }

        }


        public void eliminarEmpleado(string UsuarioBorrado)
        {
            Usuario.deleteEmpleado(UsuarioBorrado);//Ahora tenemos el IdModif en Usuario
            MessageBox.Show("Empleado eliminado correctamente");
        }

        public void modificarEmpleado(DataRow empleadoModif, string IdModif)
        {
            empleadoModif["IdModif"] = IdModif;
            empleadoModif["FechaUltModif"] = DateTime.Now; //cambiamos la auditoría

            Usuario.updateEmpleado(empleadoModif);//IdModif en Usuario
            MessageBox.Show("Cambios guardados correctamente.");
            
        }
    }
}
