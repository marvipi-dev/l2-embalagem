using Embalagem.Api.Models;
using Embalagem.Api.Views;

namespace Embalagem.Api.Data;

public interface IRepository
{
    public IEnumerable<CaixaModel> LerCaixas();
    public bool Escrever(IEnumerable<Views.Embalagem> embalagens);
    public IEnumerable<RegistroEmbalagem> LerEmbalagens();
    public bool Existe(Usuario usuario);
    public bool Escrever(Usuario usuario);
    public bool Validar(Usuario usuario);
}