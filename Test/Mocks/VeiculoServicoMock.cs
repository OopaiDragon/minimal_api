using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Interfaces;

namespace Test.Mocks
{
    public class VeiculoServicoMock : IVeiculoServices
    {
        private static List<Veiculo> veiculos = new List<Veiculo>
        {
            new Veiculo { Id = 1, Nome = "Fusca", Marca = "Volkswagen", Ano = 1975 },
            new Veiculo { Id = 2, Nome = "Corolla", Marca = "Toyota", Ano = 2020 }
        };

        public List<Veiculo> Todos(int? pagina = 1, string? nome = null, string? marca = null)
        {
            return veiculos;
        }

        public Veiculo? BuscaPorId(int id)
        {
            return veiculos.FirstOrDefault(v => v.Id == id);
        }

        public void Incluir(Veiculo veiculo)
        {
            veiculo.Id = veiculos.Count() + 1;
            veiculos.Add(veiculo);
        }

        public void Atualizar(Veiculo veiculo)
        {
            var existente = veiculos.FirstOrDefault(v => v.Id == veiculo.Id);
            if (existente != null)
            {
                existente.Nome = veiculo.Nome;
                existente.Marca = veiculo.Marca;
                existente.Ano = veiculo.Ano;
            }
        }

        public void Apagar(Veiculo veiculo)
        {
            veiculos.Remove(veiculo);
        }
    }
}
