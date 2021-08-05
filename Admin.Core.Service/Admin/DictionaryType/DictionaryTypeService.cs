using Admin.Core.Common.Attributes;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Admin;
using Admin.Core.Repository.Admin;
using Admin.Core.Service.Admin.DictionaryType.Input;
using Admin.Core.Service.Admin.DictionaryType.Output;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Core.Service.Admin.DictionaryType
{
    public class DictionaryTypeService : BaseService, IDictionaryTypeService
    {
        private readonly IDictionaryTypeRepository _DictionaryTypeRepository;
        private readonly IDictionaryRepository _dictionaryRepository;
        public DictionaryTypeService(IDictionaryTypeRepository DictionaryTypeRepository, IDictionaryRepository dictionaryRepository)
        {
            _DictionaryTypeRepository = DictionaryTypeRepository;
            _dictionaryRepository = dictionaryRepository;
        }

        public async Task<IResponseOutput> GetAsync(long id)
        {
            var result = await _DictionaryTypeRepository.GetAsync<DictionaryTypeGetOutput>(id);
            return ResponseOutput.Ok(result);
        }

        public async Task<IResponseOutput> PageAsync(PageInput<DictionaryTypeEntity> input)
        {
            var key = input.Filter?.Name;

            var list = await _DictionaryTypeRepository.Select
            .WhereIf(key.NotNull(), a => a.Name.Contains(key) || a.Code.Contains(key))
            .Count(out var total)
            .OrderByDescending(true, c => c.Id)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync<DictionaryTypeListOutput>();

            var data = new PageOutput<DictionaryTypeListOutput>()
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Ok(data);
        }

        public async Task<IResponseOutput> AddAsync(DictionaryTypeAddInput input)
        {
            var DictionaryType = Mapper.Map<DictionaryTypeEntity>(input);
            var id = (await _DictionaryTypeRepository.InsertAsync(DictionaryType)).Id;
            return ResponseOutput.Result(id > 0);
        }

        public async Task<IResponseOutput> UpdateAsync(DictionaryTypeUpdateInput input)
        {
            if (!(input?.Id > 0))
            {
                return ResponseOutput.NotOk();
            }

            var entity = await _DictionaryTypeRepository.GetAsync(input.Id);
            if (!(entity?.Id > 0))
            {
                return ResponseOutput.NotOk("数据字典不存在！");
            }

            Mapper.Map(input, entity);
            await _DictionaryTypeRepository.UpdateAsync(entity);
            return ResponseOutput.Ok();
        }

        [Transaction]
        public async Task<IResponseOutput> DeleteAsync(long id)
        {
            //删除字典数据
            await _dictionaryRepository.DeleteAsync(a => a.DictionaryTypeId == id);

            //删除字典类型
            await _DictionaryTypeRepository.DeleteAsync(a => a.Id == id);

            return ResponseOutput.Ok();
        }

        [Transaction]
        public async Task<IResponseOutput> SoftDeleteAsync(long id)
        {
            await _dictionaryRepository.SoftDeleteAsync(a => a.DictionaryTypeId == id);
            await _DictionaryTypeRepository.SoftDeleteAsync(id);

            return ResponseOutput.Ok();
        }

        [Transaction]
        public async Task<IResponseOutput> BatchSoftDeleteAsync(long[] ids)
        {
            await _dictionaryRepository.SoftDeleteAsync(a => ids.Contains(a.DictionaryTypeId));
            await _DictionaryTypeRepository.SoftDeleteAsync(ids);

            return ResponseOutput.Ok();
        }
    }
}