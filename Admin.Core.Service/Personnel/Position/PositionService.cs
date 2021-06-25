using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Personnel;
using Admin.Core.Repository.Personnel;
using Admin.Core.Service.Personnel.Position.Input;
using Admin.Core.Service.Personnel.Position.Output;
using System.Threading.Tasks;

namespace Admin.Core.Service.Personnel.Position
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

        public async Task<IResponseOutput> GetAsync(long id)
        {
            var result = await _positionRepository.GetAsync<PositionGetOutput>(id);
            return ResponseOutput.Ok(result);
        }

        public async Task<IResponseOutput> PageAsync(PageInput<PositionEntity> input)
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

            return ResponseOutput.Ok(data);
        }

        public async Task<IResponseOutput> AddAsync(PositionAddInput input)
        {
            var entity = Mapper.Map<PositionEntity>(input);
            var id = (await _positionRepository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id > 0);
        }

        public async Task<IResponseOutput> UpdateAsync(PositionUpdateInput input)
        {
            if (!(input?.Id > 0))
            {
                return ResponseOutput.NotOk();
            }

            var entity = await _positionRepository.GetAsync(input.Id);
            if (!(entity?.Id > 0))
            {
                return ResponseOutput.NotOk("职位不存在！");
            }

            Mapper.Map(input, entity);
            await _positionRepository.UpdateAsync(entity);
            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> DeleteAsync(long id)
        {
            var result = false;
            if (id > 0)
            {
                result = (await _positionRepository.DeleteAsync(m => m.Id == id)) > 0;
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(long id)
        {
            var result = await _positionRepository.SoftDeleteAsync(id);

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> BatchSoftDeleteAsync(long[] ids)
        {
            var result = await _positionRepository.SoftDeleteAsync(ids);

            return ResponseOutput.Result(result);
        }
    }
}