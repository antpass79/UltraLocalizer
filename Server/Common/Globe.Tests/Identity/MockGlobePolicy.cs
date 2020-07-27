using Globe.Identity.Options;
using System.Collections.Generic;

namespace Globe.Tests.Identity
{
    static public class Constants
    {
        public static class Strings
        {
            public static class Policy
            {
                public const string Name = "AdministrativeRights";
            }

            public static class JwtClaimIdentifiers
            {
                public const string Rol = "administrator", Id = "id";
            }

            public static class JwtClaims
            {
                public const string ApiAccess = "full_access";
            }
        }
    }

    public class MockGlobePolicy
    {
        public static GlobePolicy[] Mock()
        {
            var policies = new List<GlobePolicy>();
            policies.Add(new GlobePolicy(
                Constants.Strings.Policy.Name,
                (new List<GlobeClaim>
                {
                    new GlobeClaim(
                        Constants.Strings.JwtClaimIdentifiers.Rol,
                        new [] { Constants.Strings.JwtClaims.ApiAccess })
                }).ToArray(),
                (new List<GlobeRole>
                {
                    new GlobeRole(
                        "Admin",
                        "Full Access")
                }).ToArray()));

            return policies.ToArray();
        }
    }
}
