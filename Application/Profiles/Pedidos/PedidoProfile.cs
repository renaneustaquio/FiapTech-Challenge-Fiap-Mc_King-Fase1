using Application.DataTranfer.Pedidos.Requests;
using Application.DataTranfer.Pedidos.Responses;
using AutoMapper;
using Domain.Clientes.Entidades;
using Domain.Pedidos.Entidades;
using Domain.Produtos.Entidades;

namespace Application.Profiles.Pedidos
{
    public class PedidoProfile : Profile
    {
        public PedidoProfile()
        {
            CreateMap<Pedido, PedidoResponse>()
                .ForMember(dest => dest.PedidoStatus, opt => opt.MapFrom(src => src.PedidoStatus
                                                                .OrderByDescending(ps => ps.DataCriacao)
                                                                .FirstOrDefault())); ;

            CreateMap<PedidoRequest, Pedido>()
                .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src => src.ClienteCodigo.HasValue ? new Cliente(src.ClienteCodigo.Value) : null))
                .ForMember(dest => dest.PedidoCombo, opt => opt.MapFrom(src => src.PedidoCombo));


            CreateMap<PedidoComboRequest, PedidoCombo>()
                .ForMember(dest => dest.PedidoComboItem, opt => opt.MapFrom(src => src.PedidoComboItems));

            CreateMap<PedidoComboItemRequest, PedidoComboItem>()
                .ForMember(dest => dest.Produto, opt => opt.MapFrom(src => new Produto(src.ProdutoCodigo)));

            CreateMap<PedidoStatus, PedidoStatusResponse>();

            CreateMap<PedidoCombo, PedidoComboResponse>();

            CreateMap<PedidoComboItem, PedidoComboItemResponse>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Produto.Nome))
                .ForMember(dest => dest.preco, opt => opt.MapFrom(src => (decimal)src.Preco));

            CreateMap<PedidoPagamento, PedidoPagamentoResponse>();

            CreateMap<PedidoPagamentoRequest, PedidoPagamento>()
                .ForMember(dest => dest.Pedido, opt => opt.Ignore()); ;

            CreateMap<PedidoStatus, PedidoStatusResponse>();
        }
    }
}
