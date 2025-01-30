using Domain.Enums;

namespace Application.DataTranfer.Produtos.Requests
{
    public class ProdutoRequest
    {
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public CategoriaEnum Categoria { get; set; }
        public AtivoInativoEnum Situacao { get; set; }
    }
}