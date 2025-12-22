@using ZhonTai.Module.Dev;
@using ZhonTai.DynamicApi.Enums;
@{
    var gen = Model as ZhonTai.Module.Dev.Api.Contracts.Domain.CodeGen.CodeGenEntity;
    if (gen == null) return;
    if (gen.Fields == null) return;
    if (gen.Fields.Count() == 0) return;

    var areaName = "" + gen.ApiAreaName;
    var entityName = "" + gen.EntityName;

    var areaNamePc = areaName.NamingPascalCase();
    var entityNamePc = entityName.NamingPascalCase();

    var areaNameKc = areaName.NamingKebabCase();// KebabCase(areaName);
    var entityNameKc = entityName.NamingKebabCase();// KebabCase(entityName);

    var areaNameCc = areaName.NamingCamelCase();// camelCase(areaName);
    var entityNameCc = entityName.NamingCamelCase();// camelCase(entityName);

    var at = "@";
    var apiName = entityName + "Api";

    var permissionArea = string.Concat(areaNameKc, ":", entityNameKc);

    var queryColumns = gen.Fields.Where(w => w.WhetherQuery);

    var defineUiComponentsImportPath = new Dictionary<string, string>()
    {
        //{"my-select-dictionary","@/components/my-select-window/dictionary" },
        //{"my-role","@/components/my-select-window/role" },
        //{"my-user","@/components/my-select-window/user"},
        //{"my-position","@/components/my-select-window/position" }
        //{"my-upload","@/components/my-upload/index" }
    };
    var uiComponentsMethodName = new Dictionary<string, string>()
    {
        //{"my-select-dictionary","onOpenDic" },
        //{"my-role","onOpenRole" },
        //{"my-user","onOpenUser" },
        //{"my-position","onOpenPosition" }
    };
    var editors = gen.Fields.Select(s => s.Editor).Distinct();
    var uiComponentsInfo = editors
        .Where(w => defineUiComponentsImportPath.Keys.Contains(w))
        .Select(s => new { ImportName = s.NamingPascalCase(), ImportPath = defineUiComponentsImportPath[s] });


        

    // 获取数据输入控件
    string editorName(ZhonTai.Module.Dev.Api.Contracts.Domain.CodeGen.CodeGenFieldEntity col, out string attrs, out string innerBody,out string subfix,out string colWidth)
    {
        attrs = string.Empty;
        subfix = string.Empty;
        innerBody = string.Empty;
        colWidth= new List<string>(){"my-upload","my-editor","my-input-textarea"}.Contains(col.Editor)?"24":"12";
        var editorName = col.Editor;
        if (String.IsNullOrWhiteSpace(editorName)) editorName = "el-input";
        if (!string.IsNullOrWhiteSpace(col.DictTypeCode))
        {
            editorName = "el-select";
            if( col.IsNullable)attrs += " clearable";
            innerBody = string.Concat("<el-option v-for=", "\"item in state.dicts['", col.DictTypeCode, "']\" :key=\"item.value\" :value=\"item.value\" :label=\"item.name\" />");
        }
        else if (col.Editor == "el-date-picker"){
            editorName = "el-date-picker";
            attrs += " value-format=\"YYYY-MM-DD\"";
            if( col.IsNullable)attrs += " clearable";
        }
        else if (col.Editor == "el-select")
        {
            editorName = col.Editor;
            if (col.IsNullable) attrs += " clearable";
            if(!String.IsNullOrWhiteSpace(col.DisplayColumn) || !String.IsNullOrWhiteSpace(col.ValueColumn))
            {
                var labels = (""+col.DisplayColumn).Split(',');
                var values = ("" + col.ValueColumn).Split(',');
                var cout = labels.Length;
                if (values.Length > cout) cout = values.Length;
                for(var i = 0; i < cout; i++)
                {
                    innerBody += string.Concat("<el-option value=\"" + (values.Length > i ? values[i] : "") + "\" label=\"" + (labels.Length > i ? labels[i] : "") + "\" />");
                }
            }
        }
        else if(defineUiComponentsImportPath.Keys.Any(a => a == col.Editor))
        {
            attrs = attrs + " class=\"input-with-select\"";
            innerBody = "<el-button slot=\"append\" icon=\"el-icon-more\" @click=\"" + uiComponentsMethodName[col.Editor] + "('editForm','" + col.DictTypeCode + "','" + col.Title + "')\" />";
        }
        else if (col.Editor == "my-upload")
        {
            editorName = "my-upload";
            attrs += " v-if='state.showDialog'";
        }
        else if (col.Editor == "my-editor")
        {
            editorName = "my-editor";
            attrs += " v-if='state.showDialog'";
        }
        else if (col.Editor == "my-input-textarea"){
            editorName= "el-input";
            attrs += " type=\"textarea\"";
        }
        else if (col.Editor == "my-input-number"){
            editorName= "el-input";
            attrs += " type=\"number\"";
        }
        else if (col.Editor == "my-bussiness-select"){
            editorName= "el-select";
            if (col.IsNullable) attrs += " clearable";
            if (col.IncludeMode == 1){
                attrs += " multiple";
                subfix="_Values";
            }
            if(!String.IsNullOrWhiteSpace(col.IncludeEntity)){
                //业务下拉前缀
                var selectPrefix = col.IncludeEntity.Replace("Entity", "");
                var selectTitle="name";
                if(!String.IsNullOrWhiteSpace(col.IncludeEntityKey))
                    selectTitle=col.IncludeEntityKey.NamingCamelCase();
                if (col.IncludeMode == 1){
                    //一对多,转换模型 xxxIds_Values
                    innerBody = string.Concat("<el-option v-for=", "\"item in state.select",selectPrefix,"ListData\" :key=\"item.id\" :value=\"String(item.id)\" :label=\"item.",selectTitle,"\" />");
                }else{
                    innerBody = string.Concat("<el-option v-for=", "\"item in state.select",selectPrefix,"ListData\" :key=\"item.id\" :value=\"item.id\" :label=\"item.",selectTitle,"\" />");
                }
            }
        }

        return editorName;
    }
    
    var dictCodes = gen.Fields.Where(w => !String.IsNullOrWhiteSpace(w.DictTypeCode)).Select(s => s.DictTypeCode).Distinct();// editors.Any(a => a == "my-select-dictionary");
    var hasDict = dictCodes.Any();
    //关联的模型
    var includeFieldEntitys = gen.Fields.Where(w => !String.IsNullOrWhiteSpace(w.IncludeEntity)).Select(w=>w.IncludeEntity.Replace("Entity", "")).Distinct();
    var hasUpload=editors.Any(a=>a=="my-upload");
    //var hasRole = editors.Any(a => a == "my-role");
    //var hasUser = editors.Any(a => a == "my-user");
    //var hasPosition = editors.Any(a => a == "my-position");

    string jsBool(Boolean exp){
        return exp ? "true" : "false";
    }
}
@{ 
    string attributes, inner, subfix,colWidth;
}
<template>
  <MyLayout>
    <el-card class="my-query-box mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
      <el-form :inline="true" label-width="auto" @(at)submit.stop.prevent>
        @foreach (var col in queryColumns.Where(w=>!w.IsIgnoreColumn()))
        {
            var editor = editorName(col, out attributes, out inner,out subfix,out colWidth);
        @:<el-form-item label="@(col.Title)">
        if(!string.IsNullOrWhiteSpace(inner)){
        @:  <@(editor)@if(!attributes.Contains("clearable"))@(" clearable")@(attributes) v-model="state.filter.@(col.ColumnName.NamingCamelCase())@(subfix)" placeholder="" @(at)keyup.enter="onQuery">
        @:    @(inner)
        @:  </@(editor)>
        }
        else
        {
        @:  <@(editor)@if(!attributes.Contains("clearable"))@(" clearable")@(attributes) v-model="state.filter.@(col.ColumnName.NamingCamelCase())@(subfix)" placeholder="" @(at)keyup.enter="onQuery" />
        }
        @:</el-form-item>
        }
        @if (queryColumns.Count() > 0)
        {
        @:<el-form-item>
        @:  <el-button type="primary" icon="ele-Search" @(at)click="onQuery">查询</el-button>
        @:</el-form-item>
        }
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <div class="my-tools-box mb8 my-flex my-flex-between">
        <div>
          <el-space wrap :size="12">
          @if (gen.GenAdd){
            @:<el-button type="primary" v-auth="perms.add" icon="ele-Plus" @(at)click="onAdd">新增</el-button>
          }
          @if(gen.GenBatchDelete || gen.GenBatchSoftDelete){
            @:<el-dropdown :placement="'bottom-end'" v-if="auths([perms.batSoftDelete, perms.batDelete])">
            @:  <el-button type="warning">批量操作 <el-icon><ele-ArrowDown /></el-icon></el-button>
            @:  <template #dropdown>
            @:    <el-dropdown-menu>
                if(gen.GenBatchSoftDelete){
                  @:  <el-dropdown-item v-if="auth(perms.batSoftDelete)" :disabled="state.sels.length==0" @(at)click="onBatchSoftDelete" icon="ele-DeleteFilled">批量删除</el-dropdown-item>
                  }
                  if(gen.GenBatchDelete){
                  @:  <el-dropdown-item v-if="auth(perms.batDelete)"  :disabled="state.sels.length==0" @(at)click="onBatchDelete" icon="ele-Delete">批量彻底删除</el-dropdown-item>
                  }
            @:    </el-dropdown-menu>
            @:  </template>
            @:</el-dropdown>
          }
          </el-space>
        </div>
        <div></div>
      </div>
      <el-table v-loading="state.loading" :data="state.data" row-key="id" border @(at)selection-change="selsChange">
        @if(gen.GenBatchDelete||gen.GenBatchSoftDelete){
        @:<el-table-column type="selection" width="50" />
        }
            @foreach (var col in gen.Fields.Where(w => w.WhetherTable && !w.IsIgnoreColumn()))
            {
                if(col.IsIncludeColumn()&&!string.IsNullOrWhiteSpace(col.IncludeEntityKey)){
                    if(col.IncludeMode==0){
        @:<el-table-column prop="@(col.ColumnName.NamingCamelCase())_Text" label="@(col.Title)" show-overflow-tooltip width />
                    }else if(col.IncludeMode==1){
                          
        @:<el-table-column prop="@(col.ColumnName.NamingCamelCase())_Texts" label="@(col.Title)" show-overflow-tooltip width >
        @:  <template #default="{ row }">
        @:    {{ row.@(col.ColumnName.NamingCamelCase())_Texts ? row.@(col.ColumnName.NamingCamelCase())_Texts.join(',') : '' }}
        @:  </template>
        @:</el-table-column>
                    }
                }else if(col.Editor=="my-upload"){
        @:<el-table-column prop="@(col.ColumnName.NamingCamelCase())" label="@(col.Title)" show-overflow-tooltip width >
        @:  <template #default="{ row }">
        @:   <div class="my-flex">
        @:     <el-image :src="row.@(col.ColumnName.NamingCamelCase())" :preview-src-list="preview@(col.ColumnName)list"
        @:       :initial-index="get@(col.ColumnName)InitialIndex(row.@(col.ColumnName.NamingCamelCase()))" :lazy="true" :hide-on-click-modal="true" fit="scale-down"
        @:       preview-teleported style="width: 80px; height: 80px" />
        @:     <div class="ml10 my-flex-fill my-flex-y-center">
        @:     </div>
        @:   </div>
        @: </template>
        @:</el-table-column>
                }else{
        @:<el-table-column prop="@(col.ColumnName.NamingCamelCase())@if(!string.IsNullOrWhiteSpace(col.DictTypeCode))@("DictName")" label="@(col.Title)" show-overflow-tooltip width />
                }
            }
        <el-table-column 
          v-auths="[perms.update@(gen.GenSoftDelete?", perms.softDelete":"")@(gen.GenDelete?", perms.delete":"")]"
          label="操作"
          :width="actionColWidth"
          align="center"
          header-align="center"
          fixed="right"
        >
          <template #default="{ row }">
            <el-button v-auth="perms.update" icon="ele-EditPen" text type="primary" @(at)click.stop="onEdit(row)">编辑</el-button>
            @if(gen.GenDelete&&gen.GenSoftDelete){
            @:<el-dropdown v-if="authAll([perms.delete,perms.softDelete])">
            @:  <el-button icon="el-icon--right" text type="danger" >操作 <el-icon class="el-icon--right"><component :is="'ele-ArrowDown'" /></el-icon></el-button>
            @:  <template #dropdown>
            @:    <el-dropdown-menu>
            @:      <el-dropdown-item v-if="auth(perms.softDelete)" @(at)click.stop="onSoftDelete(row)" icon="ele-DeleteFilled">删除</el-dropdown-item>
            @:      <el-dropdown-item v-if="auth(perms.delete)" @(at)click.stop="onDelete(row)" icon="ele-Delete">彻底删除</el-dropdown-item>
            @:    </el-dropdown-menu>
            @:  </template>            
            @:</el-dropdown>
            @:<span v-else style="margin-left:5px;height:inherit">
            @:  <el-button text type="danger" v-if="auth(perms.softDelete)" style="height:inherit" @(at)click.stop="onDelete(row)" icon="ele-DeleteFilled">删除</el-button>
            @:  <el-button text type="danger" v-if="auth(perms.delete)" style="height:inherit" @(at)click.stop="onDelete(row)" icon="ele-Delete">彻底删除</el-button>
            @:</span>
            }
            @if(gen.GenSoftDelete&&!gen.GenDelete){
            @:<el-button text type="danger" v-if="auth(perms.softDelete)" @(at)click.stop="onSoftDelete(row)" icon="ele-DeleteFilled">删除</el-button>
            }
            @if(gen.GenDelete&&!gen.GenSoftDelete){
            @:<el-button text type="danger" v-if="auth(perms.delete)" @(at)click.stop="onDelete(row)" icon="ele-Delete">彻底删除</el-button>
            }
          </template>
        </el-table-column>
      </el-table>

      <div class="my-flex my-flex-end mt10">
        <el-pagination
          v-model:currentPage="state.pageInput.currentPage"
          v-model:page-size="state.pageInput.pageSize"
          :total="state.total"
          :page-sizes="[10, 20, 50, 100]"
          background
          @(at)size-change="onSizeChange"
          @(at)current-change="onCurrentChange"
          layout="total, sizes, prev, pager, next, jumper"
        />
      </div>
    </el-card>

    <@(entityNameKc)-form ref="@(entityNameCc)FormRef" :title="state.@(entityNameCc)FormTitle"></@(entityNameKc)-form>
  </MyLayout>
