using System.Collections.Generic;
using System.Security.Claims;

namespace Admin.Core.Common.Auth
{
    public interface IUser
    {
        long Id { get; }
        string Name { get; }
        bool IsAuthenticated();
        IEnumerable<Claim> GetClaimsIdentity();
        List<string> GetClaimValueByType(string ClaimType);
    }
}
