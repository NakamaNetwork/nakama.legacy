using System.Collections.Generic;
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

        public static string GetId(this ClaimsPrincipal principal)
        {
            return principal?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public static IEnumerable<string> GetRoles(this ClaimsPrincipal principal)
        {
            return principal.FindAll(ClaimTypes.Role).Select(x => x.Value);
        }

        public static bool IsInRole(this ClaimsPrincipal principal, string role)
        {
            return principal.FindFirst(x => x.Type == ClaimTypes.Role && x.Value == role) != null;
        }

        public static bool IsInAnyRole(this ClaimsPrincipal principal, params string[] roles)
        {
            return principal.FindFirst(x => x.Type == ClaimTypes.Role && roles.Contains(x.Value)) != null;
        }

        public static bool IsInAllRoles(this ClaimsPrincipal principal, params string[] roles)
        {
            return principal.FindAll(x => x.Type == ClaimTypes.Role && roles.Contains(x.Value)).Count() >= roles.Length;
        }
    }
}
