using System.Collections.Generic;
using System.Security.Claims;

namespace Globe.Tests.Identity
{
    public class MockClaimsPrincipal
    {
        public ClaimsPrincipal Mock()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, GlobeIdentityContants.CLAIMS_NAME_IDENTITY),
            };
            var identity = new ClaimsIdentity(claims, GlobeIdentityContants.AUTHENTICATION_TYPE_BEARER);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            return claimsPrincipal;
        }
    }
}
