using Embalagem.Api.Models;
using Embalagem.Api.Views;

namespace Embalagem.Api.Data;

public interface IRepository
{
    public Task<IEnumerable<CaixaModel>?> LerCaixasAsync();
    public Task<bool?> EscreverAsync(IEnumerable<Views.Embalagem> embalagens);
    public Task<IEnumerable<RegistroEmbalagem>?> LerEmbalagensAsync();
    public Task<bool?> ExisteAsync(Usuario usuario);
    public Task<bool?> EscreverAsync(Usuario usuario);
    public Task<bool?> ValidarAsync(Usuario usuario);
}