using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.Tenant;
using Admin.Core.Service.Admin.Tenant.Input;

namespace Admin.Core.Controllers.Admin
{
    /// <summary>
    /// 租户管理
    /// </summary>
    public class TenantController : AreaController
    {
        private readonly ITenantService _roleServices;

        public TenantController(ITenantService roleServices)
        {
            _roleServices = roleServices;
        }

        /// <summary>
        /// 查询单条租户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(long id)
        {
            return await _roleServices.GetAsync(id);
        }

        /// <summary>
        /// 查询分页租户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPage(PageInput<TenantEntity> model)
        {
            return await _roleServices.PageAsync(model);
        }

        /// <summary>
        /// 新增租户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(TenantAddInput input)
        {
            return await _roleServices.AddAsync(input);
        }

        /// <summary>
        /// 修改租户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(TenantUpdateInput input)
        {
            return await _roleServices.UpdateAsync(input);
        }

        /// <summary>
        /// 删除租户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(long id)
        {
            return await _roleServices.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量删除租户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> BatchSoftDelete(long[] ids)
        {
            return await _roleServices.BatchSoftDeleteAsync(ids);
        }
    }
}
