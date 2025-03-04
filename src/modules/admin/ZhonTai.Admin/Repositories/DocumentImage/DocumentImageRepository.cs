using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Domain.DocImage;

namespace ZhonTai.Admin.Repositories;

public class DocumentImageRepository : AdminRepositoryBase<DocImageEntity>, IDocImageRepository
{
    public DocumentImageRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}