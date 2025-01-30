using Application.Repository.Clientes;
using Domain.Clientes.Entidades;
using Infra.Repository.Bases;
using NHibernate;

namespace Infra.Repository.Clientes
{
    public class ClienteRepository(ISession session) : BaseRepository<Cliente>(session), IClienteRepository
    {
    }
}
