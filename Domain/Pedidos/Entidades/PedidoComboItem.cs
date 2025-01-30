using Domain.Produtos.Entidades;

namespace Domain.Pedidos.Entidades
{
    public class PedidoComboItem
    {
        public virtual int Codigo { get; protected set; }
        public virtual PedidoCombo PedidoCombo { get; protected set; }
        public virtual Produto Produto { get; protected set; }
        public virtual decimal Preco { get; protected set; }

        public PedidoComboItem() { }

        public PedidoComboItem(PedidoCombo pedidoCombo, Produto produto)
        {
            PedidoCombo = pedidoCombo;
            Produto = produto;
            Preco = produto.Preco;
        }
    }
}
