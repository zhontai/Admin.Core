using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Domain.DocumentImage;

namespace ZhonTai.Admin.Repositories;

public class DocumentImageRepository : AdminRepositoryBase<DocumentImageEntity>, IDocumentImageRepository
{
    public DocumentImageRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}