using System;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Helpers;
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
                new Claim(JwtRegisteredClaimNames.Iat, _jwtOptions.IssuedAt.ToUnixEpochDate().ToString(), ClaimValueTypes.Integer64),
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
            var roles = identity.FindAll(ClaimTypes.Role).Select(x => x.Value).ToList();
            var id = identity.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!String.IsNullOrEmpty(id))
            {
                var item = await _context.UserProfiles.SingleOrDefaultAsync(x => x.Id == id);
                if (item != null)
                {
                    var casted = _mapper.Map<MyProfileModel>(item);
                    if (casted.UserRoles.All(x => roles.Contains(x)) && roles.All(x => casted.UserRoles.Contains(x)))
                    {
                        return casted;
                    }
                    // If the identity roles don't match the account roles, force a relog.
                }
            }
            return null;
        }
    }
}