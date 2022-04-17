using System.Threading.Tasks;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.Dictionary;
using ZhonTai.Admin.Services.Dictionary.Dto;
using ZhonTai.Admin.Domain.Dictionary.Dto;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace ZhonTai.Admin.Services.Dictionary
{
    /// <summary>
    /// 数据字典服务
    /// </summary>
    [DynamicApi(Area = "admin")]
    public class DictionaryService : BaseService, IDictionaryService, IDynamicApi
    {
        private readonly IDictionaryRepository _dictionaryRepository;

        public DictionaryService(IDictionaryRepository dictionaryRepository)
        {
            _dictionaryRepository = dictionaryRepository;
        }

        /// <summary>
        /// 查询数据字典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IResultOutput> GetAsync(long id)
        {
            var result = await _dictionaryRepository.GetAsync<DictionaryGetOutput>(id);
            return ResultOutput.Ok(result);
        }

        /// <summary>
        /// 查询数据字典列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResultOutput> GetPageAsync(PageInput<DictionaryGetPageDto> input)
        {
            var key = input.Filter?.Name;
            var dictionaryTypeId = input.Filter?.DictionaryTypeId;
            var list = await _dictionaryRepository.Select
            .WhereDynamicFilter(input.DynamicFilter)
            .WhereIf(dictionaryTypeId.HasValue && dictionaryTypeId.Value > 0, a => a.DictionaryTypeId == dictionaryTypeId)
            .WhereIf(key.NotNull(), a => a.Name.Contains(key) || a.Code.Contains(key))
            .Count(out var total)
            .OrderByDescending(true, c => c.Id)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync<DictionaryListOutput>();

            var data = new PageOutput<DictionaryListOutput>()
            {
                List = list,
                Total = total
            };

            return ResultOutput.Ok(data);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IResultOutput> AddAsync(DictionaryAddInput input)
        {
            var dictionary = Mapper.Map<DictionaryEntity>(input);
            var id = (await _dictionaryRepository.InsertAsync(dictionary)).Id;
            return ResultOutput.Result(id > 0);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IResultOutput> UpdateAsync(DictionaryUpdateInput input)
        {
            if (!(input?.Id > 0))
            {
                return ResultOutput.NotOk();
            }

            var entity = await _dictionaryRepository.GetAsync(input.Id);
            if (!(entity?.Id > 0))
            {
                return ResultOutput.NotOk("数据字典不存在！");
            }

            Mapper.Map(input, entity);
            await _dictionaryRepository.UpdateAsync(entity);
            return ResultOutput.Ok();
        }

        /// <summary>
        /// 彻底删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IResultOutput> DeleteAsync(long id)
        {
            var result = false;
            if (id > 0)
            {
                result = (await _dictionaryRepository.DeleteAsync(m => m.Id == id)) > 0;
            }

            return ResultOutput.Result(result);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IResultOutput> SoftDeleteAsync(long id)
        {
            var result = await _dictionaryRepository.SoftDeleteAsync(id);

            return ResultOutput.Result(result);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IResultOutput> BatchSoftDeleteAsync(long[] ids)
        {
            var result = await _dictionaryRepository.SoftDeleteAsync(ids);

            return ResultOutput.Result(result);
        }
    }
}