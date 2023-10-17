using System.Threading.Tasks;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Services;
using MyApp.Api.Domain.Module;
using MyApp.Api.Domain.Module.Dto;
using MyApp.Api.Services.Module.Input;
using MyApp.Api.Services.Module.Output;
using ZhonTai;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using Microsoft.AspNetCore.Mvc;
using MyApp.Api.Core.Consts;
#if (!NoTaskScheduler)
using FreeScheduler;
using Newtonsoft.Json;
#endif

namespace MyApp.Api.Services.Module;

/// <summary>
/// 模块服务
/// </summary>
[Order(1010)]
[DynamicApi(Area = ApiConsts.AreaName)]
public class ModuleService : BaseService, IModuleService, IDynamicApi
{
    private IModuleRepository _moduleRepository => LazyGetRequiredService<IModuleRepository>();

    public ModuleService()
    {
    }

    /// <summary>
    /// 查询模块
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ModuleGetOutput> GetAsync(long id)
    {
        var result = await _moduleRepository.GetAsync<ModuleGetOutput>(id);
        return result;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<ModuleListOutput>> GetPageAsync(PageInput<ModuleGetPageDto> input)
    {
        var key = input.Filter?.Name;

        var list = await _moduleRepository.Select
        .WhereIf(key.NotNull(), a => a.Name.Contains(key))
        .Count(out var total)
        .OrderByDescending(true, c => c.Id)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync<ModuleListOutput>();

        var data = new PageOutput<ModuleListOutput>()
        {
            List = list,
            Total = total
        };

        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(ModuleAddInput input)
    {
        var entity = Mapper.Map<ModuleEntity>(input);
        await _moduleRepository.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(ModuleUpdateInput input)
    {
        var entity = await _moduleRepository.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception("模块不存在");
        }

        Mapper.Map(input, entity);
        await _moduleRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(long id)
    {
        await _moduleRepository.DeleteAsync(m => m.Id == id);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task SoftDeleteAsync(long id)
    {
        await _moduleRepository.SoftDeleteAsync(id);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task BatchSoftDeleteAsync(long[] ids)
    {
        await _moduleRepository.SoftDeleteAsync(ids);
    }

#if (!NoTaskScheduler)
    /// <summary>
    /// 执行任务
    /// </summary>
    /// <returns></returns>
    public void ExecuteTask()
    {
        var scheduler = LazyGetRequiredService<Scheduler>();
        //方式1：添加任务组，第一组每次间隔15秒，第二组每次间隔2分钟
        scheduler.AddTask(TaskNames.ModuleTaskName, JsonConvert.SerializeObject(new
        {
            moduleId = 1
        }), new int[] { 15, 15, 120, 120 });

        /*
        //方式2：添加任务，每次间隔15秒
        scheduler.AddTask(TaskConsts.ModuleTaskName, JsonConvert.SerializeObject(new
        {
            moduleId = 1
        }), 2, 15);

        //方式3：无限循环任务，每次间隔10分钟
        scheduler.AddTask(TaskConsts.ModuleTaskName, JsonConvert.SerializeObject(new
        {
            moduleId = 1
        }), -1, 600);

        //方式4：每天凌晨执行一次
        scheduler.AddTaskRunOnDay(TaskConsts.ModuleTaskName, JsonConvert.SerializeObject(new
        {
            moduleId = 1
        }), 1, "0:00:00");

        //方式5：每周一晚上11点半执行一次，0为周日，1-6为周一至周六
        scheduler.AddTaskRunOnWeek(TaskConsts.ModuleTaskName, JsonConvert.SerializeObject(new
        {
            moduleId = 1
        }), 1, "1:23:30:00");

        //方式6：每个月1号下午4点执行1次
        scheduler.AddTaskRunOnMonth(TaskConsts.ModuleTaskName, JsonConvert.SerializeObject(new
        {
            moduleId = 1
        }), 1, "1:16:00:00");

        //方式7：自定义cron表达式，从0秒开始每10秒执行一次
        scheduler.AddTaskCustom(TaskConsts.ModuleTaskName, JsonConvert.SerializeObject(new
        {
            moduleId = 1
        }), "0/10 * * * * ?");
        */
    }
#endif
}