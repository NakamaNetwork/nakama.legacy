using System;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using TreasureGuide.Entities;
using TreasureGuide.Web.Configurations;
using TreasureGuide.Web.Helpers;
using TreasureGuide.Web.Models.ProfileModels;

namespace TreasureGuide.Web.Services
{
    public interface IAuthExportService
    {
        Task<AccessTokenModel> Get(ClaimsPrincipal user);
        Task<ProfileDetailModel> GetUserInfo(ClaimsPrincipal identity);
    }

    public class AuthExportService : IAuthExportService
    {
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly TreasureEntities _context;
        private readonly IMapper _mapper;

        public AuthExportService(IOptions<JwtIssuerOptions> jwtOptions, TreasureEntities context, IMapper mapper)
        {
            _jwtOptions = jwtOptions.Value;
            _context = context;
            _mapper = mapper;
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

        public async Task<ProfileDetailModel> GetUserInfo(ClaimsPrincipal identity)
        {
            if (!identity.IsAuthenticated())
            {
                return null;
            }
            var id = identity.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!String.IsNullOrEmpty(id))
            {
                var item = await _context.UserProfiles.SingleOrDefaultAsync(x => x.Id == id);
                if (item != null)
                {
                    var casted = _mapper.Map<MyProfileModel>(item);
                    return casted;
                }
            }
            return null;
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}