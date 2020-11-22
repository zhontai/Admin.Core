using Admin.Core.Model.Admin;
using Admin.Core.Common.Auth;

namespace Admin.Core.Repository.Admin
{
    public class DocumentRepository : RepositoryBase<DocumentEntity>, IDocumentRepository
    {
        public DocumentRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
