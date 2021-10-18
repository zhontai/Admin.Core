using System.Security.Claims;

namespace ZhonTai.Common.Auth
{
    public interface IUserToken
    {
        string Create(Claim[] claims);

        Claim[] Decode(string jwtToken);
    }
}