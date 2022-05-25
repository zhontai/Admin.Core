using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Services.Position.Input;
using System.Threading.Tasks;
using ZhonTai.Admin.Domain.Position.Dto;

namespace ZhonTai.Admin.Services.Position
{
    public interface IPositionService
    {
        Task<IResultOutput> GetAsync(long id);

        Task<IResultOutput> GetPageAsync(PageInput<PositionGetPageDto> input);

        Task<IResultOutput> AddAsync(PositionAddInput input);

        Task<IResultOutput> UpdateAsync(PositionUpdateInput input);

        Task<IResultOutput> DeleteAsync(long id);

        Task<IResultOutput> SoftDeleteAsync(long id);

        Task<IResultOutput> BatchSoftDeleteAsync(long[] ids);
    }
}