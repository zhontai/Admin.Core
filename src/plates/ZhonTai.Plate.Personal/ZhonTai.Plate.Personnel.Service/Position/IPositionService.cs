using ZhonTai.Common.Input;
using ZhonTai.Common.Output;
using ZhonTai.Plate.Personnel.Domain;
using ZhonTai.Plate.Personnel.Service.Position.Input;
using System.Threading.Tasks;

namespace ZhonTai.Plate.Personnel.Service.Position
{
    public interface IPositionService
    {
        Task<IResponseOutput> GetAsync(long id);

        Task<IResponseOutput> PageAsync(PageInput<PositionEntity> input);

        Task<IResponseOutput> AddAsync(PositionAddInput input);

        Task<IResponseOutput> UpdateAsync(PositionUpdateInput input);

        Task<IResponseOutput> DeleteAsync(long id);

        Task<IResponseOutput> SoftDeleteAsync(long id);

        Task<IResponseOutput> BatchSoftDeleteAsync(long[] ids);
    }
}