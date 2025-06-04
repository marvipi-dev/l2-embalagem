using Embalagem.Api.Models;

namespace Embalagem.Api.Data;

public interface IRepository
{
    public Task<IEnumerable<Caixa>?> LerCaixasAsync();
    public Task<bool?> EscreverAsync(IEnumerable<EmbalagemRegistro> embalagens);
    public Task<IEnumerable<EmbalagemRegistro>?> LerEmbalagensAsync();
    public Task<bool?> ExisteAsync(Usuario usuario);
    public Task<bool?> EscreverAsync(Usuario usuario);
    public Task<bool?> ValidarAsync(Usuario usuario);
}