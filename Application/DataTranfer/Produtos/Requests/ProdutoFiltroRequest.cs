using Domain.Enums;

namespace Application.DataTranfer.Produtos.Requests
{
    public class ProdutoFiltroRequest
    {
        public string? Nome { get; set; }
        public AtivoInativoEnum? Situacao { get; set; }
        public CategoriaEnum? Categoria { get; set; }
    }
}
