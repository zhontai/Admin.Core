using System.Security.Claims;

namespace Admin.Core.Common.Auth
{
    public interface IUserToken
    {
        string Create(Claim[] claims);

        Claim[] Decode(string jwtToken);
    }
}
