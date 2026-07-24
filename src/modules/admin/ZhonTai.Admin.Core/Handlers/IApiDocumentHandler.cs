namespace ZhonTai.Admin.Core.Handlers;

public interface IApiDocumentHandler
{
    /// <summary>
    /// 同步Api文档
    /// </summary>
    /// <returns></returns>
    Task SyncAsync();
}