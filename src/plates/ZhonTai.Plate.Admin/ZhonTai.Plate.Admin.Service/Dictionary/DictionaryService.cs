using System.Threading.Tasks;
using ZhonTai.Common.Domain.Dto;
using ZhonTai.Plate.Admin.Domain.Dictionary;
using ZhonTai.Plate.Admin.Service.Dictionary.Output;
using ZhonTai.Plate.Admin.Service.Dictionary.Input;

namespace ZhonTai.Plate.Admin.Service.Dictionary
{
    public class DictionaryService : BaseService, IDictionaryService
    {
        private readonly IDictionaryRepository _dictionaryRepository;

        public DictionaryService(IDictionaryRepository dictionaryRepository)
        {
            _dictionaryRepository = dictionaryRepository;
        }

        public async Task<IResultOutput> GetAsync(long id)
        {
            var result = await _dictionaryRepository.GetAsync<DictionaryGetOutput>(id);
            return ResultOutput.Ok(result);
        }

        public async Task<IResultOutput> GetPageAsync(PageInput<DictionaryEntity> input)
        {
            var key = input.Filter?.Name;
            var dictionaryTypeId = input.Filter?.DictionaryTypeId;
            var list = await _dictionaryRepository.Select
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

        public async Task<IResultOutput> AddAsync(DictionaryAddInput input)
        {
            var dictionary = Mapper.Map<DictionaryEntity>(input);
            var id = (await _dictionaryRepository.InsertAsync(dictionary)).Id;
            return ResultOutput.Result(id > 0);
        }

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

        public async Task<IResultOutput> DeleteAsync(long id)
        {
            var result = false;
            if (id > 0)
            {
                result = (await _dictionaryRepository.DeleteAsync(m => m.Id == id)) > 0;
            }

            return ResultOutput.Result(result);
        }

        public async Task<IResultOutput> SoftDeleteAsync(long id)
        {
            var result = await _dictionaryRepository.SoftDeleteAsync(id);

            return ResultOutput.Result(result);
        }

        public async Task<IResultOutput> BatchSoftDeleteAsync(long[] ids)
        {
            var result = await _dictionaryRepository.SoftDeleteAsync(ids);

            return ResultOutput.Result(result);
        }
    }
}