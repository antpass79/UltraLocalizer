using Globe.Identity.Models;
using Globe.Identity.Options;
using Globe.Identity.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Globe.Identity.Services
{
    public class LoginService<TUser> : IAsyncLoginService
        where TUser : GlobeUser, new()
    {
        private readonly UserManager<TUser> _userManager;
        private readonly SignInManager<TUser> _signInManager;
        IJwtTokenEncoder<TUser> _jwtTokenEncoder;

        public LoginService(UserManager<TUser> userManager, SignInManager<TUser> signInManager, IJwtTokenEncoder<TUser> jwtTokenEncoder)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenEncoder = jwtTokenEncoder;
        }

        async public Task<LoginResult> LoginAsync(Credentials credentials)
        {
            if (string.IsNullOrEmpty(credentials.UserName) || string.IsNullOrEmpty(credentials.Password))
                return await Task.FromResult<LoginResult>(BuildInvalidLoginResult());

            var userToVerify = await _userManager.FindByNameAsync(credentials.UserName);
            if (userToVerify == null)
                return await Task.FromResult<LoginResult>(BuildInvalidLoginResult());

            var result = await _signInManager.PasswordSignInAsync(userToVerify, credentials.Password, false, false);
            if (!result.Succeeded)
                return await Task.FromResult<LoginResult>(BuildInvalidLoginResult());

            return await Task.FromResult<LoginResult>(new LoginResult
            {
                Successful = true,
                Token = await _jwtTokenEncoder.EncodeAsync(userToVerify)
            });
        }

        async public Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        private LoginResult BuildInvalidLoginResult()
        {
            return new LoginResult
            {
                Successful = false,
                Error = "Invalid credentials"
            };
        }
    }

    public class LoginService : LoginService<GlobeUser>
    {
        public LoginService(UserManager<GlobeUser> userManager, SignInManager<GlobeUser> signInManager, IOptions<JwtAuthenticationOptions> jwtOptions, IJwtTokenEncoder<GlobeUser> jwtTokenEncoder)
            : base(userManager, signInManager, jwtTokenEncoder)
        {
        }
    }
}
