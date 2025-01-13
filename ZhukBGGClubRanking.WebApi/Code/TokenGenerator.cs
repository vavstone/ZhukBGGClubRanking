using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ZhukBGGClubRanking.WebApi.Code
{
    public static class TokenGenerator
    {
        public static string GenerateToken(string jwtSecretKey, int tokenExpiryMinutes, string userName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSecretKey);

            // Configure the token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userName) // Set the claim with the name of the token user
                }),
                Expires = DateTime.UtcNow.AddMinutes(tokenExpiryMinutes), // Set the token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // Set the signing credentials for the token
            };

            // Create the JWT token based on the token descriptor
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Write the JWT token as a string
            var generatedToken = tokenHandler.WriteToken(token);

            return generatedToken;
        }
    }
}
