using Application.DataTranfer.Produtos.Requests;
using Application.DataTranfer.Produtos.Responses;

namespace Application.Services.Produtos.Intefaces
{
    public interface IProdutoService
    {
        ProdutoResponse Inserir(ProdutoRequest produtoRequest);
        ProdutoResponse Alterar(int codigo, ProdutoRequest produtoRequest);
        void Excluir(int codigo);
        List<ProdutoResponse> Consultar(ProdutoFiltroRequest produtoFiltroRequest);
        ProdutoResponse Consultar(int codigo);
    }
}