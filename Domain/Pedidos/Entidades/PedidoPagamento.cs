using Domain.Enums;

namespace Domain.Pedidos.Entidades
{
    public class PedidoPagamento
    {
        public virtual int Codigo { get; protected set; }
        public virtual Pedido Pedido { get; protected set; }
        public virtual decimal Valor { get; protected set; }
        public virtual DateTime DataPagamento { get; protected set; }
        public virtual MetodoPagamentoEnum Metodo { get; protected set; } = MetodoPagamentoEnum.MercadoPago;

        protected PedidoPagamento()
        {
        }
        public virtual void SetPedido(Pedido pedido)
        {
            Pedido = pedido;
        }
    }
}
