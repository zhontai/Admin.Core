using ProtoBuf;
using System.Collections.Generic;
namespace ZhonTai.Admin.Core.GrpcServices.Dtos;

/// <summary>
/// 用户权限
/// </summary>
[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class UserGetPermissionGrpcOutput
{
    public static class Models
    {
        /// <summary>
        /// 接口
        /// </summary>
        [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
        public class ApiModel
        {
            /// <summary>
            /// 请求方法
            /// </summary>
            public string HttpMethods { get; set; }

            /// <summary>
            /// 请求地址
            /// </summary>
            public string Path { get; set; }
        }
    }

    /// <summary>
    /// 接口列表
    /// </summary>
    public List<Models.ApiModel> Apis { get; set; }

    /// <summary>
    /// 权限点编码列表
    /// </summary>
    public List<string> Codes { get; set; }
}
