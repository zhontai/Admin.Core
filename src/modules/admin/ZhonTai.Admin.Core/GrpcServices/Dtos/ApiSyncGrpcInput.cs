using ProtoBuf;

namespace ZhonTai.Admin.Core.GrpcServices.Dtos;

/// <summary>
/// 接口同步
/// </summary>
[ProtoContract(ImplicitFields = ImplicitFields.None)]
public class ApiSyncGrpcInput
{
    public static class Models
    {
        /// <summary>
        /// 接口同步模型
        /// </summary>
        [ProtoContract(ImplicitFields = ImplicitFields.None)]
        public class ApiSyncModel
        {
            /// <summary>
            /// 接口名称
            /// </summary>
            [ProtoMember(1)]
            public string Label { get; set; }

            /// <summary>
            /// 接口地址
            /// </summary>
            [ProtoMember(2)]
            public string Path { get; set; }

            /// <summary>
            /// 父级路径
            /// </summary>
            [ProtoMember(3)]
            public string ParentPath { get; set; }

            /// <summary>
            /// 接口提交方法
            /// </summary>
            [ProtoMember(4)]
            public string HttpMethods { get; set; }
        }
    }

    /// <summary>
    /// 接口同步列表
    /// </summary>
    public List<Models.ApiSyncModel> Apis { get; set; }
}