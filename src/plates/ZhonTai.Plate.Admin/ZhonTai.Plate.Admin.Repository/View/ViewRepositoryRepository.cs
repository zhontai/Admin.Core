using ZhonTai.Plate.Admin.Domain.View;

namespace ZhonTai.Plate.Admin.Repository
{
    public class ViewRepository : RepositoryBase<ViewEntity>, IViewRepository
    {
        public ViewRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}