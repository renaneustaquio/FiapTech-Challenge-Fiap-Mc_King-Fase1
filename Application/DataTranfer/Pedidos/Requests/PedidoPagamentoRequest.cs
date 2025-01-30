using Domain.Enums;

namespace Application.DataTranfer.Pedidos.Requests
{
    public class PedidoPagamentoRequest
    {
        public decimal Valor { get; set; }
        public DateTime DataPagamento { get; set; }
        public MetodoPagamentoEnum Metodo { get; set; }
    }
}
