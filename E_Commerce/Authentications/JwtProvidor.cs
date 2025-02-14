
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_Commerce.Authentications
{
    public class JwtProvidor(IOptions<JwtOptions> jwtOptions) : IJwtProvidor
    {
        private readonly JwtOptions jwtOptions = jwtOptions.Value;

        public (string token, int expirein) GenerateToken(ApplicationUser user)
        {
            Claim[] claims = [
                new(JwtRegisteredClaimNames.Sub , user.Id),
                new(JwtRegisteredClaimNames.Email , user.Email!),
                new(JwtRegisteredClaimNames.Name , user.Name),
                new(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
                ];
            //keys
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));
            var sigingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwtOptions.ExpiryMinutes),
                signingCredentials: sigingCredentials
                );
            return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresin: jwtOptions.ExpiryMinutes * 60);
        }

        public string? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = symmetricSecurityKey , 
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                } , out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                return jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
            }
            catch
            {
                return null;
            }
        }
    }
}
