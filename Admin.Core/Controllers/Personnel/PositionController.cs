using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Personnel;
using Admin.Core.Service.Personnel.Position;
using Admin.Core.Service.Personnel.Position.Input;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Admin.Core.Controllers.Personnel
{
    /// <summary>
    /// 职位管理
    /// </summary>
    public class PositionController : AreaController
    {
        private readonly IPositionService _roleServices;

        public PositionController(IPositionService roleServices)
        {
            _roleServices = roleServices;
        }

        /// <summary>
        /// 查询单条职位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(long id)
        {
            return await _roleServices.GetAsync(id);
        }

        /// <summary>
        /// 查询分页职位
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPage(PageInput<PositionEntity> model)
        {
            return await _roleServices.PageAsync(model);
        }

        /// <summary>
        /// 新增职位
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(PositionAddInput input)
        {
            return await _roleServices.AddAsync(input);
        }

        /// <summary>
        /// 修改职位
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(PositionUpdateInput input)
        {
            return await _roleServices.UpdateAsync(input);
        }

        /// <summary>
        /// 删除职位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(long id)
        {
            return await _roleServices.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量删除职位
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