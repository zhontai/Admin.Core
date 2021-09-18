using My.Admin.Common.Input;
using My.Admin.Common.Output;
using My.Admin.Domain.Admin;
using My.Admin.Repository.Admin;
using My.Admin.Service.Admin.Dictionary.Input;
using My.Admin.Service.Admin.Dictionary.Output;
using System.Threading.Tasks;

namespace My.Admin.Service.Admin.Dictionary
{
    public class DictionaryService : BaseService, IDictionaryService
    {
        private readonly IDictionaryRepository _dictionaryRepository;

        public DictionaryService(IDictionaryRepository dictionaryRepository)
        {
            _dictionaryRepository = dictionaryRepository;
        }

        public async Task<IResponseOutput> GetAsync(long id)
        {
            var result = await _dictionaryRepository.GetAsync<DictionaryGetOutput>(id);
            return ResponseOutput.Ok(result);
        }

        public async Task<IResponseOutput> PageAsync(PageInput<DictionaryEntity> input)
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

            return ResponseOutput.Ok(data);
        }

        public async Task<IResponseOutput> AddAsync(DictionaryAddInput input)
        {
            var dictionary = Mapper.Map<DictionaryEntity>(input);
            var id = (await _dictionaryRepository.InsertAsync(dictionary)).Id;
            return ResponseOutput.Result(id > 0);
        }

        public async Task<IResponseOutput> UpdateAsync(DictionaryUpdateInput input)
        {
            if (!(input?.Id > 0))
            {
                return ResponseOutput.NotOk();
            }

            var entity = await _dictionaryRepository.GetAsync(input.Id);
            if (!(entity?.Id > 0))
            {
                return ResponseOutput.NotOk("数据字典不存在！");
            }

            Mapper.Map(input, entity);
            await _dictionaryRepository.UpdateAsync(entity);
            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> DeleteAsync(long id)
        {
            var result = false;
            if (id > 0)
            {
                result = (await _dictionaryRepository.DeleteAsync(m => m.Id == id)) > 0;
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(long id)
        {
            var result = await _dictionaryRepository.SoftDeleteAsync(id);

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> BatchSoftDeleteAsync(long[] ids)
        {
            var result = await _dictionaryRepository.SoftDeleteAsync(ids);

            return ResponseOutput.Result(result);
        }
    }
}