using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.Dictionary;

namespace ZhonTai.Admin.Repositories
{
    public class DictionaryRepository : RepositoryBase<DictionaryEntity>, IDictionaryRepository
    {
        public DictionaryRepository(DbUnitOfWorkManager uowm) : base(uowm)
        {
        }
    }
}