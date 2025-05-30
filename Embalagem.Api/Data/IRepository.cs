using Embalagem.Api.Models;
using Embalagem.Api.Views;

namespace Embalagem.Api.Data;

public interface IRepository
{
    public IEnumerable<CaixaModel> LerCaixas();
    public bool Escrever(IEnumerable<Views.Embalagem> embalagens);
    public IEnumerable<EmbalagemGetResponse> LerEmbalagens();
}