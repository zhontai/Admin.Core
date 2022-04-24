using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.DictionaryType;

namespace ZhonTai.Admin.Repositories
{
    public class DictionaryTypeRepository : RepositoryBase<DictionaryTypeEntity>, IDictionaryTypeRepository
    {
        public DictionaryTypeRepository(DbUnitOfWorkManager uowm) : base(uowm)
        {
        }
    }
}