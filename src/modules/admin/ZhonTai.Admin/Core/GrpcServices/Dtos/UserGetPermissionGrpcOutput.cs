using ProtoBuf;
using ZhonTai.Admin.Services.User.Dto;

namespace ZhonTai.Admin.Core.GrpcServices.Dtos;

/// <summary>
/// 用户权限
/// </summary>
[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class UserGetPermissionGrpcOutput : UserGetPermissionOutput
{
}
