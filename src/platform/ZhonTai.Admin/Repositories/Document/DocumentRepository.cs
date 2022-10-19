using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.Document;

namespace ZhonTai.Admin.Repositories;

public class DocumentRepository : RepositoryBase<DocumentEntity>, IDocumentRepository
{
    public DocumentRepository(UnitOfWorkManagerCloud uowm) : base(DbKeys.AdminDb, uowm)
    {
    }
}