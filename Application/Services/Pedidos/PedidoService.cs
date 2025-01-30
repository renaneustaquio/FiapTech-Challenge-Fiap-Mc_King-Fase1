using Application.DataTranfer.Pedidos.Requests;
using Application.DataTranfer.Pedidos.Responses;
using Application.Repository.Clientes;
using Application.Repository.Pedidos;
using Application.Repository.Produtos;
using Application.Services.Pedidos.Interfaces;
using Application.Transactions.Interfaces;
using AutoMapper;
using Domain.Enums;
using Domain.Pedidos.Entidades;
using Domain.Util;

namespace Application.Services.Pedidos
{
    public class PedidoService(IMapper mapper, IUnitOfWorks unitOfWorks,
                            IPedidoRepository pedidoRepository,
                            IPedidoComboRepository pedidoComboRepository,
                            IPedidoComboItemRepository pedidoComboItemRepository,
                            IPedidoStatusRepository pedidoStatusRepository,
                            IPedidoPagamentoRepository pedidoPagamentoRepository,
                            IClienteRepository clienteRepository,
                            IProdutoRepository produtoRepository) : IPedidoService
    {
        public List<PedidoResponse> Consultar()
        {
            try
            {
                var pedidos = pedidoRepository.Consultar();

                var pedidoResponse = mapper.Map<List<PedidoResponse>>(pedidos);

                return pedidoResponse;
            }
            catch
            {
                throw;
            }
        }

        public PedidoResponse Consultar(int codigo)
        {
            try
            {
                var pedido = pedidoRepository.RetornarPorCodigo(codigo);

                var pedidoResponse = mapper.Map<PedidoResponse>(pedido);

                return pedidoResponse;
            }
            catch
            {
                throw;
            }
        }

        public PedidoResponse Inserir(PedidoRequest pedidoRequest)
        {
            try
            {
                unitOfWorks.Begintransaction();

                var cliente = pedidoRequest.ClienteCodigo.HasValue ? clienteRepository.RetornarPorCodigo(pedidoRequest.ClienteCodigo.Value) : null;

                if (cliente == null && pedidoRequest.ClienteCodigo.HasValue)
                    throw new RegraNegocioException("Cliente não encontrado");


                if (pedidoRequest.PedidoCombo == null || !pedidoRequest.PedidoCombo.Any())
                    throw new RegraNegocioException("Pedido deve conter pelo menos um combo.");

                var pedido = new Pedido(cliente);

                pedidoRepository.Inserir(pedido);

                foreach (var pedidoComboRequest in pedidoRequest.PedidoCombo)
                {
                    var pedidoCombo = new PedidoCombo(pedido);

                    pedidoComboRepository.Inserir(pedidoCombo);

                    foreach (var pedidoComboItemRequest in pedidoComboRequest.PedidoComboItems)
                    {
                        var produto = produtoRepository.RetornarPorCodigo(pedidoComboItemRequest.ProdutoCodigo) ??
                            throw new RegraNegocioException("Produto não econtrado");

                        var pedidoComboItem = new PedidoComboItem(pedidoCombo, produto);

                        pedidoComboItemRepository.Inserir(pedidoComboItem);

                    }
                }
                var pedidoStatus = new PedidoStatus(pedido, StatusPedido.AguardandoPagamento, DateTime.Now);

                pedidoStatusRepository.Inserir(pedidoStatus);

                unitOfWorks.Commit();

                pedidoRepository.Refresh(pedido);

                return mapper.Map<PedidoResponse>(pedido);
            }
            catch
            {
                unitOfWorks.RollBack();
                throw;
            }
        }
        public PedidoResponse AlterarStatus(int codigo, StatusPedido statusPedido)
        {
            try
            {
                unitOfWorks.Begintransaction();

                var pedido = pedidoRepository.RetornarPorCodigo(codigo) ??
                    throw new RegraNegocioException("Pedido não localizado");

                var ultimoStatus = pedido.PedidoStatus.OrderByDescending(ps => ps.DataCriacao).FirstOrDefault();

                if (ultimoStatus == null)
                    throw new RegraNegocioException("Pedido não possui status");

                switch (statusPedido)
                {
                    case StatusPedido.Cancelado:
                        if (ultimoStatus.Status != StatusPedido.AguardandoPagamento)
                            throw new RegraNegocioException("Pedido já foi pago e não pode ser cancelado");
                        break;
                    case StatusPedido.Finalizado:
                        if (ultimoStatus.Status != StatusPedido.Pronto)
                            throw new RegraNegocioException("Pedido precisa estar como 'Pronto' para ser finalizado");
                        break;
                    case StatusPedido.Pronto:
                        if (ultimoStatus.Status != StatusPedido.EmPreparo)
                            throw new RegraNegocioException("Pedido precisa estar 'Em Preparo' para ser finalizado");
                        break;
                    default:
                        throw new RegraNegocioException("Pedido não pode ser alterado para esse status");
                }

                var pedidoStatus = new PedidoStatus(pedido, statusPedido, DateTime.Now);

                pedidoStatusRepository.Inserir(pedidoStatus);

                unitOfWorks.Commit();

                pedidoRepository.Refresh(pedido);

                return mapper.Map<PedidoResponse>(pedido);

            }
            catch
            {
                unitOfWorks.RollBack();
                throw;
            }
        }

        public PedidoResponse InserirCombo(int codigo, PedidoComboRequest pedidoComboRequest)
        {
            try
            {
                unitOfWorks.Begintransaction();

                var pedido = pedidoRepository.RetornarPorCodigo(codigo) ??
                    throw new RegraNegocioException("Pedido não localizado");

                if (pedido.PedidoStatus.Where(ps => ps.Status != StatusPedido.AguardandoPagamento).Any())
                    throw new RegraNegocioException("Pedido não pode ser alterado");

                if (pedidoComboRequest.PedidoComboItems == null || !pedidoComboRequest.PedidoComboItems.Any())
                    throw new RegraNegocioException("Pedido deve conter pelo menos um combo.");

                var pedidoCombo = new PedidoCombo(pedido);

                pedidoComboRepository.Inserir(pedidoCombo);

                foreach (var pedidoComboItemRequest in pedidoComboRequest.PedidoComboItems)
                {
                    var produto = produtoRepository.RetornarPorCodigo(pedidoComboItemRequest.ProdutoCodigo) ??
                        throw new RegraNegocioException("Produto não econtrado");

                    var pedidoComboItem = new PedidoComboItem(pedidoCombo, produto);

                    pedidoComboItemRepository.Inserir(pedidoComboItem);

                }
                unitOfWorks.Commit();

                pedidoRepository.Refresh(pedido);

                return mapper.Map<PedidoResponse>(pedido);
            }
            catch
            {
                unitOfWorks.RollBack();
                throw;
            }
        }

        public PedidoResponse RemoverCombo(int codigo, int codigoCombo)
        {
            try
            {
                unitOfWorks.Begintransaction();

                var pedido = pedidoRepository.RetornarPorCodigo(codigo) ??
                    throw new RegraNegocioException("Pedido não localizado");

                if (pedido.PedidoStatus.Where(ps => ps.Status != StatusPedido.AguardandoPagamento).Any())
                    throw new RegraNegocioException("Pedido não pode ser alterado");

                var pedidoCombo = pedidoComboRepository.RetornarPorCodigo(codigoCombo) ??
                    throw new RegraNegocioException("Combo do pedido não localizado");

                pedidoComboRepository.Excluir(pedidoCombo);

                unitOfWorks.Commit();

                pedidoRepository.Refresh(pedido);

                return mapper.Map<PedidoResponse>(pedido);
            }
            catch
            {
                unitOfWorks.RollBack();
                throw;
            }
        }

        public PedidoResponse ConfirmarPagamento(int codigo, PedidoPagamentoRequest pedidoPagamentoRequest)
        {
            try
            {
                unitOfWorks.Begintransaction();

                var pedido = pedidoRepository.RetornarPorCodigo(codigo) ??
                    throw new RegraNegocioException("Pedido não localizado");

                if (pedido.PedidoPagamento != null)
                    throw new RegraNegocioException("Esse pedido já possui pagamento");

                var pedidoPagamento = mapper.Map<PedidoPagamento>(pedidoPagamentoRequest);

                if (pedido.CalcularTotal() != pedidoPagamento.Valor)
                    throw new RegraNegocioException("Valor do pedido incorreto");

                pedidoPagamento.SetPedido(pedido);

                pedidoPagamentoRepository.Inserir(pedidoPagamento);

                var pedidoStatus = new PedidoStatus(pedido, StatusPedido.EmPreparo, DateTime.Now);

                pedidoStatusRepository.Inserir(pedidoStatus);

                unitOfWorks.Commit();

                pedidoRepository.Refresh(pedido);

                return mapper.Map<PedidoResponse>(pedido);
            }
            catch
            {
                unitOfWorks.RollBack();
                throw;
            }
        }

        public List<PedidoCozinhaResponse> ObterPedidosCozinha()
        {
            var todosPedidosStatus = pedidoStatusRepository.Recuperar()
                                                            .Where(ps => ps.Status == StatusPedido.EmPreparo || ps.Pedido != null)
                                                            .ToList();

            var pedidosEmPreparo = todosPedidosStatus.GroupBy(ps => ps.Pedido.Codigo)
                                                     .Select(g => g.OrderByDescending(ps => ps.DataCriacao).FirstOrDefault())
                                                     .Where(ps => ps != null && ps.Status == StatusPedido.EmPreparo)
                                                     .Select(ps => ps.Pedido.Codigo)
                                                     .ToList();

            if (pedidosEmPreparo.Count > 0)
            {
                var pedidos = pedidoRepository.Recuperar()
                                               .Where(pedido => pedidosEmPreparo.Contains(pedido.Codigo))
                                               .ToList();


                var pedidosDetalhados = pedidos.Select(pedido => new PedidoCozinhaResponse
                {
                    Pedido = pedido.Codigo,
                    PedidoCombo = pedido.PedidoCombo.Select(combo => new PedidoComboCozinhaResponse
                    {
                        Codigo = combo.Codigo,
                        DataCriacao = pedido.PedidoStatus.Where(ps => ps.Status == StatusPedido.EmPreparo)
                                                         .Select(ps => ps.DataCriacao)
                                                         .FirstOrDefault(),
                        TempoEmPreparo = DateTime.Now - (pedido.PedidoStatus.Where(ps => ps.Status == StatusPedido.EmPreparo)
                                                             .Select(ps => ps.DataCriacao)
                                                             .FirstOrDefault()),
                        PedidoComboItem = combo.PedidoComboItem.Select(item => new PedidoComboItemCozinhaResponse
                        {
                            ProdutoCodigo = item.Produto.Codigo,
                            Nome = item.Produto.Nome,
                        })
                        .ToList()
                    }).ToList()
                }).ToList();

                return pedidosDetalhados;
            }

            return new List<PedidoCozinhaResponse>();
        }

        public PedidoMonitorResponse ObterPedidosMonitor()
        {
            var pedidosEmPreparo = pedidoStatusRepository.Recuperar()
                                                          .Select(ps => new
                                                          {
                                                              PedidoCodigo = ps.Pedido.Codigo,
                                                              ps.DataCriacao,
                                                              ps.Status
                                                          })
                                                          .Where(ps => ps.DataCriacao == pedidoStatusRepository
                                                          .Recuperar()
                                                          .Where(subPs => subPs.Pedido.Codigo == ps.PedidoCodigo)
                                                          .Max(subPs => subPs.DataCriacao))
                                                          .ToList()
                                                          .Where(ps => ps != null && ps.Status == StatusPedido.EmPreparo)
                                                          .Select(ps => new PedidoStatusMonitorResponse
                                                          {
                                                              PedidoCodigo = ps.PedidoCodigo,
                                                              DataCriacao = ps.DataCriacao,
                                                              Status = ps.Status
                                                          })
                                                          .ToList();

            var pedidosFinalizados = pedidoStatusRepository.Recuperar()
                                                            .Where(ps => ps.Status == StatusPedido.Finalizado)
                                                            .OrderByDescending(ps => ps.DataCriacao)
                                                            .Select(ps => new PedidoStatusMonitorResponse
                                                            {
                                                                PedidoCodigo = ps.Pedido.Codigo,
                                                                DataCriacao = ps.DataCriacao,
                                                                Status = ps.Status
                                                            })
                                                            .Take(10)
                                                            .ToList();

            var pedidoMonitorResponse = new PedidoMonitorResponse()
            {
                PedidoEmPreparacao = pedidosEmPreparo,
                PedidoFinalizado = pedidosFinalizados
            };

            return pedidoMonitorResponse;
        }

    }
}
