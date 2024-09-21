using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using minimal_api.Dominio.Entidades;


namespace Test.Domain.Entidades
{
    [TestClass]
    public class VeiculoTest
    {
        [TestMethod]
        public void TestarGetSetPropriedades()
        {
            var car = new Veiculo();

            car.Id = 1;
            car.Nome = "Model S";
            car.Marca = "Tesla";
            car.Ano = 2024;

            Assert.AreEqual(1, car.Id);
            Assert.AreEqual("Model S", car.Nome);
            Assert.AreEqual("Tesla", car.Marca);
            Assert.AreEqual(2024, car.Ano);
        }
    }
}