using My.Admin.Domain.Admin;

namespace My.Admin.Repository.Admin
{
    public class DictionaryTypeRepository : RepositoryBase<DictionaryTypeEntity>, IDictionaryTypeRepository
    {
        public DictionaryTypeRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}