using Microsoft.AspNetCore.Http.HttpResults;
using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Interfaces;
using minimal_api.Infraestrutura.Db;

namespace minimal_api.Dominio.Sevicos
{
    public class VeiculoServices : IVeiculoServices
    {
        private readonly DbContexto _dbContexto;
        public VeiculoServices(DbContexto dbContexto)
        {
            _dbContexto = dbContexto;
        }

        public void Apagar(Veiculo veiculo)
        {
            _dbContexto.Veiculos.Remove(veiculo);
            _dbContexto.SaveChanges();
            Results.Ok($"{veiculo.Nome} apagado com sucesso!");
        }

        public void Atualizar(Veiculo veiculo)
        {
            _dbContexto.Veiculos.Update(veiculo);
            _dbContexto.SaveChanges();
        }

        public Veiculo? BuscaPorId(int id)
        {
            return _dbContexto.Veiculos.Where(v => v.Id == id).FirstOrDefault();
        }

        public void Incluir(Veiculo veiculo)
        {
            _dbContexto.Veiculos.Add(veiculo);
            _dbContexto.SaveChanges();
        }

        public List<Veiculo> Todos(int pagina = 1, string? nome = null, string? marca = null)
        {
            int tamanhoPagina = 10;
            var query = _dbContexto.Veiculos.AsQueryable();
            if (!string.IsNullOrEmpty(nome))
            {
                query = query.Where(v => v.Nome.Contains(nome));
            }
            if (!string.IsNullOrEmpty(marca))
            {
                query = query.Where(v => v.Marca.Contains(marca));
            }
            query = query
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina);
            return [.. query];
        }
    }
}