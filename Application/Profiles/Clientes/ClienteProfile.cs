using Application.DataTranfer.Clientes.Requests;
using Application.DataTranfer.Clientes.Responses;
using AutoMapper;
using Domain.Clientes.Entidades;

namespace Application.Profiles.Clientes
{
    public class ClienteProfile : Profile
    {
        public ClienteProfile()
        {
            CreateMap<Cliente, ClienteResponse>();

            CreateMap<ClienteRequest, Cliente>()
                 .ConstructUsing(src => new Cliente(src.Cpf, src.Email, src.Nome));

        }
    }
}
