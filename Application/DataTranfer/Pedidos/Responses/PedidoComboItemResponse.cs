namespace Application.DataTranfer.Pedidos.Responses
{
    public class PedidoComboItemResponse
    {
        public int Codigo { get; set; }
        public required string Nome { get; set; }
        public decimal preco { get; set; }
    }
}
