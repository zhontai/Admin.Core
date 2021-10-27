using ZhonTai.Plate.Admin.Domain;
using ZhonTai.Plate.Admin.Domain.DictionaryType;

namespace ZhonTai.Plate.Admin.Repository
{
    public class DictionaryTypeRepository : RepositoryBase<DictionaryTypeEntity>, IDictionaryTypeRepository
    {
        public DictionaryTypeRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}