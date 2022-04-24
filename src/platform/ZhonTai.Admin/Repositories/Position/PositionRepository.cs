using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.Position;

namespace ZhonTai.Admin.Repositories
{
    public class PositionRepository : RepositoryBase<PositionEntity>, IPositionRepository
    {
        public PositionRepository(DbUnitOfWorkManager uowm) : base(uowm)
        {
        }
    }
}