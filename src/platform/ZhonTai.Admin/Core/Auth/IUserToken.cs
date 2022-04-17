using System.Security.Claims;

namespace ZhonTai.Admin.Core.Auth
{
    public interface IUserToken
    {
        string Create(Claim[] claims);

        Claim[] Decode(string jwtToken);
    }
}