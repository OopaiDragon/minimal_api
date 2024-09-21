using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using minimal_api.Dominio.Entidades;


namespace Test.Domain.Entidades
{
    [TestClass]
    public class AdministradorTest
    {
        [TestMethod]
        public void TestarGetSetPropriedades()
        {
            var adm = new Administrador();

            adm.Id = 1;
            adm.Email = "Teste@exemplo.com";
            adm.Senha = "123456";
            adm.Perfil = "adm";

            Assert.AreEqual(1, adm.Id);
            Assert.AreEqual("Teste@exemplo.com", adm.Email);
            Assert.AreEqual("123456", adm.Senha);
            Assert.AreEqual("adm", adm.Perfil);
        }
    }
}