using Application.DataTranfer.Clientes.Requests;
using Application.DataTranfer.Clientes.Responses;
using Application.Repository.Clientes;
using Application.Services.Clientes;
using Application.Services.Clientes.Interfaces;
using Application.Transactions.Interfaces;
using AutoMapper;
using Domain.Clientes.Entidades;
using Domain.Util;
using FizzWare.NBuilder;
using FluentAssertions;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Application.Tests.Services.Clientes
{
    public class ClienteServiceTestes
    {
        private readonly IMapper mapper;
        private readonly IClienteRepository clienteRepository;
        private readonly IUnitOfWorks unitOfWorks;
        private readonly IClienteService clienteService;

        public ClienteServiceTestes()
        {
            mapper = Substitute.For<IMapper>();
            clienteRepository = Substitute.For<IClienteRepository>();
            unitOfWorks = Substitute.For<IUnitOfWorks>();
            clienteService = new ClienteService(mapper, clienteRepository, unitOfWorks);

        }

        [Fact]
        public void Inserir_ClienteComCpfJaCadastrado_DeveLancarRegraNegocioException()
        {
            var clienteRequest = Builder<ClienteRequest>.CreateNew()
                .With(cr => cr.Cpf, "12345678909")
                .Build();

            var clienteExistente = Builder<Cliente>.CreateNew()
                .With(c => c.Cpf, "12345678909")
                .Build();

            clienteRepository.Consultar().Returns(new List<Cliente> { clienteExistente });

            var sut = clienteService;

            sut.Invoking(x => x.Inserir(clienteRequest)).Should().Throw<RegraNegocioException>()
                .WithMessage("Cpf já cadastrado");
        }

        [Fact]
        public void Inserir_ClienteValido_DeveRetornarClienteResponse()
        {
            var clienteRequest = Builder<ClienteRequest>.CreateNew()
                .With(cr => cr.Cpf, "12345678909")
                .Build();

            var cliente = Builder<Cliente>.CreateNew().Build();
            var clienteResponse = Builder<ClienteResponse>.CreateNew().Build();

            clienteRepository.Consultar().Returns(new List<Cliente>());
            mapper.Map<Cliente>(clienteRequest).Returns(cliente);
            mapper.Map<ClienteResponse>(cliente).Returns(clienteResponse);

            var sut = clienteService;

            var result = sut.Inserir(clienteRequest);

            result.Should().Be(clienteResponse);
            unitOfWorks.Received(1).Commit();
        }

        [Fact]
        public void Consultar_DeveRetornarListaDeClientes()
        {
            var clientes = Builder<Cliente>.CreateListOfSize(3)
                .All().With(c => c.Cpf, "12345678909")
                .Build();

            var clientesResponse = Builder<ClienteResponse>.CreateListOfSize(3)
                .Build();

            clienteRepository.Consultar().Returns(clientes);
            mapper.Map<List<ClienteResponse>>(clientes).Returns(clientesResponse);

            var sut = clienteService;

            var result = sut.Consultar();

            result.Should().BeEquivalentTo(clientesResponse);
        }

        [Fact]
        public void FiltrarClientePorCpf_CpfNaoCadastrado_DeveLancarRegraNegocioException()
        {
            var clienteFiltroRequest = Builder<ClienteFiltroRequest>.CreateNew()
                .With(cfr => cfr.Cpf, "12345678909")
                .Build();

            clienteRepository.Recuperar()
                .Returns(new List<Cliente>().AsQueryable());

            var sut = clienteService;

            sut.Invoking(x => x.FiltrarClientePorCpf(clienteFiltroRequest))
                .Should().Throw<RegraNegocioException>()
                .WithMessage("Cpf não cadastrado");
        }

        [Fact]
        public void FiltrarClientePorCpf_CpfCadastrado_DeveRetornarClienteResponse()
        {
            var clienteFiltroRequest = Builder<ClienteFiltroRequest>.CreateNew()
                .With(cfr => cfr.Cpf, "12345678909")
                .Build();

            var cliente = Builder<Cliente>.CreateNew().With(c => c.Cpf, "12345678909").Build();

            var clienteResponse = Builder<ClienteResponse>.CreateNew().Build();

            clienteRepository.Recuperar()
                .Returns(new List<Cliente> { cliente }
                .AsQueryable());

            mapper.Map<ClienteResponse>(cliente).Returns(clienteResponse);

            var sut = clienteService;

            var result = sut.FiltrarClientePorCpf(clienteFiltroRequest);

            result.Should().Be(clienteResponse);
        }
    }
}
