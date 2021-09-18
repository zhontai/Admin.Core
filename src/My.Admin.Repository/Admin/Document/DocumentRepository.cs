using My.Admin.Domain.Admin;

namespace My.Admin.Repository.Admin
{
    public class DocumentRepository : RepositoryBase<DocumentEntity>, IDocumentRepository
    {
        public DocumentRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}