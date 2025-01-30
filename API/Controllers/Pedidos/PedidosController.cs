using Application.DataTranfer.Pedidos.Requests;
using Application.DataTranfer.Pedidos.Responses;
using Application.Services.Pedidos.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Pedidos
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {

        private readonly IPedidoService _pedidoService;

        public PedidosController(IPedidoService opedidoService)
        {
            _pedidoService = opedidoService;
        }

        [HttpGet]
        public ActionResult<IList<PedidoResponse>> Consultar()
        {
            return _pedidoService.Consultar();
        }

        [HttpGet]
        [Route("Preparar")]
        public ActionResult<IList<PedidoCozinhaResponse>> ConsultarPedidoCozinha()
        {
            return _pedidoService.ObterPedidosCozinha();
        }

        [HttpGet]
        [Route("Monitor")]
        public ActionResult<PedidoMonitorResponse> ConsultarPedidoMonitor()
        {
            return _pedidoService.ObterPedidosMonitor();
        }

        [HttpGet]
        [Route("{codigo}")]
        public ActionResult<PedidoResponse> Consultar(int codigo)
        {
            return _pedidoService.Consultar(codigo);
        }

        [HttpPost]
        public ActionResult<PedidoResponse> Inserir(PedidoRequest pedidoRequest)
        {

            return _pedidoService.Inserir(pedidoRequest);
        }

        [HttpPut]
        [Route("Cancelar/{codigo}")]
        public ActionResult<PedidoResponse> Cancelar(int codigo)
        {

            return _pedidoService.AlterarStatus(codigo, StatusPedido.Cancelado);
        }


        [HttpPut]
        [Route("FinalizarPreparo/{codigo}")]
        public ActionResult<PedidoResponse> FinalizarPreparo(int codigo)
        {

            return _pedidoService.AlterarStatus(codigo, StatusPedido.Pronto);
        }

        [HttpPut]
        [Route("Finalizar/{codigo}")]
        public ActionResult<PedidoResponse> Finalizar(int codigo)
        {

            return _pedidoService.AlterarStatus(codigo, StatusPedido.Finalizado);
        }

        [HttpPut]
        [Route("InserirCombo/{codigo}")]
        public ActionResult<PedidoResponse> InserirCombo(int codigo, PedidoComboRequest pedidoComboRequest)
        {
            return _pedidoService.InserirCombo(codigo, pedidoComboRequest);
        }

        [HttpPut]
        [Route("RemoverCombo/{codigo}/{codigoCombo}")]
        public ActionResult<PedidoResponse> RemoverCombo(int codigo, int codigoCombo)
        {

            return _pedidoService.RemoverCombo(codigo, codigoCombo);
        }

        [HttpPut]
        [Route("ConfirmarPagamento/{codigo}")]
        public ActionResult<PedidoResponse> ConfirmarPagamento(int codigo, PedidoPagamentoRequest pedidoPagamentoRequest)
        {

            return _pedidoService.ConfirmarPagamento(codigo, pedidoPagamentoRequest);
        }

    }
}
