using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Ahoy_Data.Models;

namespace Ahoy_CoreApi.Helper
{
    public static class JwtHelpers
    {
        public static IEnumerable<Claim> GetClaims(this UserToken userAccounts, Guid Id)
        {
            IEnumerable<Claim> claims = new Claim[] {
                new Claim("Id", userAccounts.Id.ToString()),
                    new Claim(ClaimTypes.Name, userAccounts.UserName),
                    new Claim(ClaimTypes.Email, userAccounts.EmailId),
                    new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddDays(1).ToString("dd MM yyyy HH:mm:ss tt"))
            };
            return claims;
        }


        public static IEnumerable<Claim> GetClaims(this UserToken userAccounts, out Guid Id)
        {
            Id = Guid.NewGuid();
            return GetClaims(userAccounts, Id);
        }
        public static UserToken GenTokenkey(UserToken model, JwtSettings jwtSettings)
        {
            try
            {
                var UserToken = new UserToken();
                if (model == null) throw new ArgumentException(nameof(model));
                // Get secret key
                var key = System.Text.Encoding.ASCII.GetBytes(jwtSettings.IssuerSigningKey);
                Guid Id = Guid.Empty;
                DateTime expireTime = DateTime.Now.AddDays(1);
                UserToken.Validaty = expireTime.TimeOfDay;
                var JWToken = new JwtSecurityToken(issuer: jwtSettings.ValidIssuer, 
                                                   audience: jwtSettings.ValidAudience, 
                                                   claims: GetClaims(model, out Id), 
                                                   notBefore: new DateTimeOffset(DateTime.Now).DateTime, 
                                                   expires: new DateTimeOffset(expireTime).DateTime, 
                                                   signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256));
                UserToken.Token = new JwtSecurityTokenHandler().WriteToken(JWToken);
                UserToken.UserName = model.UserName;
                UserToken.Id = model.Id;
                UserToken.GuidId = Id;
                UserToken.ExpiredTime = expireTime;
                return UserToken;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
