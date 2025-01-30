using Application.Repository.Clientes;
using Application.Repository.Pedidos;
using Application.Repository.Produtos;
using Application.Transactions.Interfaces;
using Infra.Repository.Clientes;
using Infra.Repository.Pedidos;
using Infra.Repository.Produtos;
using Infra.Transactions;
using Microsoft.Extensions.DependencyInjection;

namespace Infra;

public static class InfraBootstrapper
{
    public static void Register(IServiceCollection services)
    {
        services.AddTransient<IUnitOfWorks, UnitOfWorks>();
        services.AddNHibernate();
        services.AddTransient<IClienteRepository, ClienteRepository>();
        services.AddTransient<IProdutoRepository, ProdutoRepository>();
        services.AddTransient<IPedidoRepository, PedidoRepository>();
        services.AddTransient<IPedidoStatusRepository, PedidoStatusRepository>();
        services.AddTransient<IPedidoPagamentoRepository, PedidoPagamentoRepository>();
        services.AddTransient<IPedidoComboItemRepository, PedidoComboItemRepository>();
        services.AddTransient<IPedidoComboRepository, PedidoComboRepository>();
    }
}
