using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.Document;

namespace ZhonTai.Admin.Repositories;

public class DocumentRepository : RepositoryCloud<DocumentEntity>, IDocumentRepository
{
    public DocumentRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}