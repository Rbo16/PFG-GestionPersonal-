using GestionPersonal;
using GestionPersonal.Utiles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebasUnitarias
{
    [TestClass]
    public class UnitTestEmpleado
    {
        [TestMethod]
        public void TestInsertEmp()
        {
            Empleado empleado = new Empleado(0)
            {
                NombreE = "Test",
                Apellido = "Test",
                Usuario = "Test",
                Contrasenia = "Test",
                DNI = "123456789",
                NumSS = "123456789012",
                Tlf = "66666666",
                CorreoE = "test@test.com"
            };

            int IdModif = 1;

            empleado.insertEmpleado(IdModif);

            int IdEmpleado = Empleado.maxIdEmpleado();

            Empleado empleadoBBDD = Empleado.obtenerEmpleado(IdEmpleado);

            Assert.AreEqual(empleado.NombreE, empleadoBBDD.NombreE);
            Assert.AreEqual(empleado.Apellido, empleadoBBDD.Apellido);
            Assert.AreEqual(empleado.Usuario, empleadoBBDD.Usuario);
            Assert.AreEqual(empleado.Contrasenia, empleadoBBDD.Contrasenia);
            Assert.AreEqual(empleado.DNI, empleadoBBDD.DNI);
            Assert.AreEqual(empleado.NumSS, empleadoBBDD.NumSS);
            Assert.AreEqual(empleado.Tlf, empleadoBBDD.Tlf);
            Assert.AreEqual(empleado.CorreoE, empleadoBBDD.CorreoE);
            Assert.AreEqual(empleado.rol, empleadoBBDD.rol);
            Assert.AreEqual(empleado.EstadoE, empleadoBBDD.EstadoE);

            Assert.AreEqual(IdModif, empleadoBBDD.Auditoria.IdModif);
            //Assert.AreEqual(empleado.Auditoria.FechaUltModif, empleadoBBDD.Auditoria.FechaUltModif);
            Assert.AreEqual(false, empleadoBBDD.Auditoria.Borrado);
        }

        [TestMethod]
        public void TestUpdateEmp()
        {
            int IdEmpleado = Empleado.maxIdEmpleado();
            Empleado empleado = new Empleado(IdEmpleado)
            {
                NombreE = "UpdateN",
                Apellido = "UpdateA",
                Usuario = "testupdate",
                DNI = "987654321",
                NumSS = "210987654321",
                Tlf = "33333333",
                CorreoE = "testupda@testupda.com"
            };

            int IdModif = 1010;

            empleado.updateEmpleado(IdModif);

            Empleado empleadoBBDD = Empleado.obtenerEmpleado(IdEmpleado);

            Assert.AreEqual(empleado.NombreE, empleadoBBDD.NombreE);
            Assert.AreEqual(empleado.Apellido, empleadoBBDD.Apellido);
            Assert.AreEqual(empleado.Usuario, empleadoBBDD.Usuario);
            Assert.AreEqual(empleado.DNI, empleadoBBDD.DNI);
            Assert.AreEqual(empleado.NumSS, empleadoBBDD.NumSS);
            Assert.AreEqual(empleado.Tlf, empleadoBBDD.Tlf);
            Assert.AreEqual(empleado.CorreoE, empleadoBBDD.CorreoE);
            Assert.AreEqual(empleado.rol, empleadoBBDD.rol);

            Assert.AreEqual(IdModif, empleadoBBDD.Auditoria.IdModif);
            //Assert.AreEqual(empleado.Auditoria.FechaUltModif, empleadoBBDD.Auditoria.FechaUltModif);
            Assert.AreEqual(false, empleadoBBDD.Auditoria.Borrado);
        }

        [TestMethod]
        public void TestUpdateEstadoEmp()
        {
            int IdEmpleado = Empleado.maxIdEmpleado();
            Empleado empleado = new Empleado(IdEmpleado)
            {
                EstadoE = EstadoEmpleado.Autorizado
            };

            int IdModif = 1010;

            empleado.updateEstado(IdModif);

            Empleado empleadoBBDD = Empleado.obtenerEmpleado(IdEmpleado);

            Assert.AreEqual(empleado.EstadoE, empleadoBBDD.EstadoE);

            Assert.AreEqual(IdModif, empleadoBBDD.Auditoria.IdModif);
            //Assert.AreEqual(empleado.Auditoria.FechaUltModif, empleadoBBDD.Auditoria.FechaUltModif);
            Assert.AreEqual(false, empleadoBBDD.Auditoria.Borrado);
        }

        [TestMethod]
        public void TestUpdateContraseniaEmp()
        {
            int IdEmpleado = Empleado.maxIdEmpleado();
            Empleado empleado = new Empleado(IdEmpleado)
            {
                CorreoE = "testupda@testupda.com",
                Contrasenia = "NuevaTest"
            };

            empleado.updateContrasenia();

            Empleado empleadoBBDD = Empleado.obtenerEmpleado(IdEmpleado);

            Assert.AreEqual(empleado.Contrasenia, empleadoBBDD.Contrasenia);

        }

        [TestMethod]
        public void TestDeleteEmp()
        {
            int IdEmpleado = Empleado.maxIdEmpleado();
            Empleado empleado = new Empleado(IdEmpleado);

            int IdModif = 1010;

            empleado.deleteEmpleado(IdModif);

            Empleado empleadoBBDD = Empleado.obtenerEmpleado(IdEmpleado);

            Assert.AreEqual(IdModif, empleadoBBDD.Auditoria.IdModif);
            //Assert.AreEqual(empleado.Auditoria.FechaUltModif, empleadoBBDD.Auditoria.FechaUltModif);
            Assert.AreEqual(true, empleadoBBDD.Auditoria.Borrado);
        }

        [TestMethod]
        public void TestObtenerEmpUsuario()
        {
            Empleado empleado = new Empleado(0)
            {
                Usuario = "seguro"
            };

            empleado.obtenerEmpleado(empleado.Usuario);

            Assert.AreEqual(1006, empleado.IdEmpleado);
            Assert.AreEqual("Seguro", empleado.NombreE);
            Assert.AreEqual("El", empleado.Apellido);
            Assert.AreEqual("EB698BF66118A76945565850075A06065EA311D7EBA7CB4D52CA5227EC24D6F7", empleado.Contrasenia);
            Assert.AreEqual("seguro", empleado.Usuario);
            Assert.AreEqual("000000000", empleado.DNI);
            Assert.AreEqual("000000000000", empleado.NumSS);
            Assert.AreEqual(TipoEmpleado.Basico,empleado.rol);
            Assert.AreEqual("000", empleado.Tlf);
            Assert.AreEqual("0000", empleado.CorreoE);
            Assert.AreEqual(1002, empleado.IdDepartamento);
            Assert.AreEqual(EstadoEmpleado.Autorizado, empleado.EstadoE);

        }

        [TestMethod]
        public void TestLoginOK()
        {
            Empleado empleado = new Empleado(1)
            {
                Usuario = "god2",
                Contrasenia = ConvertidorHASH.GetHashString("root"),
            };

            Assert.AreEqual(true, empleado.iniciarSesion());
        }

        [TestMethod]
        public void TestLoginWrongPassword()
        {
            Empleado empleado = new Empleado(1)
            {
                Usuario = "god2",
                Contrasenia = ConvertidorHASH.GetHashString("mala"),
            };

            Assert.AreEqual(false, empleado.iniciarSesion());
        }

        [TestMethod]
        public void TestLoginUserNotExist()
        {
            Empleado empleado = new Empleado(1)
            {
                Usuario = "inexistente",
                Contrasenia = ConvertidorHASH.GetHashString("mala"),
            };

            Assert.AreEqual(false, empleado.iniciarSesion());
        }

        [TestMethod]
        public void TestLoginUsernotAuthorized()
        {
            Empleado empleado = new Empleado(1)
            {
                Usuario = "Test",
                Contrasenia = "Test",
            };

            Assert.AreEqual(false, empleado.iniciarSesion());
        }

        [TestMethod]
        public void TestExisteCorreo()
        {
            Empleado empleado = new Empleado(0)
            {
                CorreoE = "test@test.com"
            };

            Assert.AreEqual(true, empleado.existeCorreo());
        }
        [TestMethod]
        public void TestExisteCorreoNo()
        {
            Empleado empleado = new Empleado(0)
            {
                CorreoE = "noexiste@test.com"
            };

            Assert.AreEqual(false, empleado.existeCorreo());
        }

        [TestMethod]
        public void TestExisteDNI()
        {
            Empleado empleado = new Empleado(0)
            {
                DNI = "123456789"
            };

            Assert.AreEqual(true, empleado.existeDNI());
        }
        [TestMethod]
        public void TestExisteDNINo()
        {
            Empleado empleado = new Empleado(0)
            {
                DNI = "123123444"
            };

            Assert.AreEqual(false, empleado.existeDNI());
        }


        [TestMethod]
        public void TestExisteUsuario()
        {
            Empleado empleado = new Empleado(0)
            {
                Usuario = "Test"
            };

            Assert.AreEqual(true, empleado.existeUsuario());
        }
        [TestMethod]
        public void TestExisteusuarioNo()
        {
            Empleado empleado = new Empleado(0)
            {
                Usuario = "klkas"
            };

            Assert.AreEqual(false, empleado.existeUsuario());
        }


        [TestMethod]
        public void TestExisteNumSS()
        {
            Empleado empleado = new Empleado(0)
            {
                NumSS = "123456789012"
            };

            Assert.AreEqual(true, empleado.existeNumSS());
        }

        [TestMethod]
        public void TestExisteNumSSNo()
        {
            Empleado empleado = new Empleado(0)
            {
                NumSS = "333444555666"
            };

            Assert.AreEqual(false, empleado.existeNumSS());
        }
    }
}
