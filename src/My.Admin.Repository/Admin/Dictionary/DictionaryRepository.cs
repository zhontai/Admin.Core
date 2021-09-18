using My.Admin.Domain.Admin;

namespace My.Admin.Repository.Admin
{
    public class DictionaryRepository : RepositoryBase<DictionaryEntity>, IDictionaryRepository
    {
        public DictionaryRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}