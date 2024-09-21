using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Interfaces;
using minimal_api.Infraestrutura.Db;

namespace minimal_api.Dominio.Sevicos
{
    public class AdministradorServices : IAdministradorServices
    {
        private readonly DbContexto _dbContexto;
        public AdministradorServices(DbContexto dbContexto)
        {
            _dbContexto = dbContexto;
        }

        public Administrador? BuscaPorId(int id)
        {
            return _dbContexto.Administradores.Where(v => v.Id == id).FirstOrDefault();
        }

        public Administrador Incluir(Administrador administrador)
        {
            _dbContexto.Administradores.Add(administrador);
            _dbContexto.SaveChanges();
            return administrador;
        }

        public Administrador? Login(LoginDTO loginDTO)
        {
            var adm = _dbContexto.Administradores.Where(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha).FirstOrDefault();
            return adm;
        }

        public List<Administrador> Todos(int? pagina)
        {
            var querry = _dbContexto.Administradores.AsQueryable();
            int itensPorPagina = 10;
            if (pagina != null)
            {
                querry = querry.Skip(((int)pagina - 1) * itensPorPagina).Take(itensPorPagina);
            }
            return querry.ToList();
        }
    }
}