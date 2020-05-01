using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Admin;
using Admin.Core.Repository.Admin;
using Admin.Core.Service.Admin.Api.Input;
using Admin.Core.Service.Admin.Api.Output;
using Admin.Core.Common.Attributes;

namespace Admin.Core.Service.Admin.Api
{
    public class ApiService : IApiService
    {
        private readonly IMapper _mapper;
        private readonly IApiRepository _apiRepository;
        public ApiService(IMapper mapper, IApiRepository moduleRepository)
        {
            _mapper = mapper;
            _apiRepository = moduleRepository;
        }

        public async Task<IResponseOutput> GetAsync(long id)
        {
            var result = await _apiRepository.GetAsync<ApiGetOutput>(id);
            return ResponseOutput.Ok(result);
        }

        public async Task<IResponseOutput> ListAsync(string key)
        {
            var data = await _apiRepository
                .WhereIf(key.NotNull(), a => a.Path.Contains(key) || a.Label.Contains(key))
                .ToListAsync<ApiListOutput>();

            return ResponseOutput.Ok(data);
        }

        public async Task<IResponseOutput> PageAsync(PageInput<ApiEntity> input)
        {
            var key = input.Filter?.Label;

            var list = await _apiRepository.Select
            .WhereIf(key.NotNull(), a => a.Path.Contains(key) || a.Label.Contains(key))
            .Count(out var total)
            .OrderByDescending(true, c => c.Id)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync();

            var data = new PageOutput<ApiEntity>() 
            { 
                List = list,
                Total = total
            };

            return ResponseOutput.Ok(data);
        }

        public async Task<IResponseOutput> AddAsync(ApiAddInput input)
        {
            var entity = _mapper.Map<ApiEntity>(input);
            var id = (await _apiRepository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id > 0);
        }

        public async Task<IResponseOutput> UpdateAsync(ApiUpdateInput input)
        {
            if (!(input?.Id > 0))
            {
                return ResponseOutput.NotOk();
            }

            var entity = await _apiRepository.GetAsync(input.Id);
            if (!(entity?.Id > 0))
            {
                return ResponseOutput.NotOk("接口不存在！");
            }

            _mapper.Map(input, entity);
            await _apiRepository.UpdateAsync(entity);
            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> DeleteAsync(long id)
        {
            var result = false;
            if (id > 0)
            {
                result = (await _apiRepository.DeleteAsync(m => m.Id == id)) > 0;
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(long id)
        {
            var result = await _apiRepository.SoftDeleteAsync(id);
            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> BatchSoftDeleteAsync(long[] ids)
        {
            var result = await _apiRepository.SoftDeleteAsync(ids);

            return ResponseOutput.Result(result);
        }

        [Transaction]
        public async Task<IResponseOutput> SyncAsync(ApiSyncInput input)
        {
            //查询所有api
            var apis = await _apiRepository.Select.ToListAsync();
            var paths = apis.Select(a => a.Path).ToList();

            //path处理
            foreach (var api in input.Apis)
            {
                api.Path = api.Path?.Trim().ToLower();
                api.ParentPath = api.ParentPath?.Trim().ToLower();
            }

            #region 执行插入
            //执行父级api插入
            var parentApis = input.Apis.FindAll(a => a.ParentPath.IsNull());
            var pApis = (from a in parentApis where !paths.Contains(a.Path) select a).ToList();
            if (pApis.Count > 0)
            {
                var insertPApis = _mapper.Map<List<ApiEntity>>(pApis);
                insertPApis = await _apiRepository.InsertAsync(insertPApis);
                apis.AddRange(insertPApis);
            }

            //执行子级api插入
            var childApis = input.Apis.FindAll(a => a.ParentPath.NotNull());
            var cApis = (from a in childApis where !paths.Contains(a.Path) select a).ToList();
            if (cApis.Count > 0)
            {
                var insertCApis = _mapper.Map<List<ApiEntity>>(cApis);
                insertCApis = await _apiRepository.InsertAsync(insertCApis);
                apis.AddRange(insertCApis);
            }
            #endregion

            #region 修改和禁用
            {
                //api修改
                ApiEntity a;
                List<string> labels;
                string label;
                string desc;
                foreach (var api in parentApis)
                {
                    a = apis.Find(a => a.Path == api.Path);
                    if (a?.Id > 0)
                    {
                        labels = api.Label?.Split("\r\n")?.ToList();
                        label = labels != null && labels.Count > 0 ? labels[0] : string.Empty;
                        desc = labels != null && labels.Count > 1 ? string.Join("\r\n", labels.GetRange(1, labels.Count() - 1)) : string.Empty;
                        a.ParentId = 0;
                        a.Label = label;
                        a.Description = desc;
                        a.Enabled = true;
                    }
                }
            }

            {
                //api修改
                ApiEntity a;
                ApiEntity pa;
                List<string> labels;
                string label;
                string desc;
                foreach (var api in childApis)
                {
                    a = apis.Find(a => a.Path == api.Path);
                    pa = apis.Find(a => a.Path == api.ParentPath);
                    if (a?.Id > 0)
                    {
                        labels = api.Label?.Split("\r\n")?.ToList();
                        label = labels != null && labels.Count > 0 ? labels[0] : string.Empty;
                        desc = labels != null && labels.Count > 1 ? string.Join("\r\n", labels.GetRange(1, labels.Count() - 1)) : string.Empty;

                        a.ParentId = pa.Id;
                        a.Label = label;
                        a.Description = desc;
                        a.HttpMethods = api.HttpMethods;
                        a.Enabled = true;
                    }
                }
            }

            {
                //api禁用
                var inputPaths = input.Apis.Select(a => a.Path).ToList();
                var disabledApis = (from a in apis where !inputPaths.Contains(a.Path) select a).ToList();
                if (disabledApis.Count > 0)
                {
                    foreach (var api in disabledApis)
                    {
                        api.Enabled = false;
                    }
                }
            }
            #endregion

            //批量更新
            await _apiRepository.UpdateDiy.SetSource(apis)
            .UpdateColumns(a => new { a.ParentId, a.Label, a.HttpMethods,a.Description,a.Enabled })
            .ExecuteAffrowsAsync();

            return ResponseOutput.Ok();
        }
    }
}