</template>

<script lang="ts" setup name="@(areaNameCc)/@(entityNameKc)">
import { 
  PageInput@(entityNamePc)GetPageInput,
  @(entityNamePc)GetPageInput,
  @(entityNamePc)GetPageOutput,
@if(gen.GenGetList){
@:  @(entityNamePc)GetListInput, 
@:  @(entityNamePc)GetListOutput,
}
@{
    if (includeFieldEntitys.Any())
    {
        foreach(var incField in includeFieldEntitys)
        {
@:  @(incField)GetListOutput,
@:  @(incField)GetOutput,                    
        }
    }
}
} from '/@(at)/api/@(areaNameKc)/data-contracts'
@if(gen.Fields.Any(s=>s.Editor=="my-upload")){
@:import {  FileGetPageOutput } from '/@(at)/api/admin/data-contracts'
}
import { @(apiName) } from '/@(at)/api/@(areaNameKc)/@(entityNamePc)'
@if (includeFieldEntitys.Any())
{
    foreach(var incField in includeFieldEntitys)
    {
@:import { @(incField)Api } from '/@(at)/api/@(areaNameKc)/@(incField)'
    }
}
@if (hasDict)
{
@:import { DictApi } from '/@(at)/api/admin/Dict'
}
import eventBus from '/@(at)/utils/mitt'
import { auth, auths, authAll } from '/@(at)/utils/authFunction'

