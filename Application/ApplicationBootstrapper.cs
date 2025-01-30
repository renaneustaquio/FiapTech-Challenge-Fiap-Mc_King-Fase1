using Application.Services.Clientes;
using Application.Services.Clientes.Interfaces;
using Application.Services.Pedidos;
using Application.Services.Pedidos.Interfaces;
using Application.Services.Produtos;
using Application.Services.Produtos.Intefaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationBootstrapper
{
    public static void Register(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ApplicationBootstrapper));
        services.AddTransient<IClienteService, ClienteService>();
        services.AddTransient<IProdutoService, ProdutoService>();
        services.AddTransient<IPedidoService, PedidoService>();
    }
}
