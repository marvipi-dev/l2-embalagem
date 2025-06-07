using Embalagem.Api.Models;

namespace Embalagem.Api.Data;

public interface IUsuarioRepository
{
    public Task<bool?> ExisteAsync(Usuario usuario);
    public Task<bool?> EscreverAsync(Usuario usuario);
    public Task<bool?> ValidarAsync(Usuario usuario);
}