// 引入组件
const @(entityNamePc)Form = defineAsyncComponent(() => import('./components/@(entityNameKc)-form.vue'))

const { proxy } = getCurrentInstance() as any

// 表单组件引用
const @(entityNameCc)FormRef = ref()

// 权限标识
const perms = {
  add: 'api:@(permissionArea):add',
  update: 'api:@(permissionArea):update',
  delete: 'api:@(permissionArea):delete',
  batDelete: 'api:@(permissionArea):batch-delete',
  softDelete: 'api:@(permissionArea):soft-delete',
  batSoftDelete: 'api:@(permissionArea):batch-soft-delete',
}

// 操作列宽度
const actionColWidth = authAll([perms.update, perms.softDelete]) || authAll([perms.update, perms.delete]) ? 180 : 120

// 组件状态
const state = reactive({
  loading: false,
  @(entityNameCc)FormTitle: '',
  total: 0,
  sels: [] as Array<@(entityNamePc)GetPageOutput>,
  filter: {
@foreach(var f in queryColumns.Where(w=>!w.IsIgnoreColumn())){
@:    @(f.ColumnName.NamingCamelCase()): null,
}
  } as @(entityNamePc)GetPageInput @if(gen.GenGetList)@("| " + entityNamePc+"GetListInput"),
  pageInput: {
    currentPage: 1,
    pageSize: 20,
  } as PageInput@(entityNamePc)GetPageInput,
  data: [] as Array<@if(gen.GenGetList)@(entityNamePc+"GetListOutput")else@(entityNamePc+"GetPageOutput")>,
  @foreach(var incField in includeFieldEntitys){
@:  select@(incField)ListData: [] as @(incField)GetListOutput[],
}
@foreach (var col in gen.Fields.Where(s=>s.Editor=="my-upload")){
@:  file@(col.ColumnName)ListData: [] as Array<FileGetPageOutput>,
}
@if (hasDict){
  @://字典相关
  @:dicts:{
    foreach (var d in dictCodes)
    {
    @:"@(d)":[],   
    }
  @:}
    }
})

