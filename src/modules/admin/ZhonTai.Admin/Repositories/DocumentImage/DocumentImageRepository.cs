using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Domain.DocumentImage;

namespace ZhonTai.Admin.Repositories;

public class DocumentImageRepository : AdminRepositoryBase<DocImageEntity>, IDocImageRepository
{
    public DocumentImageRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}