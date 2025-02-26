using ProtoBuf;

namespace ZhonTai.Admin.Core.GrpcServices.Dtos;

/// <summary>
/// 用户权限
/// </summary>
[ProtoContract(ImplicitFields = ImplicitFields.None)]
public class UserGetPermissionGrpcOutput
{
    public static class Models
    {
        /// <summary>
        /// 接口
        /// </summary>
        [ProtoContract(ImplicitFields = ImplicitFields.None)]
        public class ApiModel
        {
            /// <summary>
            /// 请求方法
            /// </summary>
            [ProtoMember(1)]
            public string HttpMethods { get; set; }

            /// <summary>
            /// 请求地址
            /// </summary>
            [ProtoMember(2)]
            public string Path { get; set; }
        }
    }

    /// <summary>
    /// 接口列表
    /// </summary>
    [ProtoMember(1)]
    public List<Models.ApiModel> Apis { get; set; }

    /// <summary>
    /// 权限点编码列表
    /// </summary>
    [ProtoMember(2)]
    public List<string> Codes { get; set; }
}
