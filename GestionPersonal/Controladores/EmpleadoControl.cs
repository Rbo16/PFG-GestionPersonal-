﻿using GestionPersonal.Controladores;
using GestionPersonal.Utiles;
using GestionPersonal.Vistas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
            cargarEmpleados();
            ventanaActiva = new Empleados(this);
            ventanaActiva.Show();
        }

        private void cargarEmpleados()
        {
            //Mostramos una tabla en la que no se muestren las contaseñas
            dtEmpleadosCif = Listar.listarEmpleados();
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
            return Listar.filtrarTabla(dtEmpleadosCif, filtro);
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
        public bool crearEmpleado(string NombreE, string Apellido, string Usuario, string DNI, 
            string NumSS, string Tlf, string CorreoE)
        {
            bool creado = true;

            List<string> listaCampos = new List<string>();
            //Guardamos los campos para comprobar que no estén vacíos

            listaCampos.Add(NombreE);
            listaCampos.Add(Apellido);
            listaCampos.Add(Usuario);
            listaCampos.Add(DNI);
            listaCampos.Add(NumSS);
            listaCampos.Add(Tlf);
            listaCampos.Add(CorreoE);

            if (!camposVacios(listaCampos))
            {
                if (comprobarCaracteres(DNI,NumSS))
                { 
                    //int.TryParse(SIdDepartamento, out int IdDepartamento);
                    Empleado nuevoEmpleado = new Empleado(0)
                    {
                        NombreE = NombreE,
                        Apellido = Apellido,
                        Usuario = Usuario,
                        DNI = DNI,
                        NumSS = NumSS,
                        Tlf = Tlf,
                        CorreoE = CorreoE,
                        //IdDepartamento = IdDepartamento,
                    };

                    nuevoEmpleado.insertEmpleado(this.Usuario.IdEmpleado);
                }
               
            }
            else
            {
                MessageBox.Show("Rellene todos los campos necesarios");
            }

            return creado;
        }


        public void eliminarEmpleado(string SIdEmpleado)
        {
            int.TryParse(SIdEmpleado, out int IdEmpelado);
            Empleado empleadoBorrado = new Empleado(IdEmpelado);
            empleadoBorrado.deleteEmpleado(this.Usuario.IdEmpleado);
        }

        public void modificarEmpleado(DataRow empleadoModif)
        {
            List<string> lCampos = new List<string>();

            for(int i = 1; i < empleadoModif.Table.Rows.Count; i++)
            {
                lCampos.Add(empleadoModif[i].ToString());
            }
            if(!camposVacios(lCampos)) 
            {
                int.TryParse(empleadoModif["IdEmpleado"].ToString(), out int IdEmpleado);
                string NombreE = empleadoModif["NombreE"].ToString();
                string Apellido = empleadoModif["Apellido"].ToString();
                string Usuario = empleadoModif["IdEmpleado"].ToString();
                TipoEmpleado.TryParse(empleadoModif["Rol"].ToString(), out TipoEmpleado rol);
                EstadoEmpleado.TryParse(empleadoModif["EstadoE"].ToString(), out EstadoEmpleado EstadoE);
                string DNI = empleadoModif["DNI"].ToString();
                string NumSS = empleadoModif["NumSS"].ToString();
                string Tlf = empleadoModif["Tlf"].ToString();
                string CorreoE = empleadoModif["CorreoE"].ToString();
                int.TryParse(empleadoModif["IdDepartamento"].ToString(), out int IdDepartamento);

                if (comprobarCaracteres(DNI, NumSS))
                {
                    Empleado empleadoModificado = new Empleado(IdEmpleado)
                    {
                        NombreE = NombreE,
                        Apellido = Apellido,
                        Usuario = Usuario,
                        rol = rol,
                        EstadoE = EstadoE,
                        DNI = DNI,
                        NumSS = NumSS,
                        Tlf = Tlf,
                        CorreoE = CorreoE,
                        IdDepartamento = IdDepartamento,
                    };

                    empleadoModificado.updateEmpleado(this.Usuario.IdEmpleado);

                    MessageBox.Show("Cambios guardados correctamente.");
                }
            }
            else
            {
                MessageBox.Show("Rellene todos los campos necesarios");
            }
        }

        private bool comprobarCaracteres(string DNI, string NumSS)
        {
            bool result = true;

            if (DNI.Trim().Length != 9)
            {
                result = false;
                MessageBox.Show("El DNI ha de tener 9 caracteres.");//HAZ UN METODO PARA USARLO EN UPDATE
            }
            else if (NumSS.Trim().Length != 12)
            {
                result = false;
                MessageBox.Show("El Número de la Seguridad Social ha de tener 12 caracteres.");
            }

            return result;
        }

        public void prepararFiltro()
        {
            this.filtro = string.Empty;
            ventanaControl.bloquearVActual();
            FiltroEmpleadoControl controladorFiltroE = new FiltroEmpleadoControl(this);
        }


    }
}
