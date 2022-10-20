using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.DocumentImage;

namespace ZhonTai.Admin.Repositories;

public class DocumentImageRepository : RepositoryBase<DocumentImageEntity>, IDocumentImageRepository
{
    public DocumentImageRepository(UnitOfWorkManagerCloud uowm) : base(DbKeys.AppDb, uowm)
    {
    }
}