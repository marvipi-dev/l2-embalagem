using Embalagem.Api.Data;
using Embalagem.Api.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Embalagem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IRepository _repository;

        public UsuarioController(IRepository repository)
        {
            _repository = repository;
        }
        
        [HttpPost]
        public IActionResult Registrar([FromBody] Usuario usuario)
        {
            if (_repository.Existe(usuario))
            {
                return Conflict("Usuario já cadastrado");
            }

            var sucesso = _repository.Escrever(usuario);
            return sucesso ? Created() : StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }
}
