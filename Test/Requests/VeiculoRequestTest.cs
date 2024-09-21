using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.ModelViews;
using Test.Helpers;

namespace Test.Requests
{
    [TestClass]
    public class VeiculoRequestTest
    {
        private static string? token;

        [ClassInitialize]
        public static async Task ClassInit(TestContext testContext)
        {
            Helpers.Setup.ClassInit(testContext);
            await Autenticar();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Helpers.Setup.ClassCleanup();
        }

        private static async Task Autenticar()
        {
            var loginDTO = new LoginDTO
            {
                Email = "adm@teste.com",
                Senha = "123456"
            };

            var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "application/json");

            var response = await Setup.client.PostAsync("/administradores/login", content);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            var admLogado = JsonSerializer.Deserialize<AdmLogado>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            token = admLogado?.Token;
        }

        private void AdicionarToken()
        {
            if (!string.IsNullOrEmpty(token))
            {
                Setup.client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        [TestMethod]
        public async Task TestarCriarVeiculo()
        {
            string logFilePath = "C:\\Users\\OpaiDragon\\Documents\\GitHub\\test-log.txt";
            File.AppendAllText(logFilePath, "Iniciando o teste de criação de veículo.\n");

            AdicionarToken();

            var veiculoDTO = new VeiculoDTO
            {
                Nome = "Civic",
                Marca = "Honda",
                Ano = 2021
            };

            var content = new StringContent(JsonSerializer.Serialize(veiculoDTO), Encoding.UTF8, "application/json");

            var response = await Setup.client.PostAsync("/veiculos", content);
            Assert.AreEqual(System.Net.HttpStatusCode.Created, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();
            var veiculoCriado = JsonSerializer.Deserialize<Veiculo>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.IsNotNull(veiculoCriado);
            Assert.AreEqual(veiculoDTO.Nome, veiculoCriado.Nome);
            Assert.AreEqual(veiculoDTO.Marca, veiculoCriado.Marca);
            Assert.AreEqual(veiculoDTO.Ano, veiculoCriado.Ano);
            File.AppendAllText(logFilePath, $"ID do Veículo Criado: {veiculoCriado.Id}\n");
        }

        [TestMethod]
        public async Task TestarListarVeiculos()
        {
            AdicionarToken();

            var response = await Setup.client.GetAsync("/veiculos");
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();
            var veiculos = JsonSerializer.Deserialize<List<Veiculo>>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.IsNotNull(veiculos);
            Assert.IsTrue(veiculos.Count > 0);
        }

        [TestMethod]
        public async Task TestarBuscarVeiculoPorId()
        {
            AdicionarToken();

            int id = 3;
            var response = await Setup.client.GetAsync($"/veiculos/{id}");
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();
            var veiculo = JsonSerializer.Deserialize<Veiculo>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.IsNotNull(veiculo);
            Assert.AreEqual(id, veiculo.Id);
            Assert.IsNotNull(veiculo.Nome);
            Assert.IsNotNull(veiculo.Marca);
            Assert.AreEqual(2021, veiculo.Ano);
        }

        [TestMethod]
        public async Task TestarAtualizarVeiculo()
        {
            AdicionarToken();

            int id = 1;
            var veiculoDTO = new VeiculoDTO
            {
                Nome = "Corolla",
                Marca = "Toyota",
                Ano = 2022
            };

            var content = new StringContent(JsonSerializer.Serialize(veiculoDTO), Encoding.UTF8, "application/json");

            var response = await Setup.client.PutAsync($"/veiculos/{id}", content);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();
            var veiculoAtualizado = JsonSerializer.Deserialize<Veiculo>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.IsNotNull(veiculoAtualizado);
            Assert.AreEqual(id, veiculoAtualizado.Id);
            Assert.AreEqual(veiculoDTO.Nome, veiculoAtualizado.Nome);
            Assert.AreEqual(veiculoDTO.Marca, veiculoAtualizado.Marca);
            Assert.AreEqual(veiculoDTO.Ano, veiculoAtualizado.Ano);
        }

        [TestMethod]
        public async Task TestarDeletarVeiculo()
        {
            AdicionarToken();

            int id = 1;

            var response = await Setup.client.DeleteAsync($"/veiculo/{id}");
            Assert.AreEqual(System.Net.HttpStatusCode.NoContent, response.StatusCode);

            var responseAfterDelete = await Setup.client.GetAsync($"/veiculos/{id}");
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, responseAfterDelete.StatusCode);
        }
    }
}
