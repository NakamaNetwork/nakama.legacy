using System.Linq;
using System.Security.Claims;

namespace TreasureGuide.Web.Helpers
{
    public static class IdentityHelper
    {
        public static bool IsAuthenticated(this ClaimsPrincipal principal)
        {
            return principal?.Claims.Any() ?? false;
        }
    }
}
