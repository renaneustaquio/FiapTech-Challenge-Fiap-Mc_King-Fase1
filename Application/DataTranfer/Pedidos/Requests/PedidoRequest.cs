namespace Application.DataTranfer.Pedidos.Requests
{
    public class PedidoRequest
    {
        public int? ClienteCodigo { get; set; }
        public required List<PedidoComboRequest> PedidoCombo { get; set; }
    }
}
