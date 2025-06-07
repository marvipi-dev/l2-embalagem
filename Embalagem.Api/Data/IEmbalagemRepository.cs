using Embalagem.Api.Models;

namespace Embalagem.Api.Data;

public interface IEmbalagemRepository
{
    public Task<IEnumerable<Caixa>?> LerCaixasAsync();
    public Task<bool?> EscreverAsync(IEnumerable<EmbalagemRegistro> embalagens);
    public Task<IEnumerable<EmbalagemRegistro>?> LerEmbalagensAsync();
}