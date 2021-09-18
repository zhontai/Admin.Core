using My.Admin.Common.Input;
using My.Admin.Common.Output;
using My.Admin.Domain.Admin;
using My.Admin.Service.Admin.DictionaryType;
using My.Admin.Service.Admin.DictionaryType.Input;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace My.Admin.HttpApi.Host.Controllers.Admin
{
    /// <summary>
    /// 数据字典类型
    /// </summary>
    public class DictionaryTypeController : AreaController
    {
        private readonly IDictionaryTypeService _DictionaryTypeService;

        public DictionaryTypeController(IDictionaryTypeService DictionaryTypeService)
        {
            _DictionaryTypeService = DictionaryTypeService;
        }

        /// <summary>
        /// 查询单条数据字典类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(long id)
        {
            return await _DictionaryTypeService.GetAsync(id);
        }

        /// <summary>
        /// 查询分页数据字典类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPage(PageInput<DictionaryTypeEntity> model)
        {
            return await _DictionaryTypeService.PageAsync(model);
        }

        /// <summary>
        /// 新增数据字典类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(DictionaryTypeAddInput input)
        {
            return await _DictionaryTypeService.AddAsync(input);
        }

        /// <summary>
        /// 修改数据字典类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(DictionaryTypeUpdateInput input)
        {
            return await _DictionaryTypeService.UpdateAsync(input);
        }

        /// <summary>
        /// 删除数据字典类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(long id)
        {
            return await _DictionaryTypeService.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量删除数据字典类型
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> BatchSoftDelete(long[] ids)
        {
            return await _DictionaryTypeService.BatchSoftDeleteAsync(ids);
        }
    }
}