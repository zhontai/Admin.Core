using ZhonTai.Plate.Admin.Domain.Dictionary;

namespace ZhonTai.Plate.Admin.Repository
{
    public class DictionaryRepository : RepositoryBase<DictionaryEntity>, IDictionaryRepository
    {
        public DictionaryRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}