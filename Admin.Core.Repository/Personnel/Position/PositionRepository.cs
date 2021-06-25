using Admin.Core.Model.Personnel;

namespace Admin.Core.Repository.Personnel
{
    public class PositionRepository : RepositoryBase<PositionEntity>, IPositionRepository
    {
        public PositionRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}