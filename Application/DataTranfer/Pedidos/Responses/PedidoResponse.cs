
using Application.DataTranfer.Clientes.Responses;

namespace Application.DataTranfer.Pedidos.Responses
{
    public class PedidoResponse
    {
        public int Codigo { get; set; }
        public ClienteResponse? Cliente { get; set; }
        public IList<PedidoComboResponse>? PedidoCombo { get; set; }
        public required PedidoStatusResponse PedidoStatus { get; set; }
        public PedidoPagamentoResponse? PedidoPagamento { get; set; }
    }
}
