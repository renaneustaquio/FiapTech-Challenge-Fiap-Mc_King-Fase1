using Application.DataTranfer.Clientes.Requests;
using Application.DataTranfer.Clientes.Responses;
using Application.Repository.Clientes;
using Application.Services.Clientes.Interfaces;
using Application.Transactions.Interfaces;
using AutoMapper;
using Domain.Clientes.Entidades;
using Domain.Util;

namespace Application.Services.Clientes
{
    public class ClienteService(IMapper mapper, IClienteRepository clienteRepository, IUnitOfWorks unitOfWorks) : IClienteService
    {
        public ClienteResponse Inserir(ClienteRequest clienteRequest)
        {
            try
            {
                unitOfWorks.Begintransaction();

                var clienteCadastrado = clienteRepository.Consultar()
                                                         .Where(x => x.Cpf == clienteRequest.Cpf)
                                                         .FirstOrDefault();

                if (clienteCadastrado != null)
                    throw new RegraNegocioException("Cpf já cadastrado");

                var cliente2 = mapper.Map<Cliente>(clienteRequest);


                clienteRepository.Inserir(cliente2);

                unitOfWorks.Commit();

                return mapper.Map<ClienteResponse>(cliente2);
            }
            catch
            {
                unitOfWorks.RollBack();
                throw;
            }

        }

        public List<ClienteResponse> Consultar()
        {
            try
            {
                var clientes = clienteRepository.Recuperar();

                var clienteResponse = mapper.Map<List<ClienteResponse>>(clientes);

                return clienteResponse;
            }
            catch
            {
                throw;
            }
        }

        public ClienteResponse FiltrarClientePorCpf(ClienteFiltroRequest clienteFiltroRequest)
        {
            try
            {
                var cliente = clienteRepository.Recuperar()
                                               .Where(x => x.Cpf == clienteFiltroRequest.Cpf.ApenasNumeros())
                                               .FirstOrDefault() ?? throw new RegraNegocioException("Cpf não cadastrado");

                var clienteResponse = mapper.Map<ClienteResponse>(cliente);

                return clienteResponse;
            }
            catch
            {
                throw;
            }
        }
    }
}
