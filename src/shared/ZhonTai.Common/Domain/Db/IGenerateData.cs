using System.Threading.Tasks;
using ZhonTai.Common.Configs;

namespace ZhonTai.Common.Domain.Db
{
    /// <summary>
    /// 生成数据接口
    /// </summary>
    public interface IGenerateData
    {
        Task GenerateDataAsync(IFreeSql db, AppConfig appConfig);
    }
}
