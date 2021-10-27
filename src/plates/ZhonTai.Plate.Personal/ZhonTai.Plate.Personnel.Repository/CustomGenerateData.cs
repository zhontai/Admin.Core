using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZhonTai.Common.Domain.Db;
using ZhonTai.Common.Configs;
using ZhonTai.Common.Extensions;
using ZhonTai.Plate.Personnel.Domain.Organization;
using ZhonTai.Plate.Personnel.Domain.Organization.Output;
using ZhonTai.Plate.Personnel.Domain.Position;
using ZhonTai.Plate.Personnel.Domain.Position.Output;
using ZhonTai.Plate.Personnel.Domain.Employee;
using ZhonTai.Plate.Personnel.Domain.Employee.Output;
using ZhonTai.Plate.Admin.Domain.Tenant;
using ZhonTai.Plate.Admin.Domain;

namespace ZhonTai.Plate.Personnel.Repository
{
    public class CustomGenerateData : GenerateData, IGenerateData
    {
        public virtual async Task GenerateDataAsync(IFreeSql db, AppConfig appConfig)
        {
            #region 数据表

            //人事
            #region 部门

            var organizations = await db.Queryable<OrganizationEntity>().ToListAsync<OrganizationDataOutput>();
            var organizationTree = organizations.ToTree((r, c) =>
            {
                return c.ParentId == 0;
            },
            (r, c) =>
            {
                return r.Id == c.ParentId;
            },
            (r, datalist) =>
            {
                r.Childs ??= new List<OrganizationDataOutput>();
                r.Childs.AddRange(datalist);
            });

            #endregion

            #region 岗位

            var positions = await db.Queryable<PositionEntity>().ToListAsync<PositionDataOutput>();

            #endregion

            #region 员工

            var employees = await db.Queryable<EmployeeEntity>().ToListAsync<EmployeeDataOutput>();

            #endregion

            #endregion

            #region 生成数据

            var isTenant = appConfig.Tenant;
            SaveDataToJsonFile<OrganizationEntity>(organizationTree, isTenant);
            SaveDataToJsonFile<PositionEntity>(positions, isTenant);
            SaveDataToJsonFile<EmployeeEntity>(employees, isTenant);
            if (isTenant)
            {
                var tenantId = (await db.Queryable<TenantEntity>().Where(a => a.Code.ToLower() == "zhontai").ToOneAsync())?.Id;
                organizationTree = organizations.Where(a => a.TenantId == tenantId).ToList().ToTree((r, c) =>
                {
                    return c.ParentId == 0;
                },
                (r, c) =>
                {
                    return r.Id == c.ParentId;
                },
                (r, datalist) =>
                {
                    r.Childs ??= new List<OrganizationDataOutput>();
                    r.Childs.AddRange(datalist);
                });
                SaveDataToJsonFile<OrganizationEntity>(organizationTree);
                SaveDataToJsonFile<PositionEntity>(positions.Where(a => a.TenantId == tenantId));
                SaveDataToJsonFile<EmployeeEntity>(employees.Where(a => a.TenantId == tenantId));
            }

            #endregion
        }
    }
}
