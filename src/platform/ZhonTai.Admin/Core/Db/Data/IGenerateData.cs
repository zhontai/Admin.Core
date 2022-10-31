using System.Threading.Tasks;
using ZhonTai.Admin.Core.Configs;

namespace ZhonTai.Admin.Core.Db.Data;

/// <summary>
/// 生成数据接口
/// </summary>
public interface IGenerateData
{
    Task GenerateDataAsync(IFreeSql db, AppConfig appConfig);
}
