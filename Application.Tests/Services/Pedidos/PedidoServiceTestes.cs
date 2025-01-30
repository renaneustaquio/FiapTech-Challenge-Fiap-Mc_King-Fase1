using Application.DataTranfer.Pedidos.Requests;
using Application.DataTranfer.Pedidos.Responses;
using Application.Repository.Clientes;
using Application.Repository.Pedidos;
using Application.Repository.Produtos;
using Application.Services.Pedidos;
using Application.Services.Pedidos.Interfaces;
using Application.Transactions.Interfaces;
using AutoMapper;
using Domain.Clientes.Entidades;
using Domain.Enums;
using Domain.Pedidos.Entidades;
using Domain.Produtos.Entidades;
using Domain.Util;
using FizzWare.NBuilder;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Application.Tests.Services.Pedidos
{
    public class PedidoServiceTests
    {
        private readonly IPedidoService sut;
        private readonly IMapper mapper;
        private readonly IUnitOfWorks unitOfWorks;
        private readonly IPedidoRepository pedidoRepository;
        private readonly IClienteRepository clienteRepository;
        private readonly IProdutoRepository produtoRepository;
        private readonly IPedidoStatusRepository pedidoStatusRepository;
        private readonly IPedidoComboRepository pedidoComboRepository;
        private readonly IPedidoComboItemRepository pedidoComboItemRepository;
        private readonly IPedidoPagamentoRepository pedidoPagamentoRepository;

        public PedidoServiceTests()
        {
            mapper = Substitute.For<IMapper>();
            unitOfWorks = Substitute.For<IUnitOfWorks>();
            pedidoRepository = Substitute.For<IPedidoRepository>();
            clienteRepository = Substitute.For<IClienteRepository>();
            produtoRepository = Substitute.For<IProdutoRepository>();
            pedidoStatusRepository = Substitute.For<IPedidoStatusRepository>();
            pedidoComboRepository = Substitute.For<IPedidoComboRepository>();
            pedidoComboItemRepository = Substitute.For<IPedidoComboItemRepository>();
            pedidoPagamentoRepository = Substitute.For<IPedidoPagamentoRepository>();

            sut = new PedidoService(
                mapper,
                unitOfWorks,
                pedidoRepository,
                pedidoComboRepository,
                pedidoComboItemRepository,
                pedidoStatusRepository,
                pedidoPagamentoRepository,
                clienteRepository,
                produtoRepository);
        }

        [Fact]
        public void Consultar_ShouldReturnPedidoResponses()
        {
            var pedidos = Builder<Pedido>.CreateListOfSize(5).Build();
            var pedidoResponses = Builder<PedidoResponse>.CreateListOfSize(5).Build().ToList();

            pedidoRepository.Consultar().Returns(pedidos);
            mapper.Map<List<PedidoResponse>>(pedidos).Returns(pedidoResponses);

            var result = sut.Consultar();

            pedidoRepository.Received(1).Consultar();
            mapper.Received(1).Map<List<PedidoResponse>>(pedidos);
        }

        [Fact]
        public void Inserir_ShouldInsertNewPedido()
        {
            var pedidoComboRequest = new PedidoComboRequest
            {
                PedidoComboItems = new List<PedidoComboItemRequest>
            {
                new PedidoComboItemRequest { ProdutoCodigo = 1 }
            }
            };

            var pedidoRequest = Builder<PedidoRequest>.CreateNew()
                .With(pr => pr.ClienteCodigo = 1)
                .With(pr => pr.PedidoCombo = new List<PedidoComboRequest> { pedidoComboRequest })
                .Build();

            var cliente = new Cliente(pedidoRequest.ClienteCodigo.Value);
            var pedido = new Pedido(cliente);

            clienteRepository.RetornarPorCodigo(pedidoRequest.ClienteCodigo.Value).Returns(cliente);
            mapper.Map<Pedido>(pedidoRequest).Returns(pedido);
            produtoRepository.RetornarPorCodigo(1).Returns(new Produto(1));

            var result = sut.Inserir(pedidoRequest);

            unitOfWorks.Received(1).Begintransaction();

            pedidoRepository.Received(1).Inserir(Arg.Is<Pedido>(p => p.Cliente.Codigo == cliente.Codigo));
            unitOfWorks.Received(1).Commit();
        }

        [Fact]
        public void Inserir_ShouldThrowRegraNegocioException_WhenClienteNotFound()
        {
            var pedidoRequest = Builder<PedidoRequest>.CreateNew()
                .With(pr => pr.ClienteCodigo = 1)
                .With(pr => pr.PedidoCombo = new List<PedidoComboRequest> { null })
                .Build();

            clienteRepository.RetornarPorCodigo(1).Returns((Cliente)null);

            sut.Invoking(sut => sut.Inserir(pedidoRequest)).Should().Throw<RegraNegocioException>();
        }

        [Fact]
        public void Inserir_ShouldThrowRegraNegocioException_WhenPedidoComboIsEmpty()
        {
            var pedidoRequest = Builder<PedidoRequest>.CreateNew()
                .With(pr => pr.ClienteCodigo = 1)
                .With(pr => pr.PedidoCombo = new List<PedidoComboRequest>())
                .Build();

            var cliente = new Cliente(1);
            clienteRepository.RetornarPorCodigo(1).Returns(cliente);

            sut.Invoking(sut => sut.Inserir(pedidoRequest)).Should().Throw<RegraNegocioException>();
        }

        [Fact]
        public void Inserir_ShouldThrowRegraNegocioException_WhenProdutoNotFound()
        {
            var pedidoComboRequest = new PedidoComboRequest
            {
                PedidoComboItems = new List<PedidoComboItemRequest>
            {
                new PedidoComboItemRequest { ProdutoCodigo = 1 }
            }
            };

            var pedidoRequest = Builder<PedidoRequest>.CreateNew()
                .With(pr => pr.ClienteCodigo = 1)
                .With(pr => pr.PedidoCombo = new List<PedidoComboRequest> { pedidoComboRequest })
                .Build();

            var cliente = new Cliente(1);
            clienteRepository.RetornarPorCodigo(1).Returns(cliente);
            produtoRepository.RetornarPorCodigo(1).Returns((Produto)null);

            sut.Invoking(sut => sut.Inserir(pedidoRequest)).Should().Throw<RegraNegocioException>();

        }
        [Fact]
        public void AlterarStatus_DeveAlterarParaCancelado_QuandoStatusForAguardandoPagamento()
        {
            var pedido = Builder<Pedido>.CreateNew()
                .With(p => p.Cliente, new Cliente(1))
                .With(p => p.PedidoStatus, new List<PedidoStatus>
                {
                     new PedidoStatus(null, StatusPedido.AguardandoPagamento, DateTime.Now)
                })
                .Build();

            pedidoRepository.RetornarPorCodigo(1).Returns(pedido);

            sut.Invoking(x => x.AlterarStatus(1, StatusPedido.Cancelado)).Should().NotThrow();

            pedidoStatusRepository.Received(1).Inserir(Arg.Is<PedidoStatus>(ps => ps.Status == StatusPedido.Cancelado));
            unitOfWorks.Received(1).Commit();
        }

        [Fact]
        public void AlterarStatus_DeveLancarExcecao_QuandoPedidoNaoEncontrado()
        {
            pedidoRepository.RetornarPorCodigo(1).Returns((Pedido)null);

            sut.Invoking(x => x.AlterarStatus(1, StatusPedido.Cancelado)).Should().Throw<RegraNegocioException>();
        }

        [Fact]
        public void AlterarStatus_DeveLancarExcecao_QuandoAlterarParaCanceladoComStatusInvalido()
        {
            var pedido = Builder<Pedido>.CreateNew()
                .With(p => p.Cliente, new Cliente(1))
                .With(p => p.PedidoStatus, new List<PedidoStatus>
                                            {
                                                new PedidoStatus(null, StatusPedido.EmPreparo, DateTime.Now)
                                            })
                .Build();

            pedidoRepository.RetornarPorCodigo(1).Returns(pedido);

            sut.Invoking(x => x.AlterarStatus(1, StatusPedido.Cancelado)).Should().Throw<RegraNegocioException>();
        }

        [Fact]
        public void AlterarStatus_DeveAlterarParaFinalizado_QuandoStatusEhPronto()
        {
            var pedido = Builder<Pedido>.CreateNew()
                .With(p => p.Cliente, new Cliente(1))
                .With(p => p.PedidoStatus, new List<PedidoStatus>
                                            {
                                                new PedidoStatus(null, StatusPedido.Pronto, DateTime.Now)
                                            })
                .Build();

            pedidoRepository.RetornarPorCodigo(1).Returns(pedido);

            sut.Invoking(x => x.AlterarStatus(1, StatusPedido.Finalizado)).Should().NotThrow();

            pedidoStatusRepository.Received(1).Inserir(Arg.Is<PedidoStatus>(ps => ps.Status == StatusPedido.Finalizado));
            unitOfWorks.Received(1).Commit();
        }

        [Fact]
        public void AlterarStatus_DeveLancarExcecao_QuandoAlterarParaFinalizadoComStatusInvalido()
        {
            var pedido = Builder<Pedido>.CreateNew()
                .With(p => p.Cliente, new Cliente(1))
                .With(p => p.PedidoStatus, new List<PedidoStatus>
                                             {
                                                  new PedidoStatus(null, StatusPedido.AguardandoPagamento, DateTime.Now)
                                             })
                .Build();

            pedidoRepository.RetornarPorCodigo(1).Returns(pedido);

            sut.Invoking(x => x.AlterarStatus(1, StatusPedido.Finalizado))
                .Should().Throw<RegraNegocioException>();
        }

        [Fact]
        public void AlterarStatus_DeveAlterarParaPronto_QuandoStatusForEmPreparo()
        {
            var pedido = Builder<Pedido>.CreateNew()
                .With(p => p.Cliente, new Cliente(1))
                .With(p => p.PedidoStatus, new List<PedidoStatus>
                                            {
                                                new PedidoStatus(null, StatusPedido.EmPreparo, DateTime.Now)
                                            })
                .Build();

            pedidoRepository.RetornarPorCodigo(1).Returns(pedido);

            sut.Invoking(x => x.AlterarStatus(1, StatusPedido.Pronto)).Should().NotThrow();

            pedidoStatusRepository.Received(1).Inserir(Arg.Is<PedidoStatus>(ps => ps.Status == StatusPedido.Pronto));
            unitOfWorks.Received(1).Commit();
        }

        [Fact]
        public void AlterarStatus_DeveLancarExcecao_QuandoAlterarParaProntoComStatusInvalido()
        {
            var pedido = Builder<Pedido>.CreateNew()
                .With(p => p.Cliente, new Cliente(1))
                .With(p => p.PedidoStatus, new List<PedidoStatus>
                                            {
                                                new PedidoStatus(null, StatusPedido.Cancelado, DateTime.Now)
                                            })
                .Build();

            pedidoRepository.RetornarPorCodigo(1).Returns(pedido);

            sut.Invoking(x => x.AlterarStatus(1, StatusPedido.Pronto))
                .Should().Throw<RegraNegocioException>();
        }
    }
}
