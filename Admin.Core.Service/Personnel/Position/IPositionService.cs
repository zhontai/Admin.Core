using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Personnel;
using Admin.Core.Service.Personnel.Position.Input;
using System.Threading.Tasks;

namespace Admin.Core.Service.Personnel.Position
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