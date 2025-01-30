namespace Application.DataTranfer.Pedidos.Requests
{
    public class PedidoComboRequest
    {
        public required IList<PedidoComboItemRequest> PedidoComboItems { get; set; }
    }
}
