using Microsoft.IdentityModel.Tokens;

namespace Globe.Identity.Security
{
    public interface ISigningCredentialsBuilder
    {
        SymmetricSecurityKey SigningKey { get; }
        ISigningCredentialsBuilder AddSecretKey(string secretKey);
        ISigningCredentialsBuilder AddAlgorithm(string algorithm);
        SigningCredentials Build();
    }
}
