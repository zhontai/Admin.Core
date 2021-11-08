using ZhonTai.Common.Domain.Dto;
using ZhonTai.Plate.Personnel.Service.Position.Input;
using System.Threading.Tasks;
using ZhonTai.Plate.Personnel.Domain.Position.Dto;

namespace ZhonTai.Plate.Personnel.Service.Position
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