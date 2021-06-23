using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Personnel;
using Admin.Core.Service.Personnel.Organization;
using Admin.Core.Service.Personnel.Organization.Input;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Admin.Core.Controllers.Personnel
{
    /// <summary>
    /// 组织机构
    /// </summary>
    public class OrganizationController : AreaController
    {
        private readonly IOrganizationService _organizationServices;

        public OrganizationController(IOrganizationService organizationServices)
        {
            _organizationServices = organizationServices;
        }

        /// <summary>
        /// 查询单条组织机构
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(long id)
        {
            return await _organizationServices.GetAsync(id);
        }

        /// <summary>
        /// 查询分页组织机构
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPage(PageInput<OrganizationEntity> model)
        {
            return await _organizationServices.PageAsync(model);
        }

        /// <summary>
        /// 新增组织机构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(OrganizationAddInput input)
        {
            return await _organizationServices.AddAsync(input);
        }

        /// <summary>
        /// 修改组织机构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(OrganizationUpdateInput input)
        {
            return await _organizationServices.UpdateAsync(input);
        }

        /// <summary>
        /// 删除组织机构
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(long id)
        {
            return await _organizationServices.SoftDeleteAsync(id);
        }
    }
}