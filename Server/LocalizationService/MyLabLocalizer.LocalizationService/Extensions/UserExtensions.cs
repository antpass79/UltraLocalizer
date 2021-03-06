﻿using MyLabLocalizer.Shared.Utilities;
using System;
using System.Linq;
using System.Security.Claims;

namespace MyLabLocalizer.LocalizationService.Extensions
{
    public static class UserExtensions
    {
        public static bool IsInAdministratorGroup(this ClaimsPrincipal principal)
        {
            var roles = Roles.Group_Administrators.Split(",", StringSplitOptions.RemoveEmptyEntries);
            return roles.Any(role => principal.IsInRole(role));
        }
    }
}
