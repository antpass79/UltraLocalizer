using Globe.Identity.Models;
using Globe.Identity.Security;
using Globe.Identity.Services;
using Globe.Tests;
using Globe.Tests.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Xunit;

namespace Globe.Identity.Tests
{
    public class LoginServiceTests
    {
        [Theory]
        [InlineData(TestConstants.USER_NAME_user3, TestConstants.PASSWORD_user3ETemailDOTcom)]
        async public void LoginWithNotExistsCredentials(string userName, string password)
        {
            var userManager = await new MockUserManagerBuilder().Mock();
            var signInManager = await new MockSignInManagerBuilder(userManager).Mock();

            var jwtIssuerOptions = new MockJwtAuthenticationOptions().Mock();
            var jwtTokenEncoder = new JwtTokenEncoder<GlobeUser>(userManager, jwtIssuerOptions);

            var loginService = new LoginService(userManager, signInManager, jwtIssuerOptions, jwtTokenEncoder);
            var loginResult = await loginService.LoginAsync(new Credentials
            {
                UserName = userName,
                Password = password
            });

            Assert.False(loginResult.Successful);
            Assert.Equal(TestConstants.RETURN_VALUE_Invalid_credentials, loginResult.Error);
        }

        [Theory]
        [InlineData(TestConstants.USER_NAME_user2, TestConstants.PASSWORD_user2ETpasswordNotValid)]
        async public void LoginWithInvalidCredentials(string userName, string password)
        {
            var userManager = await new MockUserManagerBuilder().Mock();
            var signInManager = await new MockSignInManagerBuilder(userManager).Mock();

            var jwtIssuerOptions = new MockJwtAuthenticationOptions().Mock();
            var jwtTokenEncoder = new JwtTokenEncoder<GlobeUser>(userManager, jwtIssuerOptions);

            var loginService = new LoginService(userManager, signInManager, jwtIssuerOptions, jwtTokenEncoder);
            var loginResult = await loginService.LoginAsync(new Credentials
            {
                UserName = userName,
                Password = password
            });

            Assert.False(loginResult.Successful);
            Assert.Equal(TestConstants.RETURN_VALUE_Invalid_credentials, loginResult.Error);
        }

        [Theory]
        [InlineData(TestConstants.USER_NAME_user2, TestConstants.PASSWORD_user2ETpassword)]
        async public void LoginWithValidCredentials(string userName, string password)
        {
            var userManager = await new MockUserManagerBuilder().Mock();
            var signInManager = await new MockSignInManagerBuilder(userManager).Mock();

            var jwtIssuerOptions = new MockJwtAuthenticationOptions().Mock();
            var jwtTokenEncoder = new JwtTokenEncoder<GlobeUser>(userManager, jwtIssuerOptions);

            var loginService = new LoginService(userManager, signInManager, jwtIssuerOptions, jwtTokenEncoder);
            var loginResult = await loginService.LoginAsync(new Credentials
            {
                UserName = userName,
                Password = password
            });

            Assert.True(loginResult.Successful);
            Assert.NotNull(loginResult.Token);
            Assert.NotEmpty(loginResult.Token);
        }

        [Theory]
        [InlineData(TestConstants.USER_NAME_user2, TestConstants.PASSWORD_user2ETpassword)]
        async public void LogoutALoggedUser(string userName, string password)
        {
            var userManager = await new MockUserManagerBuilder().Mock();
            var signInManager = await new MockSignInManagerBuilder(userManager).Mock();

            var jwtIssuerOptions = new MockJwtAuthenticationOptions().Mock();
            var jwtTokenEncoder = new JwtTokenEncoder<GlobeUser>(userManager, jwtIssuerOptions);

            var loginService = new LoginService(userManager, signInManager, jwtIssuerOptions, jwtTokenEncoder);
            var loginResult = await loginService.LoginAsync(new Credentials
            {
                UserName = userName,
                Password = password
            });

            var user = await userManager.FindByNameAsync(TestConstants.USER_NAME_user2);
            var userClaims = await userManager.GetClaimsAsync(user);

            var claimsIdentity = new ClaimsIdentity(userClaims, GlobeIdentityContants.AUTHENTICATION_TYPE_BEARER);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var isLoggedAfterLogin = signInManager.IsSignedIn(claimsPrincipal);

            await loginService.LogoutAsync();

            var isLoggedAfterLogout = signInManager.IsSignedIn(ClaimsPrincipal.Current);

            Assert.True(isLoggedAfterLogin);
            Assert.False(isLoggedAfterLogout);
        }
    }
}
