using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.Dictionary;

namespace ZhonTai.Admin.Repositories;

public class DictionaryRepository : RepositoryBase<DictionaryEntity>, IDictionaryRepository
{
    public DictionaryRepository(UnitOfWorkManagerCloud uowm) : base(DbKeys.AppDb, uowm)
    {
    }
}