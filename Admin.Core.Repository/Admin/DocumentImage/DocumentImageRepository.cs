using FreeSql;
using Admin.Core.Model.Admin;
using Admin.Core.Common.Auth;

namespace Admin.Core.Repository.Admin
{
    public class DocumentImageRepository : RepositoryBase<DocumentImageEntity>, IDocumentImageRepository
    {
        public DocumentImageRepository(UnitOfWorkManager uowm, IUser user) : base(uowm, user)
        {
        }
    }
}
