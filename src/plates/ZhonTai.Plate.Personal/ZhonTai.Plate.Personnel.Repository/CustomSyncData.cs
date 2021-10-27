using System.Threading.Tasks;
using ZhonTai.Common.Domain.Db;
using ZhonTai.Common.Configs;
using ZhonTai.Plate.Admin.Domain.Dual;
using ZhonTai.Plate.Admin.Domain;
using ZhonTai.Plate.Personnel.Domain.Organization;
using ZhonTai.Plate.Personnel.Domain.Position;
using ZhonTai.Plate.Personnel.Domain.Employee;

namespace ZhonTai.Plate.Personnel.Repository
{
    public class CustomSyncData : SyncData, ISyncData
    {
        public virtual async Task SyncDataAsync(IFreeSql db, DbConfig dbConfig = null, AppConfig appConfig = null)
        {
            using (var uow = db.CreateUnitOfWork())
            using (var tran = uow.GetOrBeginTransaction())
            {
                var dualRepo = db.GetRepositoryBase<DualEntity>();
                dualRepo.UnitOfWork = uow;
                if (!await dualRepo.Select.AnyAsync())
                {
                    await dualRepo.InsertAsync(new DualEntity { });
                }

                var isTenant = appConfig.Tenant;

                var organizations = GetData<OrganizationEntity>(isTenant);
                await InitDataAsync(db, uow, tran, organizations, dbConfig);

                var positions = GetData<PositionEntity>(isTenant);
                await InitDataAsync(db, uow, tran, positions, dbConfig);

                var employees = GetData<EmployeeEntity>(isTenant);
                await InitDataAsync(db, uow, tran, employees, dbConfig);

                uow.Commit();
            }
        }
    }
}
