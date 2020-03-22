using System.Collections.Generic;

namespace Admin.Core.Service.Admin.Api.Input
{
    /// <summary>
    /// 接口同步
    /// </summary>
    public class ApiSyncInput
    {
        public List<ApiSyncDto> Apis { get; set; }
    }
}
