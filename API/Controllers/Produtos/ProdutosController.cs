using Application.DataTranfer.Produtos.Requests;
using Application.DataTranfer.Produtos.Responses;
using Application.Services.Produtos.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Produtos
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoService _produtoService;

        public ProdutosController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }


        [HttpPost]
        public ActionResult<ProdutoResponse> Inserir(ProdutoRequest produtoRequest)
        {

            return _produtoService.Inserir(produtoRequest);
        }

        [HttpPut]
        [Route("{codigo}")]
        public ActionResult<ProdutoResponse> Alterar(int codigo, ProdutoRequest produtoRequest)
        {

            return _produtoService.Alterar(codigo, produtoRequest);
        }


        [HttpGet]
        public ActionResult<List<ProdutoResponse>> Consultar([FromQuery] ProdutoFiltroRequest produtoFiltroRequest)
        {
            return _produtoService.Consultar(produtoFiltroRequest);
        }

        [HttpGet]
        [Route("{codigo}")]
        public ActionResult<ProdutoResponse> Consultar(int codigo)
        {
            return _produtoService.Consultar(codigo);
        }
    }

}
