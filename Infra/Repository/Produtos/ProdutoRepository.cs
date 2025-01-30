using Application.Repository.Produtos;
using Domain.Produtos.Entidades;
using Infra.Repository.Bases;
using NHibernate;

namespace Infra.Repository.Produtos
{
    public class ProdutoRepository(ISession session) : BaseRepository<Produto>(session), IProdutoRepository
    {
    }
}