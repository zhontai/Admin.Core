using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Admin.Core.Service.Admin.Dictionary;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.Dictionary.Input;

namespace Admin.Core.Controllers.Admin
{
    /// <summary>
    /// 数据字典
    /// </summary>
    public class DictionaryController : AreaController
    {
        private readonly IDictionaryService _dictionaryServices;

        public DictionaryController(IDictionaryService dictionaryServices)
        {
            _dictionaryServices = dictionaryServices;
        }

        /// <summary>
        /// 查询单条数据字典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(long id)
        {
            return await _dictionaryServices.GetAsync(id);
        }

        /// <summary>
        /// 查询分页数据字典
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPage(PageInput<DictionaryEntity> model)
        {
            return await _dictionaryServices.PageAsync(model);
        }

        /// <summary>
        /// 新增数据字典
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(DictionaryAddInput input)
        {
            return await _dictionaryServices.AddAsync(input);
        }

        /// <summary>
        /// 修改数据字典
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(DictionaryUpdateInput input)
        {
            return await _dictionaryServices.UpdateAsync(input);
        }

        /// <summary>
        /// 删除数据字典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(long id)
        {
            return await _dictionaryServices.SoftDeleteAsync(id);
        }
    }
}
