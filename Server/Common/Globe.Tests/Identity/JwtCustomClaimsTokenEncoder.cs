using Globe.Identity.Options;
using Globe.Identity.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Globe.Tests.Identity
{
    public class JwtCustomClaimsTokenEncoder<TUser> : JwtTokenEncoder<TUser>
        where TUser : IdentityUser
    {
        IEnumerable<Claim> _claims;

        public JwtCustomClaimsTokenEncoder(
            UserManager<TUser> userManager,
            IOptions<JwtAuthenticationOptions> options,
            IEnumerable<Claim> claims)
            : base(userManager, options)
        {
            _claims = claims;
        }

        async protected override Task<IEnumerable<Claim>> BuildClaimsAsync(TUser input)
        {
            return await Task.FromResult<IEnumerable<Claim>>(_claims);
        }
    }
}
