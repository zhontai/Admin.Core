using ZhonTai.Plate.Admin.Domain;
using ZhonTai.Plate.Admin.Domain.Document;

namespace ZhonTai.Plate.Admin.Repository
{
    public class DocumentRepository : RepositoryBase<DocumentEntity>, IDocumentRepository
    {
        public DocumentRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}