﻿using Domain.Pedidos.Entidades;
using FluentNHibernate.Mapping;

namespace Infra.Map.Pedidos
{
    public class PedidoComboItemMap : ClassMap<PedidoComboItem>
    {
        public PedidoComboItemMap()
        {
            Table("pedido_combo_items");
            Id(x => x.Codigo).Column("codigo").GeneratedBy.Sequence("pedido_combo_items_codigo_seq");
            References(x => x.PedidoCombo).Column("pedido_combo_codigo").Not.Nullable().Cascade.None();
            References(x => x.Produto).Column("codigo_produto").Not.Nullable().Cascade.None();
            Map(x => x.Preco).Column("preco").Not.Nullable();
        }
    }
}
