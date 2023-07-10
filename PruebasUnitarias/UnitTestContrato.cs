using GestionPersonal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebasUnitarias
{
    [TestClass]
    public class UnitTestContrato
    {
        [TestMethod]
        public void TestInsertCont()
        {
            Contrato contrato = new Contrato(0)
            {
                IdEmpleado = 1006,
                HorasTrabajo = 80,
                HorasDescanso = 0.5,
                HoraEntrada = TimeSpan.Parse("12:00:00"),
                HoraSalida = TimeSpan.Parse("21:00:00"),
                Salario = 5555,
                Puesto = "tester",
                VacacionesMes = 1.5,
                Duracion = "6 meses",
                TipoContrato = TipoContrato.ParcialTarde,
                DocumentoPDF = "Aqui prueba",
            };

            int IdModif = 1;

            contrato.insertContrato(IdModif);

            int IdContrato = Contrato.maxIdContrato();

            Contrato contratoBBDD = Contrato.obtenerContrato(IdContrato);

            Assert.AreEqual(contrato.IdEmpleado, contratoBBDD.IdEmpleado);
            Assert.AreEqual(contrato.HorasTrabajo, contratoBBDD.HorasTrabajo);
            Assert.AreEqual(contrato.HorasDescanso, contratoBBDD.HorasDescanso);
            Assert.AreEqual(contrato.HoraEntrada, contratoBBDD.HoraEntrada);
            Assert.AreEqual(contrato.HoraSalida, contratoBBDD.HoraSalida);
            Assert.AreEqual(contrato.Salario, contratoBBDD.Salario);
            Assert.AreEqual(contrato.Puesto, contratoBBDD.Puesto);
            Assert.AreEqual(contrato.VacacionesMes, contratoBBDD.VacacionesMes);
            Assert.AreEqual(contrato.FechaAlta, contratoBBDD.FechaAlta);
            Assert.AreEqual(contrato.Duracion, contratoBBDD.Duracion);
            Assert.AreEqual(contrato.TipoContrato, contratoBBDD.TipoContrato);
            Assert.AreEqual(contrato.DocumentoPDF, contratoBBDD.DocumentoPDF);
            Assert.AreEqual(true, contratoBBDD.Activo);

            Assert.AreEqual(IdModif, contratoBBDD.Auditoria.IdModif);
            //Assert.AreEqual(contrato.Auditoria.FechaUltModif, contratoBBDD.Auditoria.FechaUltModif);
            Assert.AreEqual(false, contratoBBDD.Auditoria.Borrado);

        }

        [TestMethod]
        public void TestUpdateCont()
        {
            int IdContrato = Contrato.maxIdContrato();
            Contrato contrato = new Contrato(IdContrato)
            {
                HorasTrabajo = 333,
                HorasDescanso = 3.3,
                HoraEntrada = TimeSpan.Parse("10:33:33"),
                HoraSalida = TimeSpan.Parse("21:33:33"),
                Salario = 666,
                Puesto = "tester update",
                VacacionesMes = 3.3,
                Duracion = "Indefinido",
                TipoContrato = TipoContrato.Completa,
                DocumentoPDF = "Aqui prueba update",
            };

            int IdModif = 1010;

            contrato.updateContrato(IdModif);

            Contrato contratoBBDD = Contrato.obtenerContrato(IdContrato);

            Assert.AreEqual(contrato.HorasTrabajo, contratoBBDD.HorasTrabajo);
            Assert.AreEqual(contrato.HorasDescanso, contratoBBDD.HorasDescanso);
            Assert.AreEqual(contrato.HoraEntrada, contratoBBDD.HoraEntrada);
            Assert.AreEqual(contrato.HoraSalida, contratoBBDD.HoraSalida);
            Assert.AreEqual(contrato.Salario, contratoBBDD.Salario);
            Assert.AreEqual(contrato.Puesto, contratoBBDD.Puesto);
            Assert.AreEqual(contrato.VacacionesMes, contratoBBDD.VacacionesMes);
            Assert.AreEqual(contrato.Duracion, contratoBBDD.Duracion);
            Assert.AreEqual(contrato.TipoContrato, contratoBBDD.TipoContrato);
            Assert.AreEqual(contrato.DocumentoPDF, contratoBBDD.DocumentoPDF);

            Assert.AreEqual(IdModif, contratoBBDD.Auditoria.IdModif);
            //Assert.AreEqual(contrato.Auditoria.FechaUltModif, contratoBBDD.Auditoria.FechaUltModif);
            Assert.AreEqual(false, contratoBBDD.Auditoria.Borrado);

        }

        [TestMethod]
        public void TestDeleteCont()
        {
            int IdContrato = Contrato.maxIdContrato();
            Contrato contrato = new Contrato(IdContrato);

            int IdModif = 1010;

            contrato.deleteContrato(IdModif);

            Contrato contratoBBDD = Contrato.obtenerContrato(IdContrato);

            Assert.AreEqual(IdModif, contratoBBDD.Auditoria.IdModif);
            //Assert.AreEqual(contrato.Auditoria.FechaUltModif, contratoBBDD.Auditoria.FechaUltModif);
            Assert.AreEqual(true, contratoBBDD.Auditoria.Borrado);

        }

        [TestMethod]
        public void TestCambiarActivoCont()
        {
            int IdContrato = Contrato.maxIdContrato();
            Contrato contrato = new Contrato(IdContrato)
            {
                IdEmpleado = 1006
            };

            contrato.cambiarActivo();

            Contrato contratoBBDD = Contrato.obtenerContrato(IdContrato);

            Assert.AreEqual(contrato.FechaBaja.Date, contratoBBDD.FechaBaja.Date);
            Assert.AreEqual(false, contratoBBDD.Activo);

        }
    }

}
