using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Domain.Document;

namespace ZhonTai.Admin.Repositories;

public class DocumentRepository : AdminRepositoryBase<DocumentEntity>, IDocumentRepository
{
    public DocumentRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}