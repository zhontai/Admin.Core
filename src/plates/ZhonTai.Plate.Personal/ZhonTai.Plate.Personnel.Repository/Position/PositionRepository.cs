using ZhonTai.Plate.Admin.Domain;
using ZhonTai.Plate.Personnel.Domain.Position;

namespace ZhonTai.Plate.Personnel.Repository
{
    public class PositionRepository : RepositoryBase<PositionEntity>, IPositionRepository
    {
        public PositionRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}