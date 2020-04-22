using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Globe.Client.Platform.Extensions
{
    internal static class IdentityExtensions
    {
        public static IEnumerable<string> GetRoles(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            if (claimsIdentity == null)
                return new List<string>();

            return claimsIdentity.Claims
                .Where(claim => claim.Type == ClaimTypes.Role)
                .Select(claim => claim.Value)
                .ToList();
        }
    }
}
