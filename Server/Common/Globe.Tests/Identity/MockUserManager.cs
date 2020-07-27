using Globe.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;

namespace Globe.Tests.Identity
{
    public class MockUserManager : UserManager<GlobeUser>
    {
        public MockUserManager()
            : base(new Mock<IUserStore<GlobeUser>>().Object,
                  new Mock<IOptions<IdentityOptions>>().Object,
                  new Mock<IPasswordHasher<GlobeUser>>().Object,
                  new IUserValidator<GlobeUser>[0],
                  new IPasswordValidator<GlobeUser>[0],
                  new Mock<ILookupNormalizer>().Object,
                  new Mock<IdentityErrorDescriber>().Object,
                  new Mock<IServiceProvider>().Object,
                  new Mock<ILogger<UserManager<GlobeUser>>>().Object)
        { }
    }
}
