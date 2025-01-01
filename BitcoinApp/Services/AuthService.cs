using BitcoinApp.Auxiliar;
using BitcoinApp.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BitcoinApp.Services
{
    public class AuthService
    {
        public string GenerateToken(UserAuth userAuth)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AuthSettings.PrivateKey);//class AuthSettings criada
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature);    //algoritmo HMAC-SHA256 

            //Inf para criar o JWT: Subject, Expires, SigningCredentials
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(userAuth),
                NotBefore = DateTime.UtcNow.AddDays(-1),
                Expires = DateTime.UtcNow.AddMonths(3),
                SigningCredentials = credentials,
            };

            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        /// <summary>
        /// Define Claims (Profiles)
        /// Each role is added as a separate declaration of type ClaimTypes.Role
        /// </summary>
        /// <param name="userAuth"></param>
        /// <returns></returns>
        private static ClaimsIdentity GenerateClaims(UserAuth userAuth)
        {
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.Name, userAuth.Email));

            foreach (var role in userAuth.Roles)
                claims.AddClaim(new Claim(ClaimTypes.Role, role));

            return claims;
        }

    }
}
