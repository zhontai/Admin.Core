using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.Dictionary;

namespace ZhonTai.Admin.Repositories;

public class DictionaryRepository : RepositoryCloud<DictionaryEntity>, IDictionaryRepository
{
    public DictionaryRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}