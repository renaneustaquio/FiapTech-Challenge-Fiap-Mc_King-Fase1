using Domain.Enums;

namespace Application.DataTranfer.Pedidos.Responses
{
    public class PedidoStatusMonitorResponse
    {
        public int PedidoCodigo { get; set; }
        public StatusPedido Status { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
