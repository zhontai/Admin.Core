using Admin.Core.Model.Admin;

namespace Admin.Core.Repository.Admin
{
    public class DictionaryRepository : RepositoryBase<DictionaryEntity>, IDictionaryRepository
    {
        public DictionaryRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}