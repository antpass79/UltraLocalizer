using System;
using System.Linq;
using System.Security.Claims;

namespace Globe.TranslationServer.Extensions
{
    public static class UserExtensions
    {
        public static bool IsMasterTranslator(this ClaimsPrincipal principal)
        {
            return principal.Claims.Any(item => item.Type == ClaimTypes.Role && (item.Value == "MasterTranslator" || item.Value == "Admin"));
        }
    }
}
