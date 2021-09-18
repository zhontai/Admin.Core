using My.Admin.Common.Input;
using My.Admin.Common.Output;
using My.Admin.Domain.Personnel;
using My.Admin.Service.Personnel.Position.Input;
using System.Threading.Tasks;

namespace My.Admin.Service.Personnel.Position
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