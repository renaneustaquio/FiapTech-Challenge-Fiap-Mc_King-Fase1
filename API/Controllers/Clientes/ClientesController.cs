using Application.DataTranfer.Clientes.Requests;
using Application.DataTranfer.Clientes.Responses;
using Application.Services.Clientes.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Clientes
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }


        [HttpPost]
        public ActionResult<ClienteResponse> Inserir(ClienteRequest clienteRequest)
        {

            return _clienteService.Inserir(clienteRequest); ;
        }

        [HttpGet]
        public ActionResult<List<ClienteResponse>> Consultar()
        {
            return _clienteService.Consultar();
        }

        [HttpGet]
        [Route("filtrar")]
        public ActionResult<ClienteResponse> Consultar([FromQuery] ClienteFiltroRequest clienteFiltroRequest)
        {
            return _clienteService.FiltrarClientePorCpf(clienteFiltroRequest);
        }

    }

}
