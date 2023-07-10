using GestionPersonal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace PruebasUnitarias
{
    [TestClass]
    public class UnitTestAusencia
    {
        [TestMethod]
        public void TestInsertAus()
        {
            Ausencia ausencia = new Ausencia(0)
            {
                IdSolicitante = 1,
                Razon = "Prueba unitaria",
                FechaInicioA = DateTime.Parse("12/10/2001"),
                FechaFinA = DateTime.Parse("13/10/2025"),
                DescripcionAus = "Ausencia de prueba Unitaria",
                JustificantePDF = "Aqui",
            };

            ausencia.insertAusencia();

            int IdAusencia = Ausencia.maxIdAusencia();

            Ausencia ausenciaBBDD = Ausencia.obtenerAusencia(IdAusencia);

            Assert.AreEqual(ausencia.IdSolicitante, ausenciaBBDD.IdSolicitante);
            Assert.AreEqual(ausencia.Razon, ausenciaBBDD.Razon);
            Assert.AreEqual(ausencia.FechaInicioA, ausenciaBBDD.FechaInicioA);
            Assert.AreEqual(ausencia.FechaFinA, ausenciaBBDD.FechaFinA);
            Assert.AreEqual(ausencia.DescripcionAus, ausenciaBBDD.DescripcionAus);
            Assert.AreEqual(ausencia.JustificantePDF, ausenciaBBDD.JustificantePDF);

            Assert.AreEqual(ausencia.IdSolicitante, ausenciaBBDD.Auditoria.IdModif);
            //Assert.AreEqual(ausencia.Auditoria.FechaUltModif, ausenciaBBDD.Auditoria.FechaUltModif);
            Assert.AreEqual(false, ausenciaBBDD.Auditoria.Borrado);
        }

        [TestMethod]
        public void TestUpdateAus1()
        {
            int IdAusencia = Ausencia.maxIdAusencia();
            Ausencia ausencia = new Ausencia(IdAusencia)
            {
                IdSolicitante = 1,
                Razon = "Update",
                FechaInicioA = DateTime.Parse("14/10/2050"),
                FechaFinA = DateTime.Parse("14/10/2063"),
                DescripcionAus = "Update prueba",
                JustificantePDF = "Aqui12",
                EstadoA = EstadoAusencia.Rechazada
            };

            int IdModif = 1010;

            ausencia.updateAusencia(IdModif);

            Ausencia ausenciaBBDD = Ausencia.obtenerAusencia(IdAusencia);

            Assert.AreEqual(ausencia.IdSolicitante, ausenciaBBDD.IdSolicitante);
            Assert.AreEqual(ausencia.Razon, ausenciaBBDD.Razon);
            Assert.AreEqual(ausencia.FechaInicioA, ausenciaBBDD.FechaInicioA);
            Assert.AreEqual(ausencia.FechaFinA, ausenciaBBDD.FechaFinA);
            Assert.AreEqual(ausencia.DescripcionAus, ausenciaBBDD.DescripcionAus);
            Assert.AreEqual(ausencia.JustificantePDF, ausenciaBBDD.JustificantePDF);
            Assert.AreEqual(ausencia.EstadoA, ausenciaBBDD.EstadoA);

            Assert.AreEqual(IdModif, ausenciaBBDD.Auditoria.IdModif);
            //Assert.AreEqual(ausencia.Auditoria.FechaUltModif, ausenciaBBDD.Auditoria.FechaUltModif);
            Assert.AreEqual(false, ausenciaBBDD.Auditoria.Borrado);
        }

        [TestMethod]
        public void TestDeleteAus1()
        {
            int IdAusencia = Ausencia.maxIdAusencia();
            Ausencia ausencia = new Ausencia(IdAusencia);

            int IdModif = 1010;

            ausencia.deleteAusencia(IdModif);

            Ausencia ausenciaBBDD = Ausencia.obtenerAusencia(IdAusencia);

            Assert.AreEqual(IdModif, ausenciaBBDD.Auditoria.IdModif);
            //Assert.AreEqual(ausencia.Auditoria.FechaUltModif, ausenciaBBDD.Auditoria.FechaUltModif);
            Assert.AreEqual(true, ausenciaBBDD.Auditoria.Borrado);
        }

        [TestMethod]
        public void TestUpdateAutorizadorAus1()
        {
            int IdAusencia = Ausencia.maxIdAusencia();
            Ausencia ausencia = new Ausencia(IdAusencia)
            {
                IdAutorizador = 1010,
                EstadoA = EstadoAusencia.Aceptada
            };

            ausencia.updateAutorizador();

            Ausencia ausenciaBBDD = Ausencia.obtenerAusencia(IdAusencia);

            Assert.AreEqual(ausencia.IdAutorizador, ausenciaBBDD.IdAutorizador);
            Assert.AreEqual(ausencia.EstadoA, ausenciaBBDD.EstadoA);

            Assert.AreEqual(ausencia.IdAutorizador, ausenciaBBDD.Auditoria.IdModif);
            //Assert.AreEqual(ausencia.Auditoria.FechaUltModif, ausenciaBBDD.Auditoria.FechaUltModif);
            Assert.AreEqual(false, ausenciaBBDD.Auditoria.Borrado);
        }
    }
}
