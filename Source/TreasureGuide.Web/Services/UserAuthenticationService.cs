using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TreasureGuide.Web.Configurations;
using TreasureGuide.Web.Helpers;
using TreasureGuide.Web.Models.ProfileModels;

namespace TreasureGuide.Web.Services
{
    public interface IAuthExportService
    {
        Task<AccessTokenModel> Get(ClaimsPrincipal user);
        ProfileAuthExport GetUserInfo(ClaimsPrincipal identity);
    }

    public class AuthExportService : IAuthExportService
    {
        private readonly JwtIssuerOptions _jwtOptions;

        public AuthExportService(IOptions<JwtIssuerOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<AccessTokenModel> Get(ClaimsPrincipal identity)
        {
            if (!identity.IsAuthenticated())
            {
                return new AccessTokenModel();
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, identity.FindFirstValue(ClaimTypes.NameIdentifier)),
                new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                identity.FindFirst(ClaimTypes.Name)
            }.Concat(identity.FindAll(ClaimTypes.Role)).Where(x => x != null).ToArray();

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                _jwtOptions.NotBefore,
                _jwtOptions.Expiration,
                _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            // Serialize and return the response
            var response = new AccessTokenModel
            {
                Token = encodedJwt,
                Expiration = _jwtOptions.Expiration
            };

            return response;
        }

        public ProfileAuthExport GetUserInfo(ClaimsPrincipal identity)
        {
            if (!identity.IsAuthenticated())
            {
                return null;
            }
            var name = identity.FindFirstValue(ClaimTypes.Name);
            var roles = identity.FindAll(ClaimTypes.Role).Select(x => x.Value);
            var model = new ProfileAuthExport
            {
                UserName = name,
                Roles = roles
            };
            return model;
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}