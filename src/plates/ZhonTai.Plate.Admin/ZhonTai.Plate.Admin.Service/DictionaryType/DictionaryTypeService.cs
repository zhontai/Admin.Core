using System.Linq;
using System.Threading.Tasks;
using ZhonTai.Common.Attributes;
using ZhonTai.Common.Domain.Dto;
using ZhonTai.Plate.Admin.Service.DictionaryType.Dto;
using ZhonTai.Plate.Admin.Domain.DictionaryType;
using ZhonTai.Plate.Admin.Domain.Dictionary;
using ZhonTai.Plate.Admin.Domain.DictionaryType.Dto;

namespace ZhonTai.Plate.Admin.Service.DictionaryType
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

        public async Task<IResultOutput> GetAsync(long id)
        {
            var result = await _DictionaryTypeRepository.GetAsync<DictionaryTypeGetOutput>(id);
            return ResultOutput.Ok(result);
        }

        public async Task<IResultOutput> GetPageAsync(PageInput<DictionaryTypeGetPageDto> input)
        {
            var key = input.Filter?.Name;

            var list = await _DictionaryTypeRepository.Select
            .WhereDynamicFilter(input.DynamicFilter)
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

            return ResultOutput.Ok(data);
        }

        public async Task<IResultOutput> AddAsync(DictionaryTypeAddInput input)
        {
            var DictionaryType = Mapper.Map<DictionaryTypeEntity>(input);
            var id = (await _DictionaryTypeRepository.InsertAsync(DictionaryType)).Id;
            return ResultOutput.Result(id > 0);
        }

        public async Task<IResultOutput> UpdateAsync(DictionaryTypeUpdateInput input)
        {
            if (!(input?.Id > 0))
            {
                return ResultOutput.NotOk();
            }

            var entity = await _DictionaryTypeRepository.GetAsync(input.Id);
            if (!(entity?.Id > 0))
            {
                return ResultOutput.NotOk("数据字典不存在！");
            }

            Mapper.Map(input, entity);
            await _DictionaryTypeRepository.UpdateAsync(entity);
            return ResultOutput.Ok();
        }

        [Transaction]
        public async Task<IResultOutput> DeleteAsync(long id)
        {
            //删除字典数据
            await _dictionaryRepository.DeleteAsync(a => a.DictionaryTypeId == id);

            //删除字典类型
            await _DictionaryTypeRepository.DeleteAsync(a => a.Id == id);

            return ResultOutput.Ok();
        }

        [Transaction]
        public async Task<IResultOutput> SoftDeleteAsync(long id)
        {
            await _dictionaryRepository.SoftDeleteAsync(a => a.DictionaryTypeId == id);
            await _DictionaryTypeRepository.SoftDeleteAsync(id);

            return ResultOutput.Ok();
        }

        [Transaction]
        public async Task<IResultOutput> BatchSoftDeleteAsync(long[] ids)
        {
            await _dictionaryRepository.SoftDeleteAsync(a => ids.Contains(a.DictionaryTypeId));
            await _DictionaryTypeRepository.SoftDeleteAsync(ids);

            return ResultOutput.Ok();
        }
    }
}