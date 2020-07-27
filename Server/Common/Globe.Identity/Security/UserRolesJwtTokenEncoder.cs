using Globe.Identity.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Globe.Identity.Security
{
    public class UserRolesJwtTokenEncoder<TUser> : JwtTokenEncoder<TUser>
        where TUser : IdentityUser
    {
        public UserRolesJwtTokenEncoder(UserManager<TUser> userManager, IOptions<JwtAuthenticationOptions> options)
            : base(userManager, options)
        {
        }

        async protected override Task<IEnumerable<Claim>> BuildClaimsAsync(TUser input)
        {
            var roles = await UserManager.GetRolesAsync(input);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, input.UserName),
            }
            .Concat(roles
                .Select(role =>
                {
                    return new Claim(ClaimTypes.Role, role);
                }));

            return await Task.FromResult(claims);
        }
    }
}
