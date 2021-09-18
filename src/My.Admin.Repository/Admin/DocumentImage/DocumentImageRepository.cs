using My.Admin.Domain.Admin;

namespace My.Admin.Repository.Admin
{
    public class DocumentImageRepository : RepositoryBase<DocumentImageEntity>, IDocumentImageRepository
    {
        public DocumentImageRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}