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
    public class UnitTestDepartamento
    {
        [TestMethod]
        public void TestInsertDep()
        {
            Departamento departamento = new Departamento()
            {
                NombreD = "TestD",
                DescripcionD = "Departamento test insert",
            };

            int IdModif = 1;

            departamento.insertarDepartamento(IdModif);

            int IdDepartamento = Departamento.maxIdDepartamento();

            Departamento departamentoBBDD = Departamento.obtenerDepartamento(IdDepartamento);

            Assert.AreEqual(departamento.NombreD, departamentoBBDD.NombreD);
            Assert.AreEqual(departamento.DescripcionD, departamentoBBDD.DescripcionD);

            Assert.AreEqual(IdModif, departamentoBBDD.Auditoria.IdModif); 
            //Assert.AreEqual(departamento.Auditoria.FechaUltModif, departamentoBBDD.Auditoria.FechaUltModif);
            Assert.AreEqual(false, departamentoBBDD.Auditoria.Borrado);
        }

        [TestMethod]
        public void TestUpdateDepo()
        {
            int IdDepartamento = Departamento.maxIdDepartamento();
            Departamento departamento = new Departamento()
            {
                IdDepartamento = IdDepartamento,
                NombreD = "TestDUpdate",
                DescripcionD = "Departamento test update",
            };

            int IdModif = 1010;

            departamento.updateDepartamento(IdModif);

            Departamento departamentoBBDD = Departamento.obtenerDepartamento(IdDepartamento);

            Assert.AreEqual(departamento.NombreD, departamentoBBDD.NombreD);
            Assert.AreEqual(departamento.DescripcionD, departamentoBBDD.DescripcionD);

            Assert.AreEqual(IdModif, departamentoBBDD.Auditoria.IdModif);
            //Assert.AreEqual(departamento.Auditoria.FechaUltModif, departamentoBBDD.Auditoria.FechaUltModif);
            Assert.AreEqual(false, departamentoBBDD.Auditoria.Borrado);
        }

        [TestMethod]
        public void TestDeleteDep()
        {
            int IdDepartamento = Departamento.maxIdDepartamento();
            Departamento departamento = new Departamento()
            {
                IdDepartamento = IdDepartamento,
            };

            int IdModif = 1010;

            departamento.deleteDepartamento(IdModif);

            Departamento departamentoBBDD = Departamento.obtenerDepartamento(IdDepartamento);

            Assert.AreEqual(IdModif, departamentoBBDD.Auditoria.IdModif);
            //Assert.AreEqual(departamento.Auditoria.FechaUltModif, departamentoBBDD.Auditoria.FechaUltModif);
            Assert.AreEqual(true, departamentoBBDD.Auditoria.Borrado);
        }

        [TestMethod]
        public void TestComprobarJefeOk()
        {
            int IdEmpleado = 1;

            Assert.AreEqual(true, Departamento.comprobarJefe(IdEmpleado));
        }


        [TestMethod]
        public void TestComprobarJefeNo()
        {
            int IdEmpleado = 1002;

            Assert.AreEqual(false, Departamento.comprobarJefe(IdEmpleado));
        }

        [TestMethod]
        public void TestAsignarJefeDep()
        {
            int IdDepartamento = Departamento.maxIdDepartamento();
            Departamento departamento = new Departamento()
            {
                IdDepartamento = IdDepartamento,
            };

            int IdEmpleado = 1002;//Antes de la prueba no era jefe

            int IdModif = 1010;

            departamento.asignarJefe(IdEmpleado,IdModif);

            Assert.AreEqual(true, Departamento.comprobarJefe(IdEmpleado));
        }
    }
}
