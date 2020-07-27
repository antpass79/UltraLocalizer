using Globe.Identity.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;

namespace Globe.Identity.Options
{
    public class ConfigureJwtBearerOptions : IPostConfigureOptions<JwtBearerOptions>
    {
        private readonly IOptions<JwtAuthenticationOptions> _jwtAuthenticationOptions;
        private readonly ISigningCredentialsBuilder _signingCredentialsBuilder;

        public ConfigureJwtBearerOptions(IOptions<JwtAuthenticationOptions> jwtAuthentication, ISigningCredentialsBuilder signingCredentialsBuilder)
        {
            _jwtAuthenticationOptions = jwtAuthentication ?? throw new ArgumentNullException(nameof(JwtAuthenticationOptions));
            _signingCredentialsBuilder = signingCredentialsBuilder;

            _ = _signingCredentialsBuilder
                .AddSecretKey(_jwtAuthenticationOptions.Value.SecretKey)
                .AddAlgorithm(SecurityAlgorithms.HmacSha256)
                .Build();
        }

        public void PostConfigure(string name, JwtBearerOptions options)
        {
            var jwtAuthenticationOptions = _jwtAuthenticationOptions.Value;

            options.ClaimsIssuer = jwtAuthenticationOptions.Issuer;
            options.IncludeErrorDetails = true;
            options.RequireHttpsMetadata = true;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAuthenticationOptions.Issuer,

                ValidateAudience = true,
                ValidAudience = jwtAuthenticationOptions.Audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingCredentialsBuilder.SigningKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        }
    }
}
