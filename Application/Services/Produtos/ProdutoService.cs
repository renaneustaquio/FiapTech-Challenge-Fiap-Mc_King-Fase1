using Application.DataTranfer.Produtos.Requests;
using Application.DataTranfer.Produtos.Responses;
using Application.Repository.Pedidos;
using Application.Repository.Produtos;
using Application.Services.Produtos.Intefaces;
using Application.Transactions.Interfaces;
using AutoMapper;
using Domain.Produtos.Entidades;
using Domain.Util;

namespace Application.Services.Produtos
{
    public class ProdutoService(IMapper mapper, IProdutoRepository produtoRepository, IPedidoComboItemRepository pedidoComboItemRepository, IUnitOfWorks unitOfWorks) : IProdutoService
    {
        public ProdutoResponse Inserir(ProdutoRequest produtoRequest)
        {
            try
            {
                unitOfWorks.Begintransaction();

                var produto = mapper.Map<Produto>(produtoRequest);

                produtoRepository.Inserir(produto);

                unitOfWorks.Commit();

                return mapper.Map<ProdutoResponse>(produto);
            }
            catch
            {
                unitOfWorks.RollBack();
                throw;
            }
        }

        public ProdutoResponse Alterar(int codigo, ProdutoRequest produtoRequest)
        {
            try
            {
                unitOfWorks.Begintransaction();

                var produto = produtoRepository.RetornarPorCodigo(codigo) ??
                    throw new RegraNegocioException("Produto não localizado");

                mapper.Map(produtoRequest, produto);

                produtoRepository.Alterar(produto);

                unitOfWorks.Commit();

                produtoRepository.Refresh(produto);

                return mapper.Map<ProdutoResponse>(produto);

            }
            catch
            {
                unitOfWorks.RollBack();
                throw;
            }

        }

        public void Excluir(int codigo)
        {
            try
            {
                unitOfWorks.Begintransaction();

                var produto = produtoRepository.RetornarPorCodigo(codigo) ??
                    throw new RegraNegocioException("Produto não localizado");

                var produtoTemDependencias = pedidoComboItemRepository.Recuperar()
                                                                       .Any(p => p.Produto.Codigo == codigo);

                if (produtoTemDependencias)
                    throw new RegraNegocioException("Não é possível excluir o produto, pois ele está sendo usado em pedidos.");

                produtoRepository.Excluir(produto);

                unitOfWorks.Commit();
            }
            catch
            {
                unitOfWorks.RollBack();

                throw;
            }

        }

        public List<ProdutoResponse> Consultar(ProdutoFiltroRequest produtoFiltroRequest)
        {
            try
            {
                var produtos = produtoRepository.Recuperar();

                if (!string.IsNullOrWhiteSpace(produtoFiltroRequest.Nome))
                {
                    produtos = produtos.Where(p => p.Nome.ToLower().Contains(produtoFiltroRequest.Nome.ToLower()));
                }

                if (produtoFiltroRequest.Categoria.HasValue)
                {
                    produtos = produtos.Where(p => p.Categoria == produtoFiltroRequest.Categoria.Value);
                }

                if (produtoFiltroRequest.Situacao.HasValue)
                {
                    produtos = produtos.Where(p => p.Situacao == produtoFiltroRequest.Situacao.Value);
                }

                var produtoList = produtos.ToList();

                return mapper.Map<List<ProdutoResponse>>(produtoList); ;
            }
            catch
            {
                throw;
            }
        }

        public ProdutoResponse Consultar(int codigo)
        {
            try
            {
                var produto = produtoRepository.RetornarPorCodigo(codigo) ??
                    throw new RegraNegocioException("Produto não localizado");

                return mapper.Map<ProdutoResponse>(produto);
            }
            catch
            {
                throw;
            }
        }
    }
}
