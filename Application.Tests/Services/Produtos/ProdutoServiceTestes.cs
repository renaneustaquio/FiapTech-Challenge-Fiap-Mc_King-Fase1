using Application.DataTranfer.Produtos.Requests;
using Application.DataTranfer.Produtos.Responses;
using Application.Repository.Pedidos;
using Application.Repository.Produtos;
using Application.Services.Produtos;
using Application.Transactions.Interfaces;
using AutoMapper;
using Domain.Pedidos.Entidades;
using Domain.Produtos.Entidades;
using Domain.Util;
using FizzWare.NBuilder;
using FluentAssertions;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Application.Tests.Services.Produtos
{
    public class ProdutoServiceTests
    {
        private readonly IMapper mapper;
        private readonly IProdutoRepository produtoRepository;
        private readonly IPedidoComboItemRepository pedidoComboItemRepository;
        private readonly IUnitOfWorks unitOfWorks;
        private readonly ProdutoService sut;

        public ProdutoServiceTests()
        {
            mapper = Substitute.For<IMapper>();
            produtoRepository = Substitute.For<IProdutoRepository>();
            pedidoComboItemRepository = Substitute.For<IPedidoComboItemRepository>();
            unitOfWorks = Substitute.For<IUnitOfWorks>();
            sut = new ProdutoService(mapper, produtoRepository, pedidoComboItemRepository, unitOfWorks);
        }

        [Fact]
        public void Inserir_DeveInserirProdutoERetornarProdutoResponse()
        {
            var produtoRequest = Builder<ProdutoRequest>.CreateNew()
                .With(x => x.Nome, "Produto Teste")
                .Build();

            var produto = Builder<Produto>.CreateNew()
                .With(x => x.Codigo, 1)
                .With(x => x.Nome, "Produto Teste")
                .Build();

            mapper.Map<Produto>(produtoRequest).Returns(produto);
            mapper.Map<ProdutoResponse>(produto).Returns(new ProdutoResponse { Codigo = 1, Nome = "Produto Teste" });

            var result = sut.Inserir(produtoRequest);

            produtoRepository.Received(1).Inserir(produto);
            unitOfWorks.Received(1).Commit();
            result.Should().NotBeNull();
            result.Codigo.Should().Be(produto.Codigo);
        }

        [Fact]
        public void Alterar_DeveAlterarProdutoERetornarProdutoResponse()
        {
            var produtoRequest = Builder<ProdutoRequest>.CreateNew()
                .With(x => x.Nome, "Produto Atualizado")
                .Build();

            var produtoExistente = Builder<Produto>.CreateNew()
                .With(x => x.Codigo, 1)
                .With(x => x.Nome, "Produto Antigo")
                .Build();

            produtoRepository.RetornarPorCodigo(1).Returns(produtoExistente);

            mapper.Map(produtoRequest, produtoExistente);

            var result = sut.Alterar(1, produtoRequest);

            produtoRepository.Received(1).Alterar(produtoExistente);

            unitOfWorks.Received(1).Commit();

            produtoRepository.Received(1).Refresh(produtoExistente);
        }

        [Fact]
        public void Excluir_DeveExcluirProdutoQuandoNaoTemDependencias()
        {
            var produtoExistente = Builder<Produto>.CreateNew()
                .With(x => x.Codigo, 1)
                .With(x => x.Nome, "Produto Teste")
                .Build();

            produtoRepository.RetornarPorCodigo(1).Returns(produtoExistente);
            pedidoComboItemRepository.Recuperar().Returns(new List<PedidoComboItem>().AsQueryable());

            sut.Excluir(1);

            produtoRepository.Received(1).Excluir(produtoExistente);
            unitOfWorks.Received(1).Commit();
        }

        [Fact]
        public void Excluir_DeveLancarExcecaoQuandoProdutoTemDependencias()
        {
            var produtoExistente = Builder<Produto>.CreateNew()
                .With(x => x.Codigo, 1)
                .With(x => x.Nome, "Produto Teste")
                .Build();

            produtoRepository.RetornarPorCodigo(1).Returns(produtoExistente);

            var pedidoComboItem = Builder<PedidoComboItem>.CreateNew()
                .With(x => x.Produto, new Produto(1))
                .Build();

            pedidoComboItemRepository.Recuperar().Returns(new List<PedidoComboItem> { pedidoComboItem }.AsQueryable());

            sut.Invoking(sut => sut.Excluir(1))
                .Should()
                .Throw<RegraNegocioException>();

            unitOfWorks.Received(1).RollBack();
        }


        [Fact]
        public void Consultar_DeveRetornarListaDeProdutosFiltrados()
        {
            var produtoFiltroRequest = Builder<ProdutoFiltroRequest>.CreateNew()
                .With(x => x.Nome, "Teste")
                .Build();

            var produtos = Builder<Produto>.CreateListOfSize(2)
                .All()
                .With(x => x.Nome, "Produto Teste")
                .Build();

            produtoRepository.Recuperar().Returns(produtos.AsQueryable());
            mapper.Map<List<ProdutoResponse>>(produtos).Returns(new List<ProdutoResponse>());

            var result = sut.Consultar(produtoFiltroRequest);

            produtoRepository.Received(1).Recuperar();
        }

        [Fact]
        public void ConsultarPorCodigo_DeveRetornarProdutoResponse()
        {
            var produtoExistente = Builder<Produto>.CreateNew()
                .With(x => x.Codigo, 1)
                .With(x => x.Nome, "Produto Teste")
                .Build();

            produtoRepository.RetornarPorCodigo(1).Returns(produtoExistente);
            mapper.Map<ProdutoResponse>(produtoExistente).Returns(new ProdutoResponse { Codigo = 1, Nome = "Produto Teste" });

            var result = sut.Consultar(1);

            result.Should().NotBeNull();
            result.Codigo.Should().Be(produtoExistente.Codigo);
        }

        [Fact]
        public void ConsultarPorCodigo_DeveLancarExcecaoQuandoProdutoNaoExistir()
        {
            produtoRepository.RetornarPorCodigo(1).Returns((Produto)null);

            sut.Invoking(sut => sut.Consultar(1))
                .Should()
                .Throw<RegraNegocioException>()
                .WithMessage("Produto não localizado");
        }
    }
}
