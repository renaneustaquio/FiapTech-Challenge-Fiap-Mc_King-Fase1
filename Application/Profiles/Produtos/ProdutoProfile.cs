using Application.DataTranfer.Produtos.Requests;
using Application.DataTranfer.Produtos.Responses;
using AutoMapper;
using Domain.Produtos.Entidades;

namespace Application.Profiles.Produtos
{
    public class ProdutoProfile : Profile
    {
        public ProdutoProfile()
        {
            CreateMap<Produto, ProdutoResponse>();

            CreateMap<ProdutoRequest, Produto>();

            CreateMap<ProdutoFiltroRequest, Produto>();
        }
    }
}
