using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.ModelViews;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Enums;
using Test.Helpers;

namespace Test.Requests
{
    [TestClass]
    public class AdministradorRequestTest
    {
        private static string? token;

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            Helpers.Setup.ClassInit(testContext);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Helpers.Setup.ClassCleanup();
        }

        [TestMethod]
        public async Task TestarGetSetPropriedades()
        {
            var loginDTO = new LoginDTO
            {
                Email = "adm@teste.com",
                Senha = "123456",
            };

            var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "application/json");

            var response = await Setup.client.PostAsync("/administradores/login", content);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();
            var admLogado = JsonSerializer.Deserialize<AdmLogado>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.IsNotNull(admLogado?.Email);
            Assert.IsNotNull(admLogado.Perfil);
            Assert.IsNotNull(admLogado.Token);

            token = admLogado.Token;
        }


        private void AdicionarToken()
        {
            if (!string.IsNullOrEmpty(token))
            {
                Setup.client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        [TestMethod]
        public async Task TestarCriarAdministrador()
        {
            AdicionarToken();

            var administradorDTO = new AdministradorDTO
            {
                Email = "novo@admin.com",
                Senha = "senha123",
                Perfil = Perfil.Adm
            };

            var content = new StringContent(JsonSerializer.Serialize(administradorDTO), Encoding.UTF8, "application/json");

            var response = await Setup.client.PostAsync("/administrador", content);
            Assert.AreEqual(System.Net.HttpStatusCode.Created, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();
            var admCriado = JsonSerializer.Deserialize<AdminstradorModelView>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.IsNotNull(admCriado);
            Assert.AreEqual(administradorDTO.Email, admCriado.Email);
            Assert.AreEqual(administradorDTO.Perfil.ToString(), admCriado.Perfil);
        }

        [TestMethod]
        public async Task TestarListarAdministradores()
        {
            AdicionarToken();

            var response = await Setup.client.GetAsync("/administradores");
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();
            var administradores = JsonSerializer.Deserialize<List<AdminstradorModelView>>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.IsNotNull(administradores);
            Assert.IsTrue(administradores.Count > 0);
        }

        [TestMethod]
        public async Task TestarBuscarAdministradorPorId()
        {
            AdicionarToken();

            int id = 1;
            var response = await Setup.client.GetAsync($"/Administradores/{id}");
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();
            var administrador = JsonSerializer.Deserialize<AdminstradorModelView>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.IsNotNull(administrador);
            Assert.AreEqual(id, administrador.Id);
            Assert.IsNotNull(administrador.Email);
        }
    }
}
