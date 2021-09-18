using My.Admin.Domain.Personnel;

namespace My.Admin.Repository.Personnel
{
    public class PositionRepository : RepositoryBase<PositionEntity>, IPositionRepository
    {
        public PositionRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}