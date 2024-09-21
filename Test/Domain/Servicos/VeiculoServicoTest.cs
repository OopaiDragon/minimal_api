using Microsoft.VisualStudio.TestTools.UnitTesting;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Sevicos;
using minimal_api.Infraestrutura.Db;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Test.Domain.Servicos
{
    [TestClass]
    public class VeiculoServicoTest
    {
        private DbContexto CriarContextoDeTeste()
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.GetFullPath(Path.Combine(assemblyPath ?? "", "..", "..", ".."));
            var builder = new ConfigurationBuilder()
                .SetBasePath(path ?? Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.Test.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            return new DbContexto(configuration);
        }


        [TestMethod]
        public void TestandoIncluirVeiculo()
        {
            var context = CriarContextoDeTeste();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Veiculos");

            var veiculo = new Veiculo
            {
                Nome = "Model S",
                Marca = "Tesla",
                Ano = 2022
            };

            var veiculoServico = new VeiculoServices(context);
            veiculoServico.Incluir(veiculo);

            Assert.AreEqual(1, veiculoServico.Todos().Count);
        }

        [TestMethod]
        public void TestandoBuscarPorId()
        {
            var context = CriarContextoDeTeste();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Veiculos");

            var veiculo = new Veiculo
            {
                Nome = "Model S",
                Marca = "Tesla",
                Ano = 2022
            };

            var veiculoServico = new VeiculoServices(context);
            veiculoServico.Incluir(veiculo);

            var resultado = veiculoServico.BuscaPorId(veiculo.Id);
            Assert.AreEqual(1, resultado?.Id);
        }

        [TestMethod]
        public void TestandoAtualizarVeiculo()
        {
            var context = CriarContextoDeTeste();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Veiculos");

            var veiculo = new Veiculo
            {
                Nome = "Model S",
                Marca = "Tesla",
                Ano = 2022
            };

            var veiculoServico = new VeiculoServices(context);
            veiculoServico.Incluir(veiculo);

            veiculo.Nome = "Carro Teste Atualizado";
            veiculoServico.Atualizar(veiculo);

            var resultado = veiculoServico.BuscaPorId(veiculo.Id);
            Assert.AreEqual("Carro Teste Atualizado", resultado?.Nome);
        }

        [TestMethod]
        public void TestandoApagarVeiculo()
        {
            var context = CriarContextoDeTeste();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Veiculos");

            var veiculo = new Veiculo
            {
                Nome = "Model S",
                Marca = "Tesla",
                Ano = 2022
            };

            var veiculoServico = new VeiculoServices(context);
            veiculoServico.Incluir(veiculo);

            veiculoServico.Apagar(veiculo);

            var resultado = veiculoServico.BuscaPorId(veiculo.Id);
            Assert.IsNull(resultado);
        }

        [TestMethod]
        public void TestandoTodos()
        {
            var context = CriarContextoDeTeste();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Veiculos");

            var veiculoServico = new VeiculoServices(context);

            for (int i = 0; i < 15; i++)
            {
                var veiculo = new Veiculo
                {
                    Nome = $"Veiculo {i}",
                    Marca = "Marca Teste"
                };
                veiculoServico.Incluir(veiculo);
            }

            var resultadoPagina1 = veiculoServico.Todos(1);
            var resultadoPagina2 = veiculoServico.Todos(2);

            Assert.AreEqual(10, resultadoPagina1.Count);
            Assert.AreEqual(5, resultadoPagina2.Count);
        }
    }
}
