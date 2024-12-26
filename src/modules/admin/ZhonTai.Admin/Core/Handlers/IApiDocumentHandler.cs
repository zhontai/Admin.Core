using System.Threading.Tasks;
using ZhonTai.Admin.Services.Api.Dto;

namespace ZhonTai.Admin.Core.Handlers;

public interface IApiDocumentHandler
{
    /// <summary>
    /// 同步API文档数据
    /// </summary>
    /// <returns></returns>
    Task SyncAsync();
}