using Admin.Core.Model.Admin;

namespace Admin.Core.Repository.Admin
{
    public class DocumentImageRepository : RepositoryBase<DocumentImageEntity>, IDocumentImageRepository
    {
        public DocumentImageRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}