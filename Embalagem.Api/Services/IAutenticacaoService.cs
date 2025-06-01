using Embalagem.Api.Views;

namespace Embalagem.Api.Services;

public interface IAutenticacaoService
{
    public string GerarToken();
}