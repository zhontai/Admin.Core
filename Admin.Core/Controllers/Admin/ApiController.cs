using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.Api;
using Admin.Core.Service.Admin.Api.Input;

namespace Admin.Core.Controllers.Admin
{
    /// <summary>
    /// 接口管理
    /// </summary>
    public class ApiController : AreaController
    {
        private readonly IApiService _apiServices;

        public ApiController(IApiService apiServices)
        {
            _apiServices = apiServices;
        }

        /// <summary>
        /// 查询单条接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(long id)
        {
            return await _apiServices.GetAsync(id);
        }

        /// <summary>
        /// 查询全部接口
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetList(string key)
        {
            return await _apiServices.ListAsync(key);
        }

        /// <summary>
        /// 查询分页接口
        /// </summary>
        /// <param name="model">分页模型</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPage(PageInput<ApiEntity> model)
        {
            return await _apiServices.PageAsync(model);
        }

        /// <summary>
        /// 新增接口
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(ApiAddInput input)
        {
            return await _apiServices.AddAsync(input);
        }

        /// <summary>
        /// 修改接口
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(ApiUpdateInput input)
        {
            return await _apiServices.UpdateAsync(input);
        }

        /// <summary>
        /// 删除接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(long id)
        {
            return await _apiServices.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量删除接口
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> BatchSoftDelete(long[] ids)
        {
            return await _apiServices.BatchSoftDeleteAsync(ids);
        }

        /// <summary>
        /// 同步接口
        /// 支持新增和修改接口
        /// 根据接口是否存在自动禁用和启用api
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Sync(ApiSyncInput input)
        {
            return await _apiServices.SyncAsync(input);
        }
    }
}
