using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.DTO.Login.Response;
using Shared.Interface;

namespace BusinessLayer.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> GenerateTokenAsync(UserDetailsResponse userDetailResponse)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims(userDetailResponse);
        var tokenOptions = GetTokenOptions(signingCredentials, claims);

        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return token;
    }

    private SecurityToken GetTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var jwtSettings = _configuration.GetSection("JWT");

        var tokenOptions = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddSeconds(Convert.ToDouble(jwtSettings["ExpireTime"])),
            signingCredentials: signingCredentials
        );

        return tokenOptions;
    }

    private async Task<List<Claim>> GetClaims(UserDetailsResponse loginUserResponse)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Email, loginUserResponse.Email)
        };

        return claims;
    }

    private SigningCredentials GetSigningCredentials()
    {
        var jwtSettings = _configuration.GetSection("JWT");
        var key = jwtSettings["Key"];
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        return new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
    }
}