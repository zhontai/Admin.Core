using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Domain.DocumentImage;

namespace ZhonTai.Admin.Repositories;

public class DocumentImageRepository : AppRepositoryBase<DocumentImageEntity>, IDocumentImageRepository
{
    public DocumentImageRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}