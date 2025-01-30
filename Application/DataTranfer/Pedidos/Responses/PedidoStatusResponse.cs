using Domain.Enums;

namespace Application.DataTranfer.Pedidos.Responses
{
    public class PedidoStatusResponse
    {
        public StatusPedido Status { get; protected set; }
        public DateTime DataCriacao { get; protected set; }
    }
}
