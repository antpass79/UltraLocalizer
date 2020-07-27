using Globe.Identity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Globe.Tests.Identity
{
    public class MockSignInManager : SignInManager<GlobeUser>
    {
        public MockSignInManager(MockUserManager mockUserManager)
            : base(mockUserManager,
                  new HttpContextAccessor(),
                  new Mock<IUserClaimsPrincipalFactory<GlobeUser>>().Object,
                  new Mock<IOptions<IdentityOptions>>().Object,
                  new Mock<ILogger<SignInManager<GlobeUser>>>().Object,
                  new Mock<IAuthenticationSchemeProvider>().Object,
                  new Mock<IUserConfirmation<GlobeUser>>().Object                  
                  )
        {
        }
    }
}