// 组件挂载后初始化数据
onMounted(() => {
@foreach(var incField in includeFieldEntitys){
@:  get@(incField)List();
}
    @if (hasDict)
    {
@:  getDictsTree()      
    }
  onQuery()
  eventBus.off('refresh@(entityNamePc)')
  eventBus.on('refresh@(entityNamePc)', async () => {
    onQuery()
  })
})

// 组件卸载前移除事件监听
onBeforeMount(() => {
  eventBus.off('refresh@(entityNamePc)')
})
@foreach(var incField in includeFieldEntitys){
@:
@:// 获取关联@(incField)列表
@:const get@(incField)List = async () => {
@:  const res = await new @(incField)Api().getList({}).catch(() => {
@:    state.select@(incField)ListData = []
@:  })
@:  state.select@(incField)ListData = res?.data || []
@:}
}
@foreach (var col in gen.Fields.Where(s=>s.Editor=="my-upload")){
@:
@:// 获取@(col.ColumnName)预览列表
@:const preview@(col.ColumnName)list = computed(() => {
@:  let imgList = [] as string[]
@:  state.file@(col.ColumnName)ListData.forEach((a) => {
@:    if (a.linkUrl) {
@:      imgList.push(a.linkUrl as string)
@:    }
@:  })
@:  return imgList
@:})
@:
@:// 获取@(col.ColumnName)初始预览索引
@:const get@(col.ColumnName)InitialIndex = (imgUrl: string) => {
@:  return preview@(col.ColumnName)list.value.indexOf(imgUrl)
@:}
}
@if (hasDict)
{
@:
@:// 获取字典树
@:const getDictsTree = async () => {
@:  let res = await new DictApi().getList(['@(string.Join("','", dictCodes))'])
@:  if(!res?.success)return;
@:    state.dicts = res.data
@:}
}

