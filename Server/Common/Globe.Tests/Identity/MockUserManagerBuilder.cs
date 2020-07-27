using Globe.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Globe.Tests.Identity
{
    public class MockUserManagerBuilder
    {
        Mock<MockUserManager> _userManager = new Mock<MockUserManager>();
        List<GlobeUser> _users = new List<GlobeUser>();
        List<Claim> _claims = new List<Claim>();

        public MockUserManagerBuilder()
        {
        }

        async public Task<MockUserManager> Mock()
        {
            PasswordHasher<GlobeUser> hasher = new PasswordHasher<GlobeUser>();

            _userManager.Object.UserValidators.Add(new UserValidator<GlobeUser>());
            _userManager.Object.PasswordValidators.Add(new PasswordValidator<GlobeUser>());

            _userManager
                .Setup(x => x.DeleteAsync(It.IsAny<GlobeUser>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManager
                .Setup(x => x.CreateAsync(It.IsAny<GlobeUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<GlobeUser, string>(async (user, password) =>
                {
                    user.PasswordHash = hasher.HashPassword(user, password);
                    if (_claims != null && _claims.Count > 0)
                        await _userManager.Object.AddClaimsAsync(user, _claims);
                    
                    _users.Add(user);
                });
            _userManager
                .Setup(x => x.UpdateAsync(It.IsAny<GlobeUser>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManager
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .Returns<string>((userName) => Task.FromResult(_users.Find(user => user.UserName == userName)));
            _userManager
                .Setup(x => x.GetClaimsAsync(It.IsAny<GlobeUser>()))
                .Returns<GlobeUser>(async (user) =>
                {
                    return await Task.FromResult(_claims);
                });
            _userManager
                .Setup(x => x.CheckPasswordAsync(It.IsAny<GlobeUser>(), It.IsAny<string>()))
                .Returns<GlobeUser, string>((user, password) =>
                {
                    var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);
                    return Task.FromResult(result == PasswordVerificationResult.Success);
                });

            if (_users == null || _users.Count == 0)
                await FillUsersAsync();

            return _userManager.Object;
        }

        async public Task<MockUserManagerBuilder> FillUsersAsync(List<GlobeUser> users = null)
        {
            if (users == null)
            {
                users = new List<GlobeUser>
                {
                    new GlobeUser
                    {
                        UserName = "user1",
                        Email = "user1@email.com"
                    },
                    new GlobeUser
                    {
                        UserName = "user2",
                        Email = "user2@email.com"
                    }
                };
            }

            foreach (var item in users)
            {
                await _userManager.Object.CreateAsync(item, $"{item.UserName}@password");
            }

            return await Task.FromResult(this);
        }

        public MockUserManagerBuilder ClaimsForAddintToCurrentUserAsync(List<Claim> claims = null)
        {
            _claims = claims;
            return this;
        }
    }
}
