using ZhonTai.Common.Domain.Dto;
using ZhonTai.Plate.Personnel.Service.Position.Input;
using ZhonTai.Plate.Personnel.Service.Position.Output;
using System.Threading.Tasks;
using ZhonTai.Plate.Admin.Service;
using ZhonTai.Plate.Personnel.Domain.Position;

namespace ZhonTai.Plate.Personnel.Service.Position
{
    public class PositionService : BaseService, IPositionService
    {
        private readonly IPositionRepository _positionRepository;

        public PositionService(
            IPositionRepository positionRepository
        )
        {
            _positionRepository = positionRepository;
        }

        public async Task<IResultOutput> GetAsync(long id)
        {
            var result = await _positionRepository.GetAsync<PositionGetOutput>(id);
            return ResultOutput.Ok(result);
        }

        public async Task<IResultOutput> GetPageAsync(PageInput<PositionEntity> input)
        {
            var key = input.Filter?.Name;

            var list = await _positionRepository.Select
            .WhereIf(key.NotNull(), a => a.Name.Contains(key))
            .Count(out var total)
            .OrderByDescending(true, c => c.Id)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync<PositionListOutput>();

            var data = new PageOutput<PositionListOutput>()
            {
                List = list,
                Total = total
            };

            return ResultOutput.Ok(data);
        }

        public async Task<IResultOutput> AddAsync(PositionAddInput input)
        {
            var entity = Mapper.Map<PositionEntity>(input);
            var id = (await _positionRepository.InsertAsync(entity)).Id;

            return ResultOutput.Result(id > 0);
        }

        public async Task<IResultOutput> UpdateAsync(PositionUpdateInput input)
        {
            if (!(input?.Id > 0))
            {
                return ResultOutput.NotOk();
            }

            var entity = await _positionRepository.GetAsync(input.Id);
            if (!(entity?.Id > 0))
            {
                return ResultOutput.NotOk("职位不存在！");
            }

            Mapper.Map(input, entity);
            await _positionRepository.UpdateAsync(entity);
            return ResultOutput.Ok();
        }

        public async Task<IResultOutput> DeleteAsync(long id)
        {
            var result = false;
            if (id > 0)
            {
                result = (await _positionRepository.DeleteAsync(m => m.Id == id)) > 0;
            }

            return ResultOutput.Result(result);
        }

        public async Task<IResultOutput> SoftDeleteAsync(long id)
        {
            var result = await _positionRepository.SoftDeleteAsync(id);

            return ResultOutput.Result(result);
        }

        public async Task<IResultOutput> BatchSoftDeleteAsync(long[] ids)
        {
            var result = await _positionRepository.SoftDeleteAsync(ids);

            return ResultOutput.Result(result);
        }
    }
}