using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Admin.Core.Common.Output;
using Admin.Core.Common.Input;
using Admin.Core.Model.Admin;
using Admin.Core.Repository.Admin;
using Admin.Core.Service.Admin.View.Input;
using Admin.Core.Service.Admin.View.Output;
using Admin.Core.Common.Attributes;

namespace Admin.Core.Service.Admin.View
{
    public class ViewService : IViewService
    {
        private readonly IMapper _mapper;
        private readonly IViewRepository _viewRepository;
        public ViewService(IMapper mapper, IViewRepository moduleRepository)
        {
            _viewRepository = moduleRepository;
            _mapper = mapper;
        }

        public async Task<IResponseOutput> GetAsync(long id)
        {
            var result = await _viewRepository.GetAsync<ViewGetOutput>(id);
            return ResponseOutput.Ok(result);
        }

        public async Task<IResponseOutput> ListAsync(string key)
        {
            var data = await _viewRepository
                .WhereIf(key.NotNull(), a => a.Path.Contains(key) || a.Label.Contains(key))
                .OrderBy(a => a.ParentId)
                .OrderBy(a => a.Sort)
                .ToListAsync<ViewListOutput>();

            return ResponseOutput.Ok(data);
        }

        public async Task<IResponseOutput> PageAsync(PageInput<ViewEntity> input)
        {
            var key = input.Filter?.Label;

            long total;
            var list = await _viewRepository.Select
            .WhereIf(key.NotNull(), a => a.Path.Contains(key) || a.Label.Contains(key))
            .Count(out total)
            .OrderByDescending(true, c => c.Id)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync();

            var data = new PageOutput<ViewEntity>() 
            { 
                List = list,
                Total = total
            };

            return ResponseOutput.Ok(data);
        }

        public async Task<IResponseOutput> AddAsync(ViewAddInput input)
        {
            var entity = _mapper.Map<ViewEntity>(input);
            var id = (await _viewRepository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id > 0);
        }

        public async Task<IResponseOutput> UpdateAsync(ViewUpdateInput input)
        {
            if (!(input?.Id > 0))
            {
                return ResponseOutput.NotOk();
            }

            var entity = await _viewRepository.GetAsync(input.Id);
            if (!(entity?.Id > 0))
            {
                return ResponseOutput.NotOk("视图不存在！");
            }

            _mapper.Map(input, entity);
            await _viewRepository.UpdateAsync(entity);
            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> DeleteAsync(long id)
        {
            var result = false;
            if (id > 0)
            {
                result = (await _viewRepository.DeleteAsync(m => m.Id == id)) > 0;
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(long id)
        {
            var result = await _viewRepository.SoftDeleteAsync(id);

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> BatchSoftDeleteAsync(long[] ids)
        {
            var result = await _viewRepository.SoftDeleteAsync(ids);

            return ResponseOutput.Result(result);
        }

        [Transaction]
        public async Task<IResponseOutput> SyncAsync(ViewSyncInput input)
        {
            //查询所有view
            var views = await _viewRepository.Select.ToListAsync();
            var paths = views.Select(a => a.Path).ToList();

            //path处理
            foreach (var view in input.Views)
            {
                view.Path = view.Path?.Trim().ToLower();
                view.ParentPath = view.ParentPath?.Trim().ToLower();
            }


            #region 执行插入
            //执行父级view插入
            var parentViews = input.Views.FindAll(a => a.ParentPath.IsNull());
            var pViews = (from a in parentViews where !paths.Contains(a.Path) select a).ToList();
            if (pViews.Count > 0)
            {
                var insertPViews = _mapper.Map<List<ViewEntity>>(pViews);
                insertPViews = await _viewRepository.InsertAsync(insertPViews);
                views.AddRange(insertPViews);
            }

            //执行子级view插入
            var childViews = input.Views.FindAll(a => a.ParentPath.NotNull());
            var cViews = (from a in childViews where !paths.Contains(a.Path) select a).ToList();
            if (cViews.Count > 0)
            {
                var insertCViews = _mapper.Map<List<ViewEntity>>(cViews);
                insertCViews = await _viewRepository.InsertAsync(insertCViews);
                views.AddRange(insertCViews);
            }
            #endregion

            #region 修改和禁用
            //view修改
            {
                ViewEntity a;
                List<string> labels;
                string label;
                string desc;
                foreach (var view in parentViews)
                {
                    a = views.Find(a => a.Path == view.Path);
                    if (a?.Id > 0)
                    {
                        labels = view.Label?.Split("\r\n")?.ToList();
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
                ViewEntity a;
                ViewEntity pa;
                List<string> labels;
                string label;
                string desc;
                foreach (var view in childViews)
                {
                    a = views.Find(a => a.Path == view.Path);
                    pa = views.Find(a => a.Path == view.ParentPath);
                    if (a?.Id > 0)
                    {
                        labels = view.Label?.Split("\r\n")?.ToList();
                        label = labels != null && labels.Count > 0 ? labels[0] : string.Empty;
                        desc = labels != null && labels.Count > 1 ? string.Join("\r\n", labels.GetRange(1, labels.Count() - 1)) : string.Empty;

                        a.ParentId = pa.Id;
                        a.Label = label;
                        a.Description = desc;
                        a.Enabled = true;
                    }
                }
            }

            //view禁用
            var inputPaths = input.Views.Select(a => a.Path).ToList();
            var disabledViews = (from a in views where !inputPaths.Contains(a.Path) select a).ToList();
            if (disabledViews.Count > 0)
            {
                foreach (var view in disabledViews)
                {
                    view.Enabled = false;
                }
            } 
            #endregion

            //批量更新
            await _viewRepository.UpdateDiy.SetSource(views)
            .UpdateColumns(a => new { a.ParentId, a.Label,a.Description,a.Enabled })
            .ExecuteAffrowsAsync();

            return ResponseOutput.Ok();
        }
    }
}
