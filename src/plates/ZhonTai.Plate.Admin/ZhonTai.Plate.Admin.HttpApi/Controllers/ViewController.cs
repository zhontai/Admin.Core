using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ZhonTai.Common.Domain.Dto;
using ZhonTai.Plate.Admin.Domain.View.Dto;
using ZhonTai.Plate.Admin.Service.View;
using ZhonTai.Plate.Admin.Service.View.Dto;

namespace ZhonTai.Plate.Admin.HttpApi
{
    /// <summary>
    /// 视图管理
    /// </summary>
    public class ViewController : AreaController
    {
        private readonly IViewService _viewService;

        public ViewController(IViewService viewService)
        {
            _viewService = viewService;
        }

        /// <summary>
        /// 查询单条视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResultOutput> Get(long id)
        {
            return await _viewService.GetAsync(id);
        }

        /// <summary>
        /// 查询全部视图
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResultOutput> GetList(string key)
        {
            return await _viewService.GetListAsync(key);
        }

        /// <summary>
        /// 查询分页视图
        /// </summary>
        /// <param name="input">分页模型</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResultOutput> GetPage(PageInput<ViewGetPageDto> input)
        {
            return await _viewService.GetPageAsync(input);
        }

        /// <summary>
        /// 新增视图
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResultOutput> Add(ViewAddInput input)
        {
            return await _viewService.AddAsync(input);
        }

        /// <summary>
        /// 修改视图
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResultOutput> Update(ViewUpdateInput input)
        {
            return await _viewService.UpdateAsync(input);
        }

        /// <summary>
        /// 删除视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResultOutput> SoftDelete(long id)
        {
            return await _viewService.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量删除视图
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResultOutput> BatchSoftDelete(long[] ids)
        {
            return await _viewService.BatchSoftDeleteAsync(ids);
        }

        /// <summary>
        /// 同步视图
        /// 支持新增和修改视图
        /// 根据视图是否存在自动禁用和启用视图
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResultOutput> Sync(ViewSyncInput input)
        {
            return await _viewService.SyncAsync(input);
        }
    }
}