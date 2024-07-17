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
    private readonly IUserRepository _userRepository;

    public TokenService(IConfiguration configuration, IUserRepository userRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
    }

    public async Task<string> GenerateTokenAsync(UserDetailsResponse userDetailResponse)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims(userDetailResponse);
        var tokenOptions = GetTokenOptions(signingCredentials, claims);

        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return token;
    }

    public async Task<string?> RefreshTokenAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return null;

        var jwtSettings = _configuration.GetSection("JWT");

        if (!double.TryParse(jwtSettings["SlidingExpireTime"], out double slidingExpireMinute))
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        if (jwtToken == null)
            return null;

        var tokenExpireTime = jwtToken
            .ValidTo
            .ToLocalTime();
        var refreshTokenExpireTime = tokenExpireTime
            .AddMinutes(slidingExpireMinute);

        if (DateTime.Now < tokenExpireTime
            || DateTime.Now > refreshTokenExpireTime)
        {
            return null;
        }

        var principal = GetPrincipal(token);
        var userEmail = principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(userEmail))
            return null;

        var user = await _userRepository.GetUserByEmail(userEmail);
        if (user == null)
            return null;

        var newToken = await GenerateTokenAsync(user);
        return newToken;
    }

    public ClaimsPrincipal? GetPrincipal(string token)
    {
        var validationParameters = GetValidationParameters();
        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;

        var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(
            SecurityAlgorithms.HmacSha256,
            StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid Token");
        }

        return principal;
    }

    private TokenValidationParameters GetValidationParameters()
    {
        var jwtSettings = _configuration.GetSection("JWT");
        var key = jwtSettings["Key"];

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        };

        return tokenValidationParameters;
    }

    private SecurityToken GetTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var jwtSettings = _configuration.GetSection("JWT");

        var tokenOptions = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpireTime"])),
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