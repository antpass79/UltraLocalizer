using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Globe.Identity.Security
{
    public class SigningCredentialsBuilder : ISigningCredentialsBuilder
    {
        string _secretKey;
        string _algorithm;

        public SymmetricSecurityKey SigningKey { get; private set; }

        public SigningCredentialsBuilder()
        {
        }

        // TODO check if passing the secret key in the code is an issue
        public ISigningCredentialsBuilder AddSecretKey(string secretKey)
        {
            this._secretKey = secretKey;
            return this;
        }

        public ISigningCredentialsBuilder AddAlgorithm(string algorithm)
        {
            this._algorithm = algorithm;
            return this;
        }

        public SigningCredentials Build()
        {
            this.SigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this._secretKey));
            return new SigningCredentials(this.SigningKey, this._algorithm);
        }
    }
}
