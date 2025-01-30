using Application.Repository.Pedidos;
using Domain.Pedidos.Entidades;
using Infra.Repository.Bases;
using NHibernate;

namespace Infra.Repository.Pedidos
{

    public class PedidoStatusRepository(ISession session) : BaseRepository<PedidoStatus>(session), IPedidoStatusRepository
    {
    }

}
