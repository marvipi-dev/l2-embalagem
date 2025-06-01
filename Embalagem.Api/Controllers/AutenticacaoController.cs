using Embalagem.Api.Data;
using Embalagem.Api.Services;
using Embalagem.Api.Views;
using Microsoft.AspNetCore.Mvc;

namespace Embalagem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly IAutenticacaoService _autenticacaoService;

        public AutenticacaoController(IRepository repository, IAutenticacaoService autenticacao)
        {
            _repository = repository;
            _autenticacaoService = autenticacao;
        }
        
        [HttpPost]
        public IActionResult GerarToken([FromBody] Usuario usuario)
        {
            if (_repository.Validar(usuario))
            {
                return Ok(_autenticacaoService.GerarToken());
            }

            return Unauthorized();
        }
    }
}
