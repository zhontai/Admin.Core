using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Domain.Doc;

namespace ZhonTai.Admin.Repositories;

public class DocRepository : AdminRepositoryBase<DocEntity>, IDocRepository
{
    public DocRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}