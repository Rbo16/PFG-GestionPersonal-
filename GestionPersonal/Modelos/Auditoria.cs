using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPersonal.Modelos
{
    public class Auditoria
    {
        public DateTime FechaUltModif { get; set; }
        public int IdModif { get; set; }
        public bool Borrado { get; set; }

        public const String Insert_1 = " FechaUltModif, IdModif, Borrado ";
        public const String Insert_2 = " @FechaUltModif, @IdModif, @Borrado ";
        public const String Update = " FechaUltModif=@FechaUltModif, IdModif=@IdModif, Borrado=@Borrado ";

        public Auditoria(int IdModif)
        {
            this.FechaUltModif = DateTime.Now;
            this.IdModif = IdModif;
            this.Borrado = false;
        }
        public Auditoria(int IdModif, bool Borrado)
        {
            this.FechaUltModif = DateTime.Now;
            this.IdModif = IdModif;
            this.Borrado = Borrado;

        }

        public Auditoria(SqlDataReader registro)
        {
            //Hay que indicar la posición de la columna por tema version
            //this.FechaUltModif = registro.GetBoolean("FechaUltModif");
            //this.IdModif = registro.GetInt32("IdModif");
            //this.Borrado = registro.GetDateTime("Borrado");
        }

        public SqlCommand introducirParametros(SqlCommand comando)
        {
            comando.Parameters.Add("@FechaUltModif", SqlDbType.DateTime);
            comando.Parameters.Add("@IdModif", SqlDbType.Int);
            comando.Parameters.Add("@Borrado", SqlDbType.Bit);

            comando.Parameters["@FechaUltModif"].Value = DateTime.Now;
            comando.Parameters["@IdModif"].Value = this.IdModif;
            comando.Parameters["@Borrado"].Value = this.Borrado;

            return comando;
        }
    }
}
