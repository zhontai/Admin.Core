using System.Threading.Tasks;
using AutoMapper;
using Admin.Core.Common.Output;
using Admin.Core.Common.Input;
using Admin.Core.Model.Admin;
using Admin.Core.Repository.Admin;
using Admin.Core.Service.Admin.Dictionary.Input;
using Admin.Core.Service.Admin.Dictionary.Output;

namespace Admin.Core.Service.Admin.Dictionary
{
    public class DictionaryService : IDictionaryService
    {
        private readonly IMapper _mapper;
        private readonly IDictionaryRepository _dictionaryRepository;

        public DictionaryService(IMapper mapper, IDictionaryRepository dictionaryRepository)
        {
            _mapper = mapper;
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

            var list = await _dictionaryRepository.Select
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
            var dictionary = _mapper.Map<DictionaryEntity>(input);
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

            _mapper.Map(input, entity);
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
    }
}
