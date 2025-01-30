namespace Application.DataTranfer.Pedidos.Responses
{
    public class PedidoCozinhaResponse
    {
        public int Pedido { get; set; }
        public required IList<PedidoComboCozinhaResponse> PedidoCombo { get; set; }
    }
}
