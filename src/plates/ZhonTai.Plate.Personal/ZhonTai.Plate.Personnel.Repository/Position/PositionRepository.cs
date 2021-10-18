using ZhonTai.Plate.Admin.Repository;
using ZhonTai.Plate.Personnel.Domain;

namespace ZhonTai.Plate.Personnel.Repository
{
    public class PositionRepository : RepositoryBase<PositionEntity>, IPositionRepository
    {
        public PositionRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}