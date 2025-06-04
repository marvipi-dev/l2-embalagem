using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Embalagem.Api.Services;

public class AutenticacaoService : IAutenticacaoService
{
    private readonly IConfiguration _config;

    public AutenticacaoService(IConfiguration config)
    {
        _config = config;
    }

    public string GerarToken()
    {
        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(signingCredentials: credenciais,
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            expires: DateTime.UtcNow.AddHours(Convert.ToInt32(_config["Jwt:Expiry"])));

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenString = tokenHandler.WriteToken(token);

        return tokenString;
    }
}