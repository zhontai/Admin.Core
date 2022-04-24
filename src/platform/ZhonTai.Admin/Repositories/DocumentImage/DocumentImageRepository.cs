using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.DocumentImage;

namespace ZhonTai.Admin.Repositories
{
    public class DocumentImageRepository : RepositoryBase<DocumentImageEntity>, IDocumentImageRepository
    {
        public DocumentImageRepository(DbUnitOfWorkManager uowm) : base(uowm)
        {
        }
    }
}