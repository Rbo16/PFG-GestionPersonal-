using GestionPersonal.Modelos;
using GestionPersonal.Utiles;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPersonal
{
    public class Departamento
    {
        private SqlConnection conexionSQL;
        private string cadenaConexion = ConfigurationManager.ConnectionStrings["GestionPersonal.Properties.Settings.masterConnectionString"].ConnectionString;

        public int IdDepartamento { get; set; }
        public string NombreD { get; set; }
        public string DescripcionD { get; set; }
        public int IdJefeDep { get; set; }

        private Auditoria Auditoria { get; set; }

        public Departamento()
        {
            IdDepartamento = 0;
            NombreD = string.Empty;
            DescripcionD = string.Empty;
            IdJefeDep = 0;
        }

        public void insertarDepartamento(int IdModif)
        {
            try
            {
                string consulta = "INSERT INTO Departamento (NombreD, DescripcionD, " + Auditoria.Insert_1 +
                                  ") VALUES (@NombreD, @DescripcionD, " + Auditoria.Insert_2 + ")";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@NombreD", SqlDbType.NVarChar);
                comando.Parameters.Add("@DescripcionD", SqlDbType.NVarChar);

                comando.Parameters["@NombreD"].Value = this.NombreD;
                comando.Parameters["@DescripcionD"].Value = this.DescripcionD;

                this.Auditoria = new Auditoria(IdModif, true);
                comando = Auditoria.introducirParametros(comando);

                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Departamento.Borrar]:");
            }
            finally
            {
                conexionSQL.Close();
            }
        }

        public void updateDepartamento(int IdModif)
        {
            try
            {
                string consulta = "UPDATE Departamento SET NombreD = @NombreD, DescripcionD = @DescripcionD, " 
                    + Auditoria.Update + " WHERE IdDepartamento = @IdDepartamento";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@NombreD", SqlDbType.NVarChar);
                comando.Parameters.Add("@DescripcionD", SqlDbType.NVarChar);
                comando.Parameters.Add("@IdDepartamento", SqlDbType.Int);

                comando.Parameters["@NombreD"].Value = this.NombreD;
                comando.Parameters["@DescripcionD"].Value = this.DescripcionD;
                comando.Parameters["@IdDepartamento"].Value = this.IdDepartamento;

                this.Auditoria = new Auditoria(IdModif);
                comando = Auditoria.introducirParametros(comando);

                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Departamento.Modificar]:");
            }
            finally
            {
                conexionSQL.Close();
            }
        }

    public void deleteDepartamento(int IdModif)
        {
            try
            {
                string consulta = "UPDATE Departamento SET " + Auditoria.Update + "WHERE IdDepartamento = @IdDepartamento";

                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();

                SqlCommand comando = new SqlCommand(consulta, conexionSQL);

                comando.Parameters.Add("@IdDepartamento", SqlDbType.Int);
                comando.Parameters["@IdDepartamento"].Value = this.IdDepartamento;

                this.Auditoria = new Auditoria(IdModif, true);
                comando = Auditoria.introducirParametros(comando);

                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ExceptionManager.Execute(ex, "ERROR[Departamento.Borrar]:");
            }
            finally
            {
                conexionSQL.Close();
            }
        }
    }
}
