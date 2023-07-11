using GestionPersonal;
using GestionPersonal.Modelos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebasUnitarias
{
    [TestClass]
    public class UnitTestProyecto
    {

        private SqlConnection conexionSQL;
        private string cadenaConexion = ConfigurationManager.ConnectionStrings["GestionPersonal.Properties.Settings.masterConnectionString"].ConnectionString;

        [TestMethod]
        public void TestInsertPro()
        {
            Proyecto proyecto = new Proyecto(0)
            {
                NombreP = "TestP",
                Cliente = "Clitest",
                FechaInicioP = DateTime.Parse("11/11/2011"),
                Tiempo = "2 meses",
                Presupuesto = 2222,
                Prioridad = TipoPrioridad.Moderada,
                DescripcionP = "Proyecto test insert"
            };

            int IdProyecto = Proyecto.maxIdProyecto();

            int IdModif = 1;

            proyecto.insertarProyecto(IdModif);

            Proyecto proyectoBBDD = Proyecto.obtenerProyecto(IdProyecto);

            Assert.AreEqual(proyecto.NombreP, proyectoBBDD.NombreP);
            Assert.AreEqual(proyecto.Cliente, proyectoBBDD.Cliente);
            Assert.AreEqual(proyecto.FechaInicioP, proyectoBBDD.FechaInicioP);
            Assert.AreEqual(proyecto.Tiempo, proyectoBBDD.Tiempo);
            Assert.AreEqual(proyecto.Presupuesto, proyectoBBDD.Presupuesto);
            Assert.AreEqual(proyecto.Prioridad, proyectoBBDD.Prioridad);
            Assert.AreEqual(proyecto.DescripcionP, proyectoBBDD.DescripcionP);

            Assert.AreEqual(IdModif, proyectoBBDD.Auditoria.IdModif);
            //Assert.AreEqual(proyecto.Auditoria.FechaUltModif, proyecto.Auditoria.FechaUltModif);
            Assert.AreEqual(false, proyectoBBDD.Auditoria.Borrado);
        }

        [TestMethod]
        public void TestUpdatePro()
        {
            int IdProyecto = Proyecto.maxIdProyecto();
            Proyecto proyecto = new Proyecto(IdProyecto)
            {
                NombreP = "TestPUpdate",
                Cliente = "Clitestupdqate",
                FechaInicioP = DateTime.Parse("13/03/2013"),
                Tiempo = "3 años",
                Presupuesto = 333,
                Prioridad = TipoPrioridad.Urgente,
                DescripcionP = "Proyecto test update"
            };

            int IdModif = 1010;

            proyecto.updateProyecto(IdModif);

            Proyecto proyectoBBDD = Proyecto.obtenerProyecto(IdProyecto);

            Assert.AreEqual(proyecto.NombreP, proyectoBBDD.NombreP);
            Assert.AreEqual(proyecto.Cliente, proyectoBBDD.Cliente);
            Assert.AreEqual(proyecto.FechaInicioP, proyectoBBDD.FechaInicioP);
            Assert.AreEqual(proyecto.Tiempo, proyectoBBDD.Tiempo);
            Assert.AreEqual(proyecto.Presupuesto, proyectoBBDD.Presupuesto);
            Assert.AreEqual(proyecto.Prioridad, proyectoBBDD.Prioridad);
            Assert.AreEqual(proyecto.DescripcionP, proyectoBBDD.DescripcionP);

            Assert.AreEqual(IdModif, proyectoBBDD.Auditoria.IdModif);
            //Assert.AreEqual(proyecto.Auditoria.FechaUltModif, proyecto.Auditoria.FechaUltModif);
            Assert.AreEqual(false, proyectoBBDD.Auditoria.Borrado);
        }

        [TestMethod]
        public void TestDeletePro()
        {
            int IdProyecto = Proyecto.maxIdProyecto();
            Proyecto proyecto = new Proyecto(IdProyecto);

            int IdModif = 1010;

            proyecto.deleteProyecto(IdModif);

            Proyecto proyectoBBDD = Proyecto.obtenerProyecto(IdProyecto);


            Assert.AreEqual(proyecto.FechaFinP, proyectoBBDD.FechaFinP);

            Assert.AreEqual(IdModif, proyectoBBDD.Auditoria.IdModif);
            //Assert.AreEqual(proyecto.Auditoria.FechaUltModif, proyecto.Auditoria.FechaUltModif);
            Assert.AreEqual(true, proyectoBBDD.Auditoria.Borrado);
        }

        [TestMethod]
        public void TestAddEmpleadoPro()
        {
            //Adición del empleado
            int IdEmpleado = 1006;
            string DNI = "000000000";
            int IdProyecto = Proyecto.maxIdProyecto();
            Proyecto proyecto = new Proyecto(IdProyecto);

            int IdModif = 1010;

            proyecto.addEmpleado(DNI, IdModif);

            //Obtención de la participación
            string consulta = "SELECT * FROM ParticipacionProyecto WHERE IdProyecto = @IdProyecto AND IdEmpleado = @IdEmpleado";

            conexionSQL = new SqlConnection(cadenaConexion);
            conexionSQL.Open();

            SqlCommand comando = new SqlCommand(consulta, conexionSQL);

            comando.Parameters.Add("@IdProyecto", SqlDbType.Int);
            comando.Parameters["@IdProyecto"].Value = IdProyecto;

            comando.Parameters.Add("@IdEmpleado", SqlDbType.Int);
            comando.Parameters["@IdEmpleado"].Value = IdEmpleado;

            //Asserts
            SqlDataReader reader = comando.ExecuteReader();
            while (reader.Read())
            {
                Assert.AreEqual(IdEmpleado, reader.GetInt32(1));
                Assert.AreEqual(IdProyecto, reader.GetInt32(2));

                Assert.AreEqual(IdModif, reader.GetInt32(4));
                //Assert.AreEqual(proyecto.Auditoria.FechaUltModif, reader.GetDateTime(3));
                Assert.AreEqual(false, reader.GetBoolean(5));
            }
        }

        [TestMethod]
        public void TestRemoveEmpleadoPro()
        {
            //Eliminación del empleado
            int IdEmpleado = 1006;
            int IdProyecto = Proyecto.maxIdProyecto();
            Proyecto proyecto = new Proyecto(IdProyecto);

            int IdModif = 1010;

            proyecto.removeEmpleado(IdEmpleado, IdModif);

            //Obtención de la participación
            string consulta = "SELECT * FROM ParticipacionProyecto WHERE IdProyecto = @IdProyecto AND IdEmpleado = @IdEmpleado";

            conexionSQL = new SqlConnection(cadenaConexion);
            conexionSQL.Open();

            SqlCommand comando = new SqlCommand(consulta, conexionSQL);

            comando.Parameters.Add("@IdProyecto", SqlDbType.Int);
            comando.Parameters["@IdProyecto"].Value = IdProyecto;

            comando.Parameters.Add("@IdEmpleado", SqlDbType.Int);
            comando.Parameters["@IdEmpleado"].Value = IdEmpleado;

            //Asserts
            SqlDataReader reader = comando.ExecuteReader();
            while (reader.Read())
            {
                Assert.AreEqual(IdModif, reader.GetInt32(4));
                //Assert.AreEqual(proyecto.Auditoria.FechaUltModif, reader.GetDateTime(3));
                Assert.AreEqual(true, reader.GetBoolean(5));
            }
        }

        [TestMethod]
        public void TestParticipaOK()
        {
            string DNI = "555555555";
            Proyecto proyecto = new Proyecto(1);

            Assert.AreEqual(true, proyecto.Participa(DNI));
        }

        [TestMethod]
        public void TestParticipaNo()
        {
            string DNI = "555555555";
            Proyecto proyecto = new Proyecto(2);

            Assert.AreEqual(false, proyecto.Participa(DNI));
        }
    }
}
