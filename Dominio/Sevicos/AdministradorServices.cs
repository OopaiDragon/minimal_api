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

        public Administrador? login(LoginDTO loginDTO)
        {
            var adm = _dbContexto.Administradores.Where(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha).FirstOrDefault();
            return adm;
        }
    }
}