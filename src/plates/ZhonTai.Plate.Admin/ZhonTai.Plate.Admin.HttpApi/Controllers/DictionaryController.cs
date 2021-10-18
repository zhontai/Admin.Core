using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ZhonTai.Common.Input;
using ZhonTai.Common.Output;
using ZhonTai.Plate.Admin.Domain.Dictionary;
using ZhonTai.Plate.Admin.Service.Dictionary;
using ZhonTai.Plate.Admin.Service.Dictionary.Input;

namespace ZhonTai.Plate.Admin.HttpApi
{
    /// <summary>
    /// 数据字典
    /// </summary>
    public class DictionaryController : AreaController
    {
        private readonly IDictionaryService _dictionaryService;

        public DictionaryController(IDictionaryService dictionaryService)
        {
            _dictionaryService = dictionaryService;
        }

        /// <summary>
        /// 查询单条数据字典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(long id)
        {
            return await _dictionaryService.GetAsync(id);
        }

        /// <summary>
        /// 查询分页数据字典
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPage(PageInput<DictionaryEntity> model)
        {
            return await _dictionaryService.PageAsync(model);
        }

        /// <summary>
        /// 新增数据字典
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(DictionaryAddInput input)
        {
            return await _dictionaryService.AddAsync(input);
        }

        /// <summary>
        /// 修改数据字典
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(DictionaryUpdateInput input)
        {
            return await _dictionaryService.UpdateAsync(input);
        }

        /// <summary>
        /// 删除数据字典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(long id)
        {
            return await _dictionaryService.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量删除数据字典
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> BatchSoftDelete(long[] ids)
        {
            return await _dictionaryService.BatchSoftDeleteAsync(ids);
        }
    }
}