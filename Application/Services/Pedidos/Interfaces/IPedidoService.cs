using Application.DataTranfer.Pedidos.Requests;
using Application.DataTranfer.Pedidos.Responses;
using Domain.Enums;

namespace Application.Services.Pedidos.Interfaces
{
    public interface IPedidoService
    {
        List<PedidoResponse> Consultar();
        PedidoResponse Consultar(int codigo);
        PedidoResponse Inserir(PedidoRequest pedidoRequest);
        PedidoResponse AlterarStatus(int codigo, StatusPedido statusPedido);
        PedidoResponse ConfirmarPagamento(int codigo, PedidoPagamentoRequest pedidoPagamentoRequest);
        PedidoResponse InserirCombo(int codigo, PedidoComboRequest pedidoComboRequest);
        PedidoResponse RemoverCombo(int codigo, int codigoCombo);
        List<PedidoCozinhaResponse> ObterPedidosCozinha();
        PedidoMonitorResponse ObterPedidosMonitor();
    }

}

