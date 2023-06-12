﻿using GestionPersonal.Controladores;
using GestionPersonal.Utiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestionPersonal
{
    public  class DepartamentoControl : Controlador
    {
        DataTable dtDepas;

        public DepartamentoControl(VentanaControlador ventanaControl) : base(ventanaControl) 
        {
            ventanaActiva = new Departamentos(this);
            ventanaActiva.Show();
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

        public DataTable listarDepartamentos()
        {
            dtDepas = Listar.listarDepartamentos();
            return dtDepas;
        }

    }
}
