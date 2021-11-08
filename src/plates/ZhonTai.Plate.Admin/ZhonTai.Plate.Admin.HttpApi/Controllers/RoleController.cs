using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ZhonTai.Common.Domain.Dto;
using ZhonTai.Plate.Admin.Domain.Role.Dto;
using ZhonTai.Plate.Admin.Service.Role;
using ZhonTai.Plate.Admin.Service.Role.Input;

namespace ZhonTai.Plate.Admin.HttpApi
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public class RoleController : AreaController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// 查询单条角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResultOutput> Get(long id)
        {
            return await _roleService.GetAsync(id);
        }

        /// <summary>
        /// 查询分页角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResultOutput> GetPage(PageInput<RoleGetPageDto> input)
        {
            return await _roleService.GetPageAsync(input);
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResultOutput> Add(RoleAddInput input)
        {
            return await _roleService.AddAsync(input);
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResultOutput> Update(RoleUpdateInput input)
        {
            return await _roleService.UpdateAsync(input);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResultOutput> SoftDelete(long id)
        {
            return await _roleService.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量删除角色
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResultOutput> BatchSoftDelete(long[] ids)
        {
            return await _roleService.BatchSoftDeleteAsync(ids);
        }
    }
}