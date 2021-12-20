using Admin.Core.Common.Output;
using Admin.Core.Service.Personnel.Organization;
using Admin.Core.Service.Personnel.Organization.Input;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Admin.Core.Controllers.Personnel
{
    /// <summary>
    /// 组织架构
    /// </summary>
    public class OrganizationController : AreaController
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        /// <summary>
        /// 查询单条组织架构
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(long id)
        {
            return await _organizationService.GetAsync(id);
        }

        /// <summary>
        /// 查询组织架构列表
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetList(string key)
        {
            return await _organizationService.GetListAsync(key);
        }

        /// <summary>
        /// 新增组织架构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(OrganizationAddInput input)
        {
            return await _organizationService.AddAsync(input);
        }

        /// <summary>
        /// 修改组织架构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(OrganizationUpdateInput input)
        {
            return await _organizationService.UpdateAsync(input);
        }

        /// <summary>
        /// 删除组织架构
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(long id)
        {
            return await _organizationService.SoftDeleteAsync(id);
        }
    }
}