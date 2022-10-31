using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Common.Extensions;

namespace ZhonTai.Admin.Core.Auth;

[SingleInstance]
public class UserToken : IUserToken
{
    private readonly JwtConfig _jwtConfig;

    public UserToken(JwtConfig jwtConfig)
    {
        _jwtConfig = jwtConfig;
    }

    public string Create(Claim[] claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecurityKey));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var timestamp = DateTime.Now.AddMinutes(_jwtConfig.Expires + _jwtConfig.RefreshExpires).ToTimestamp().ToString();
        claims = claims.Append(new Claim(ClaimAttributes.RefreshExpires, timestamp)).ToArray();

        var token = new JwtSecurityToken(
            issuer: _jwtConfig.Issuer,
            audience: _jwtConfig.Audience,
            claims: claims,
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddMinutes(_jwtConfig.Expires),
            signingCredentials: signingCredentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public JwtSecurityToken Decode(string jwtToken)
    {
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(jwtToken);
        return jwtSecurityToken;
    }
}