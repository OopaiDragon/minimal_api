using Microsoft.VisualStudio.TestTools.UnitTesting;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.Sevicos;
using minimal_api.Infraestrutura.Db;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Test.Domain.Servicos
{
    [TestClass]
    public class AdministradorServicoTest
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
        public void TestandoSalvarAdministrador()
        {
            var context = CriarContextoDeTeste();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores");

            var adm = new Administrador();
            adm.Email = "Teste@exemplo.com";
            adm.Senha = "123456";
            adm.Perfil = "adm";

            var administradorServico = new AdministradorServices(context);
            administradorServico.Incluir(adm);

            Assert.AreEqual(1, administradorServico.Todos(1).Count);
        }

        [TestMethod]
        public void TestandoBuscarPorId()
        {
            var context = CriarContextoDeTeste();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores");

            var adm = new Administrador
            {
                Email = "Teste@exemplo.com",
                Senha = "123456",
                Perfil = "adm"
            };
            var administradorServico = new AdministradorServices(context);

            administradorServico.Incluir(adm);
            var resultado = administradorServico.BuscaPorId(adm.Id);

            Assert.AreEqual(1, resultado?.Id);
        }

        [TestMethod]
        public void TestandoLogin()
        {
            var context = CriarContextoDeTeste();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores");

            var adm = new Administrador
            {
                Email = "Teste@exemplo.com",
                Senha = "123456",
                Perfil = "adm"
            };

            var administradorServico = new AdministradorServices(context);
            administradorServico.Incluir(adm);

            var loginDTO = new LoginDTO
            {
                Email = "Teste@exemplo.com",
                Senha = "123456"
            };

            var resultadoLogin = administradorServico.Login(loginDTO);
            Assert.AreEqual("Teste@exemplo.com", resultadoLogin?.Email);
        }

        [TestMethod]
        public void TestandoTodos()
        {
            var context = CriarContextoDeTeste();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores");

            var administradorServico = new AdministradorServices(context);

            for (int i = 0; i < 15; i++)
            {
                var adm = new Administrador
                {
                    Email = $"teste{i}@exemplo.com",
                    Senha = "123456",
                    Perfil = "adm"
                };
                administradorServico.Incluir(adm);
            }

            var resultadoPagina1 = administradorServico.Todos(1);
            var resultadoPagina2 = administradorServico.Todos(2);

            Assert.AreEqual(10, resultadoPagina1.Count);
            Assert.AreEqual(5, resultadoPagina2.Count);
        }
    }
}
