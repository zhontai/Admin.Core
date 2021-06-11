using Admin.Core.Model.Admin;

namespace Admin.Core.Repository.Admin
{
    public class DocumentRepository : RepositoryBase<DocumentEntity>, IDocumentRepository
    {
        public DocumentRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}