using Globe.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Globe.Tests.Identity
{
    public class MockSignInManagerBuilder
    {
        UserManager<GlobeUser> _userManager;
        Mock<MockSignInManager> _signInManager;

        ClaimsPrincipal _claimsPrincipal;

        public MockSignInManagerBuilder(UserManager<GlobeUser> userManager)
        {
            _userManager = userManager;
            _signInManager = new Mock<MockSignInManager>(userManager);
        }

        async public Task<MockSignInManager> Mock()
        {
            _signInManager
                .Setup(x => x.PasswordSignInAsync(It.IsAny<GlobeUser>(), It.IsAny<string>(), false, false))
                .Returns<GlobeUser, string, bool, bool>(async (user, password, isPersistent, lockoutOnFailure) =>
                {
                    if (await _userManager.CheckPasswordAsync(user, password))
                    {
                        var claims = await _userManager.GetClaimsAsync(user);
                        var claimsIdentity = new ClaimsIdentity(claims, GlobeIdentityContants.AUTHENTICATION_TYPE_BEARER);
                        _claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        return SignInResult.Success;
                    }

                    _claimsPrincipal = null;
                    return SignInResult.Failed;
                });
            _signInManager
                .Setup(x => x.SignOutAsync())
                .Callback(() =>
                {
                    _claimsPrincipal = null;
                });
            _signInManager
                .Setup(x => x.IsSignedIn(It.IsAny<ClaimsPrincipal>()))
                .Returns<ClaimsPrincipal>((claimsPrincipal) =>
                {
                    return _claimsPrincipal != null && _claimsPrincipal.Identity != null && _claimsPrincipal.Identity.IsAuthenticated;
                });

            return await Task.FromResult(_signInManager.Object);
        }
    }
}
