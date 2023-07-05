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

        /// <summary>
        /// Devuelve el comando proporcionado con los parámetros de la Auditoría completos.
        /// </summary>
        /// <param name="comando">comando sql al que se quieren añadir los parámetros</param>
        /// <returns></returns>
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
