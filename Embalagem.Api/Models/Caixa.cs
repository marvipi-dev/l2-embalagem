using Embalagem.Api.Helpers;

namespace Embalagem.Api.Models;

public class Caixa : Dimensoes
{
    public required string CaixaId { get; set; }

    /// <summary>
    /// Verifica se a caixa comporta determinadas dimensões.
    /// </summary>
    /// <param name="dimensoes">As <see cref="Dimensoes"/> para verificar.</param>
    /// <returns>true se as dimensões são menores do que as da caixa, senão false.</returns>
    public bool Comporta(Dimensoes dimensoes)
    {
        return dimensoes < this;
    }
}