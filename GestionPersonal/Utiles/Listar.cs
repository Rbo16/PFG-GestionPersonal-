using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPersonal.Utiles
{
    public class Listar
    {
        private static SqlConnection conexionSQL;
        private static string cadenaConexion = ConfigurationManager.ConnectionStrings["GestionPersonal.Properties.Settings.masterConnectionString"].ConnectionString;

        public static DataTable listarEmpleados()
        {
            try
            {
                DataTable dtEmpleados = new DataTable();
                string consulta = "SELECT Empleado.*, Departamento.NombreD, EnumEstadoEmpleado.EstadoE AS Estado " +
                    "FROM Empleado LEFT JOIN Departamento ON Empleado.IdDepartamento = Departamento.IdDepartamento " +
                    "LEFT JOIN EnumEstadoEmpleado ON Empleado.EstadoE = EnumEstadoEmpleado.IdEstadoE " +
                    "WHERE Empleado.Borrado = 0";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                SqlDataAdapter adaptadorSql = new SqlDataAdapter(comando);
                using (adaptadorSql)
                {
                    adaptadorSql.Fill(dtEmpleados);
                }

                return dtEmpleados;
            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Empleado.Listar]");
                return null;
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        public static DataTable listarAusencias()
        {
            try
            {
                DataTable dtAusencias = new DataTable();
                string consulta = "SELECT Ausencia.*,  CONCAT(Empleado.NombreE,' ',Empleado.Apellido) AS Solicitante, Empleado.DNI,  " +
                    "CONCAT(Empleado1.NombreE,' ',Empleado1.Apellido) AS Autorizador, EnumEstadoAusencia.EstadoA as Estado " +
                    "FROM Ausencia LEFT JOIN Empleado ON Ausencia.IdSolicitante = Empleado.IdEmpleado " +
                    "LEFT JOIN Empleado as Empleado1 ON Ausencia.IdAutorizador = Empleado1.IdEmpleado " +
                    "LEFT JOIN EnumEstadoAusencia ON Ausencia.EstadoA = EnumEstadoAusencia.IdEstadoA ";//WHERE Borrado = 0";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                SqlDataAdapter adaptadorSql = new SqlDataAdapter(comando);
                using (adaptadorSql)
                {
                    adaptadorSql.Fill(dtAusencias);
                }
                return dtAusencias;
            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Ausencia.Listar]");
                return null;
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        public static DataTable listarContratos()
        {
            try
            {
                DataTable dtContratos = new DataTable();
                string consulta = "SELECT Contrato.*,  Empleado.DNI, Empleado.NombreE, Empleado.Apellido " +
                    "FROM Contrato LEFT JOIN Empleado ON Contrato.IdEmpleado = Empleado.IdEmpleado";//WHERE Borrado = 0";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                SqlDataAdapter adaptadorSql = new SqlDataAdapter(comando);
                using (adaptadorSql)
                {
                    adaptadorSql.Fill(dtContratos);
                }

                return dtContratos;
            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Contrato.Listar]");
                return null;
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        public static DataTable listarProyectos()
        {
            try
            {
                DataTable dtProyectos = new DataTable();

                string consulta = "SELECT Proyecto.*, ParticipacionProyecto.IdEmpleado FROM Proyecto " +
                    "LEFt JOIN ParticipacionProyecto on Proyecto.IdProyecto = ParticipacionProyecto.IdProyecto";//+WHERE Borrado = 0";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                SqlDataAdapter adaptadorSql = new SqlDataAdapter(comando);
                using (adaptadorSql)
                {
                    adaptadorSql.Fill(dtProyectos);
                }

                return dtProyectos;

            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Proyecto.Listar]:");
                return null;
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        public static DataTable listarParticipacionProyectos()
        {
            try
            {
                DataTable dtParticipacion = new DataTable();

                string consulta = "SELECT ParticipacionProyecto.IdProyecto, Empleado.* FROM ParticipacionProyecto " +
                    "LEFT JOIN Empleado ON ParticipacionProyecto.IdEmpleado = Empleado.IdEmpleado";//+WHERE Borrado = 0";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                SqlDataAdapter adaptadorSql = new SqlDataAdapter(comando);
                using (adaptadorSql)
                {
                    adaptadorSql.Fill(dtParticipacion);
                }

                return dtParticipacion;

            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Participacion.Listar]:");
                return null;
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        public static DataTable listarDepartamentos()
        {
            try
            {
                DataTable dtDepas = new DataTable();

                string consulta = "SELECT Departamento.*, Empleado.IdEmpleado, Empleado.NombreE, Empleado.Apellido " +
                    "FROM Departamento LEFT JOIN Empleado ON Departamento.IdJefeDep = Empleado.IdEmpleado";//WHERE Borrado = 0";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                SqlDataAdapter adaptadorSql = new SqlDataAdapter(comando);
                using (adaptadorSql)
                {
                    adaptadorSql.Fill(dtDepas);
                }

                return dtDepas;

            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Departamento.Listar]:");
                return null;
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        /// <summary>
        /// Devuelve la tabla dada filtrada según la cadena porporcionada
        /// </summary>
        /// <param name="dtAux">Tabla a filtrar</param>
        /// <param name="filtro">Cadena con el filtro que se quiere aplicar</param>
        /// <returns></returns>
        public static DataTable filtrarTabla(DataTable dtAux, string filtro)
        {
            DataTable dtFiltro = dtAux.Clone();

            DataRow[] filasFiltradas = dtAux.Select(filtro);
            foreach (DataRow fila in filasFiltradas)
            {
                dtFiltro.ImportRow(fila);
            }

            return dtFiltro;
        }
    }
}
