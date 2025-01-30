using Domain.Enums;

namespace Application.DataTranfer.Pedidos.Responses
{
    public class PedidoPagamentoResponse
    {
        public decimal Valor { get; set; }
        public DateTime DataPagamento { get; set; }
        public required MetodoPagamentoEnum Metodo { get; set; }
    }
}
