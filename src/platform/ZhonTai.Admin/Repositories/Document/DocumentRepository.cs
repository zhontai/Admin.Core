using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.Document;

namespace ZhonTai.Admin.Repositories;

public class DocumentRepository : RepositoryBase<DocumentEntity>, IDocumentRepository
{
    public DocumentRepository(DbUnitOfWorkManager uowm) : base(uowm)
    {
    }
}