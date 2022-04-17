using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.OprationLog;

namespace ZhonTai.Admin.Repositories
{
    public class OprationLogRepository : RepositoryBase<OprationLogEntity>, IOprationLogRepository
    {
        public OprationLogRepository(DbUnitOfWorkManager uowm) : base(uowm)
        {
        }
    }
}