using ZhonTai.Common.Domain.Dto;
using System.Threading.Tasks;
using ZhonTai.Plate.Admin.Service.Api.Dto;
using ZhonTai.Plate.Admin.Domain.Api.Dto;

namespace ZhonTai.Plate.Admin.Service.Api
{
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