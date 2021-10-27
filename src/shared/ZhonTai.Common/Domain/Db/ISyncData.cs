using System.Threading.Tasks;
using ZhonTai.Common.Configs;

namespace ZhonTai.Common.Domain.Db
{
    /// <summary>
    /// 同步数据接口
    /// </summary>
    public interface ISyncData
    {
        Task SyncDataAsync(IFreeSql db, DbConfig dbConfig = null, AppConfig appConfig = null);
    }
}
