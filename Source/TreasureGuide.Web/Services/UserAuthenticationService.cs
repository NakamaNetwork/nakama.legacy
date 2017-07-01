using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TreasureGuide.Web.Configurations;
using TreasureGuide.Web.Models.ProfileModels;

namespace TreasureGuide.Web.Services
{
    public interface IAuthExportService
    {
        Task<AccessTokenModel> Get(ClaimsPrincipal user);
        ProfileModel GetUserInfo(ClaimsPrincipal user);
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
            if (identity == null)
            {
                return null;
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, identity.FindFirstValue(ClaimTypes.Name)),
                new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                identity.FindFirst(ClaimTypes.Name),
                identity.FindFirst(ClaimTypes.Email)
            };

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

        public ProfileModel GetUserInfo(ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email);
            var name = user.FindFirstValue(ClaimTypes.Name);
            var roles = user.FindAll(ClaimTypes.Role).Select(x => x.Value);
            var model = new ProfileModel
            {
                EmailAddress = email,
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