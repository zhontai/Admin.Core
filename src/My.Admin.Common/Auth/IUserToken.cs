using System.Security.Claims;

namespace My.Admin.Common.Auth
{
    public interface IUserToken
    {
        string Create(Claim[] claims);

        Claim[] Decode(string jwtToken);
    }
}