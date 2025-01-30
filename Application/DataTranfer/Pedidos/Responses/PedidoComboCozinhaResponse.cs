namespace Application.DataTranfer.Pedidos.Responses
{
    public class PedidoComboCozinhaResponse
    {
        public int Codigo { get; set; }
        public DateTime DataCriacao { get; set; }
        public TimeSpan TempoEmPreparo { get; set; }
        public required IList<PedidoComboItemCozinhaResponse> PedidoComboItem { get; set; }
    }
}
