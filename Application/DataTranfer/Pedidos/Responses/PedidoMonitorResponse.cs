namespace Application.DataTranfer.Pedidos.Responses
{
    public class PedidoMonitorResponse
    {
        public IList<PedidoStatusMonitorResponse>? PedidoEmPreparacao { get; set; }
        public IList<PedidoStatusMonitorResponse>? PedidoFinalizado { get; set; }
    }
}
