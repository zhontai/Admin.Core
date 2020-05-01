using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Admin.Core.Model.Admin;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Service.Admin.View;
using Admin.Core.Service.Admin.View.Input;

namespace Admin.Core.Controllers.Admin
{
    /// <summary>
    /// 视图管理
    /// </summary>
    public class ViewController : AreaController
    {
        private readonly IViewService _viewServices;

        public ViewController(IViewService viewServices)
        {
            _viewServices = viewServices;
        }

        /// <summary>
        /// 查询单条视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(long id)
        {
            return await _viewServices.GetAsync(id);
        }

        /// <summary>
        /// 查询全部视图
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetList(string key)
        {
            return await _viewServices.ListAsync(key);
        }

        /// <summary>
        /// 查询分页视图
        /// </summary>
        /// <param name="model">分页模型</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPage(PageInput<ViewEntity> model)
        {
            return await _viewServices.PageAsync(model);
        }

        /// <summary>
        /// 新增视图
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(ViewAddInput input)
        {
            return await _viewServices.AddAsync(input);
        }

        /// <summary>
        /// 修改视图
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(ViewUpdateInput input)
        {
            return await _viewServices.UpdateAsync(input);
        }

        /// <summary>
        /// 删除视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(long id)
        {
            return await _viewServices.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量删除视图
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> BatchSoftDelete(long[] ids)
        {
            return await _viewServices.BatchSoftDeleteAsync(ids);
        }

        /// <summary>
        /// 同步视图
        /// 支持新增和修改视图
        /// 根据视图是否存在自动禁用和启用视图
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Sync(ViewSyncInput input)
        {
            return await _viewServices.SyncAsync(input);
        }
    }
}
