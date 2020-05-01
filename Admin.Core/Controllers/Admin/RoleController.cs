using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.Role;
using Admin.Core.Service.Admin.Role.Input;

namespace Admin.Core.Controllers.Admin
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public class RoleController : AreaController
    {
        private readonly IRoleService _roleServices;

        public RoleController(IRoleService roleServices)
        {
            _roleServices = roleServices;
        }

        /// <summary>
        /// 查询单条角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(long id)
        {
            return await _roleServices.GetAsync(id);
        }

        /// <summary>
        /// 查询分页角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPage(PageInput<RoleEntity> model)
        {
            return await _roleServices.PageAsync(model);
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(RoleAddInput input)
        {
            return await _roleServices.AddAsync(input);
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(RoleUpdateInput input)
        {
            return await _roleServices.UpdateAsync(input);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(long id)
        {
            return await _roleServices.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量删除角色
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
