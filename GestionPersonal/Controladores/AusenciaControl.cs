using GestionPersonal.Controladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GestionPersonal
{
    public class AusenciaControl : Controlador
    {
        private Ausencia ausenciaVacia = new Ausencia();

        public AusenciaControl(VentanaControlador ventanaControl) : base(ventanaControl)
        {
            ventanaActiva = new Ausencias(this);
            ventanaActiva.Show();
        }


        /// <summary>
        /// Método para crear la ausencia e insertarla en la BBDD
        /// </summary>
        /// <param name="Razon">Razón de la ausencia</param>
        /// <param name="FechaInicioA">Fecha de inicio de la ausencia</param>
        /// <param name="FechaFinA">Fecha fin de la ausencia</param>
        /// <param name="DescripcionAus">Descripción de la ausencia</param>
        /// <param name="JustificantePDF">Ruta donde se ubica el justificante de la ausencia</param>
        public bool crearAusencia(string Razon, DateTime FechaInicioA, DateTime FechaFinA, string DescripcionAus, string JustificantePDF) 
        {
            bool creado = false;
            
            List<string> listaCampos = new List<string>();

            listaCampos.Add(Razon);
            listaCampos.Add(FechaInicioA.ToString());
            listaCampos.Add(FechaFinA.ToString());

            if (!camposVacios(listaCampos))
            {
                if (FechaInicioA < FechaFinA) 
                {
                    Ausencia nuevaAusencia = new Ausencia(Razon, FechaInicioA, FechaFinA, DescripcionAus, JustificantePDF, Usuario.IdEmpleado);
                    nuevaAusencia.insertAusencia();

                    MessageBox.Show("Ausencia solicitada correctamente.");
                    creado = true;
                }
                else
                {
                    MessageBox.Show("La fecha fin ha de ser mayor que la fecha incicio.");
                }
            }
            else
            {
                MessageBox.Show("Rellene todos los campos necesarios.");
            }

            return creado;
        }


        public DataTable listarAusencias(int IdSolicitante)
        {
            return ausenciaVacia.listarAusencias(IdSolicitante); ;
        }

    }
}
