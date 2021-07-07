using Admin.Core.Model.Admin;

namespace Admin.Core.Repository.Admin
{
    public class DictionaryTypeRepository : RepositoryBase<DictionaryTypeEntity>, IDictionaryTypeRepository
    {
        public DictionaryTypeRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}