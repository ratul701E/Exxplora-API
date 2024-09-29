using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace Exxplora_API.Utilities
{
    public class ClaimsHelper
    {
        public static string GetEmail(IPrincipal user)
        {
            var identity = user.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var emailClaim = identity.FindFirst(ClaimTypes.Email) ??
                                 identity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
                return emailClaim?.Value;
            }
            return null;
        }

        public static string GetRole(IPrincipal user)
        {
            var identity = user.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var roleClaim = identity.FindFirst(ClaimTypes.Role) ??
                                identity.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
                return roleClaim?.Value;
            }
            return null;
        }

        public static string GetUserId(IPrincipal user)
        {
            var identity = user.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var idClaim = identity.FindFirst(ClaimTypes.NameIdentifier) ??
                              identity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                return idClaim?.Value;
            }
            return null;
        }
    }
}