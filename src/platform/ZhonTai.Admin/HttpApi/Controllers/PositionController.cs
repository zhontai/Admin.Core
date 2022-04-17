using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Services.Position;
using ZhonTai.Admin.Services.Position.Input;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ZhonTai.Admin.Domain.Position.Dto;

namespace ZhonTai.Admin.HttpApi.Controllers
{
    /// <summary>
    /// 职位管理
    /// </summary>
    public class PositionController : AreaController
    {
        private readonly IPositionService _positionService;

        public PositionController(IPositionService positionService)
        {
            _positionService = positionService;
        }

        /// <summary>
        /// 查询职位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResultOutput> Get(long id)
        {
            return await _positionService.GetAsync(id);
        }

        /// <summary>
        /// 查询职位列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResultOutput> GetPage(PageInput<PositionGetPageDto> input)
        {
            return await _positionService.GetPageAsync(input);
        }

        /// <summary>
        /// 新增职位
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResultOutput> Add(PositionAddInput input)
        {
            return await _positionService.AddAsync(input);
        }

        /// <summary>
        /// 修改职位
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResultOutput> Update(PositionUpdateInput input)
        {
            return await _positionService.UpdateAsync(input);
        }

        /// <summary>
        /// 删除职位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResultOutput> SoftDelete(long id)
        {
            return await _positionService.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量删除职位
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResultOutput> BatchSoftDelete(long[] ids)
        {
            return await _positionService.BatchSoftDeleteAsync(ids);
        }
    }
}