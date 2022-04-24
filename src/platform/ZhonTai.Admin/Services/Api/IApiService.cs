using ZhonTai.Admin.Core.Dto;
using System.Threading.Tasks;
using ZhonTai.Admin.Services.Api.Dto;
using ZhonTai.Admin.Domain.Api.Dto;

namespace ZhonTai.Admin.Services.Api
{
    /// <summary>
    /// api½Ó¿Ú
    /// </summary>
    public interface IApiService
    {
        Task<IResultOutput> GetAsync(long id);

        Task<IResultOutput> GetListAsync(string key);

        Task<IResultOutput> GetPageAsync(PageInput<ApiGetPageDto> input);

        Task<IResultOutput> AddAsync(ApiAddInput input);

        Task<IResultOutput> UpdateAsync(ApiUpdateInput input);

        Task<IResultOutput> DeleteAsync(long id);

        Task<IResultOutput> SoftDeleteAsync(long id);

        Task<IResultOutput> BatchSoftDeleteAsync(long[] ids);

        Task<IResultOutput> SyncAsync(ApiSyncInput input);
    }
}