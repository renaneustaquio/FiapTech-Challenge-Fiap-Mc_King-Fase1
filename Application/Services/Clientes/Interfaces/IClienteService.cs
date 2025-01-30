using Application.DataTranfer.Clientes.Requests;
using Application.DataTranfer.Clientes.Responses;

namespace Application.Services.Clientes.Interfaces
{
    public interface IClienteService
    {
        ClienteResponse Inserir(ClienteRequest clienteRequest);
        List<ClienteResponse> Consultar();
        ClienteResponse FiltrarClientePorCpf(ClienteFiltroRequest clienteFiltroRequest);
    }
}
