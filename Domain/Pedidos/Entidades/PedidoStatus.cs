using Domain.Enums;

namespace Domain.Pedidos.Entidades
{
    public class PedidoStatus
    {
        public virtual int Codigo { get; protected set; }
        public virtual Pedido Pedido { get; protected set; }
        public virtual StatusPedido Status { get; protected set; } = StatusPedido.AguardandoPagamento;
        public virtual DateTime DataCriacao { get; protected set; } = DateTime.Now;

        protected PedidoStatus()
        {
        }

        public PedidoStatus(Pedido pedido, StatusPedido status, DateTime dataCriacao)
        {
            Pedido = pedido;
            Status = status;
            DataCriacao = dataCriacao;
        }
    }
}
