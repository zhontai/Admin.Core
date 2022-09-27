using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.DocumentImage;

namespace ZhonTai.Admin.Repositories;

public class DocumentImageRepository : RepositoryCloud<DocumentImageEntity>, IDocumentImageRepository
{
    public DocumentImageRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}