// 查询
const onQuery = async () => {
  state.loading = true
  state.pageInput.filter = state.filter
  const res = await new @(apiName)().getPage(state.pageInput).catch(() => {
    state.loading = false
  })

  state.data = res?.data?.list ?? []
  state.total = res?.data?.total ?? 0
  state.loading = false
  @foreach (var col in gen.Fields.Where(s=>s.Editor=="my-upload")){
@:  state.file@(col.ColumnName)ListData = res?.data?.list?.map(s => {
@:    return { linkUrl: s.@(col.ColumnName.NamingCamelCase()) }
@:  }) ?? []
}
}
@if(gen.GenAdd){
@:
@:// 新增
@:const onAdd = () => {
@:  state.@(entityNameCc)FormTitle = '新增@(gen.BusName)'
@:  @(entityNameCc)FormRef.value.open()
@:}
}
@if(gen.GenUpdate){
@:
@:// 编辑
@:const onEdit = (row: @(entityNamePc)GetPageOutput) => {
@:  state.@(entityNameCc)FormTitle = '编辑@(gen.BusName)'
@:  @(entityNameCc)FormRef.value.open(row)
@:}
}
@if(gen.GenDelete){
@:
@:// 彻底删除
@:const onDelete = (row: @(entityNamePc)GetPageOutput) => {
@:  proxy.$modal
@:    .confirmDelete(`确定要彻底删除?`)
@:    .then(async () => {
@:      await new @(apiName)().delete({ id: row.id }, { loading: true, showSuccessMessage: true })
@:      onQuery()
@:    })
@:    .catch(() => {})
@:}
}
@if(gen.GenBatchDelete){
@:
@:// 批量彻底删除
@:const onBatchDelete = async () => {
@:  proxy.$modal?.confirmDelete(`确定要彻底删除选择的${state.sels.length}条记录？`).then(async () =>{
@:    const rst = await new @(apiName)().batchDelete(state.sels?.map(item=>item.id) as number[], { loading: true, showSuccessMessage: true })
@:    if(rst?.success){
@:      onQuery()
@:    }
@:  })
@:}
}
@if(gen.GenSoftDelete){
@:
@:// 删除
@:const onSoftDelete = async (row: @(entityNamePc)GetPageOutput) => {
@:  proxy.$modal?.confirmDelete(`确定要删除？`).then(async () =>{
@:    const rst = await new @(apiName)().softDelete({ id: row.id }, { loading: true, showSuccessMessage: true })
@:    if(rst?.success){
@:      onQuery()
@:    }
@:  })
@:}
}
@if(gen.GenBatchSoftDelete){
@:
@:// 批量删除
@:const onBatchSoftDelete = async () => {
@:  proxy.$modal?.confirmDelete(`确定要将选择的${state.sels.length}条记录删除？`).then(async () =>{
@:    const rst = await new @(apiName)().batchSoftDelete(state.sels?.map(item => item.id) as number[], { loading: true, showSuccessMessage: true })
@:    if(rst?.success){
@:      onQuery()
@:    }
@:  })
@:}
}

// 页大小变化
const onSizeChange = (val: number) => {
  state.pageInput.currentPage = 1
  state.pageInput.pageSize = val
  onQuery()
}

// 当前页变化
const onCurrentChange = (val: number) => {
  state.pageInput.currentPage = val
  onQuery()
}

// 选择项变化
const selsChange = (vals: @(entityNamePc)GetPageOutput[]) => {
  state.sels = vals
}
</script>