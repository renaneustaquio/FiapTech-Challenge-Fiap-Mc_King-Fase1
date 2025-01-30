using Application.Repository.Pedidos;
using Domain.Pedidos.Entidades;
using Infra.Repository.Bases;
using NHibernate;

namespace Infra.Repository.Pedidos
{

    public class PedidoPagamentoRepository(ISession session) : BaseRepository<PedidoPagamento>(session), IPedidoPagamentoRepository
    {
    }

}
