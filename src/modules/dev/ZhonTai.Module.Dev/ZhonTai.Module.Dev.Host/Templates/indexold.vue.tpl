@using ZhonTai.Module.Dev;
@using ZhonTai.DynamicApi.Enums;
@{
    var gen = Model as ZhonTai.Module.Dev.Domain.CodeGen.CodeGenEntity;
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
    string editorName(ZhonTai.Module.Dev.Domain.CodeGen.CodeGenFieldEntity col, out string attrs, out string innerBody)
    {
        attrs = string.Empty;
        innerBody = string.Empty;
        var editorName = col.Editor;
        if (String.IsNullOrWhiteSpace(editorName)) editorName = "el-input";
        if (!string.IsNullOrWhiteSpace(col.DictTypeCode))
        {
            editorName = "el-select";
            if( col.IsNullable)attrs += " clearable ";
            innerBody = string.Concat("<el-option v-for=", "\"item in dicts['", col.DictTypeCode, "']\" :key=\"item.code\" :value=\"item.code\" :label=\"item.name\" />");
        }
        else if (col.Editor == "el-select")
        {
            editorName = col.Editor;
            if (col.IsNullable) attrs += " clearable ";
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
        else if (col.Editor == "my-upload")
        {
            editorName = "my-upload";
            attrs += " v-if='state.formShow' ";
        }
        else if(defineUiComponentsImportPath.Keys.Any(a => a == col.Editor))
        {
            attrs = attrs + " class=\"input-with-select\" ";
            innerBody = "<el-button slot=\"append\" icon=\"el-icon-more\" @click=\"" + uiComponentsMethodName[col.Editor] + "('editForm','" + col.DictTypeCode + "','" + col.Title + "')\" />";
        }

        return editorName;
    }

    var dictCodes = gen.Fields.Where(w => "dict" == w.EffectType).Select(s => s.DictTypeCode);// editors.Any(a => a == "my-select-dictionary");
    var hasDict = dictCodes.Any();
    var includeFields = gen.Fields.Where(w => !String.IsNullOrWhiteSpace(w.IncludeEntity));
    var hasUpload=editors.Any(a=>a=="my-upload");
    //var hasRole = editors.Any(a => a == "my-role");
    //var hasUser = editors.Any(a => a == "my-user");
    //var hasPosition = editors.Any(a => a == "my-position");

    string jsBool(Boolean exp){
        return exp ? "true" : "false";
    }
}
@{ 
    string attributes, inner;
}
<template>
  <div style="padding: 0px 8px">
    <!--查询-->
    <el-card shadow="never" :body-style="{ paddingBottom: '0' }" style="margin-top: 8px">
      <el-form inline :model="state.filterModel">
                @foreach (var col in queryColumns.Where(w=>!w.IsIgnoreColumn()))
                {
                    var editor = editorName(col, out attributes, out inner);
        @:<el-form-item>
        @:  <@(editor) @if(!attributes.Contains("clearable"))@("clearable") @(attributes) v-model="state.filterModel.@(col.ColumnName.NamingCamelCase())" placeholder="@(col.Title)" @(at)keyup.enter="onQuery" >
        if(!string.IsNullOrWhiteSpace(inner)){
        @:    @(inner)
        }
        @:  </@(editor)>
        @:</el-form-item>
                }
                @if (queryColumns.Count() > 0)
                {
        @:<el-form-item>
        @:  <el-button type="primary" icon="ele-Search" @(at)click="onQuery">查询</el-button>
        @:</el-form-item>
                }
        <el-form-item v-auth="perms.add">
          <el-button type="primary" icon="ele-Plus" @(at)click="onAdd">新增</el-button>
        </el-form-item>
        @if(gen.GenBatchDelete || gen.GenBatchSoftDelete){
        @:<el-form-item v-auths="[perms.batDelete, perms.batSoftDelete]" >
        if(gen.GenBatchSoftDelete){
        @:  <el-button v-auth="perms.batSoftDelete" type="warning" :disabled="state.sels.length==0" :placement="'bottom-end'" @(at)click="onBatchSoftDelete" icon="ele-DeleteFilled">批量删除</el-button>
        }
        if(gen.GenBatchDelete){
        @:  <el-button v-auth="perms.batDelete" type="danger" :disabled="state.sels.length==0" :placement="'bottom-end'" @(at)click="onBatchDelete" icon="ele-Delete">批量删除</el-button>
        }
        @:</el-form-item>
        }
       </el-form>
     </el-card>

    <!--列表-->
    <el-card shadow="never" style="margin-top: 8px">
      <el-table size="small" v-loading="state.listLoading" :data="state.@(entityNameCc)s" row-key="id" @(at)selection-change="selsChange" >
        <template #empty>
          <el-empty :image-size="100" />
        </template>
        @if(gen.GenDelete||gen.GenSoftDelete){
        @:<el-table-column type="selection" width="50" />
        }
            @foreach (var col in gen.Fields.Where(w => w.WhetherTable && !w.IsIgnoreColumn()))
            {
        @:<el-table-column prop="@(col.ColumnName.NamingCamelCase())@if(!string.IsNullOrWhiteSpace(col.DictTypeCode))@("DictName")" label="@(col.Title)" show-overflow-tooltip width />
            }
        <el-table-column v-auths="[perms.update,perms.softDelete,perms.delete]" label="操作" :width="actionColWidth" fixed="right">
          <template #default="{ row }">
            <el-button v-auth="perms.update" icon="ele-EditPen" size="small" text type="primary" @(at)click="onEdit(row)">编辑</el-button>
            @if(gen.GenDelete&&gen.GenSoftDelete){
            @:<el-dropdown v-if="authAll([perms.delete,perms.softDelete])" style="margin:5px 0 0 5px;">
            @:  <span><el-icon class="el-icon--right"><component :is="'ele-ArrowDown'" /></el-icon></span>
            @:  <template #dropdown>
            @:    <el-dropdown-menu>
            @:      <el-dropdown-item v-if="auth(perms.delete)" @(at)click="onDelete(row)" icon="ele-Delete">删除</el-dropdown-item>
            @:      <el-dropdown-item v-if="auth(perms.softDelete)" @(at)click="onSoftDelete(row)" icon="ele-DeleteFilled">软删除</el-dropdown-item>
            @:    </el-dropdown-menu>
            @:  </template>            
            @:</el-dropdown>
            @:<span v-else style="margin-left:5px;height:inherit">
            @:  <el-button text type="warning" v-if="auth(perms.softDelete)" style="height:inherit" @(at)click="onDelete(row)" icon="ele-DeleteFilled">软删除</el-button>
            @:  <el-button text type="danger" v-if="auth(perms.delete)" style="height:inherit" @(at)click="onDelete(row)" icon="ele-Delete">删除</el-button>
            @:</span>
            }
            @if(gen.GenSoftDelete&&!gen.GenDelete){
            @:<el-button text type="warning" v-if="perms.softDelete" @(at)click="onSoftDelete(row)" icon="ele-DeleteFilled">删除</el-button>
            }
            @if(gen.GenDelete&&!gen.GenSoftDelete){
            @:<el-button text type="danger" v-auth="perms.delete" @(at)click="onDelete(row)" icon="ele-Delete">删除</el-button>
            }
          </template>
        </el-table-column>
      </el-table>

      <!--分页-->
      <div class="my-flex my-flex-end" style="margin-top: 20px">
        <el-pagination ref="pager" small background :total="state.total" :page-sizes="[10, 20, 50, 100]"
           v-model:currentPage="state.pageInput.currentPage"
           v-model:page-size="state.pageInput.pageSize"
           @(at)size-change="onSizeChange" @(at)current-change="onCurrentChange"
           layout="total, sizes, prev, pager, next, jumper"/>
      </div>
    </el-card>

    
    <el-drawer direction="rtl" v-model="state.formShow" :title="state.formTitle">
      <el-form :model="state.formData" label-width="100" style="margin:8px;"
        :rules="state.editMode=='add'?addRules:updateRules" ref="dataEditor">
      @foreach(var col in gen.Fields.Where(w=>!w.IsIgnoreColumn() && ( w.WhetherAdd || w.WhetherUpdate )))
      {
        var editor = editorName(col, out attributes, out inner);
        @:<el-form-item label="@(col.Title)" prop="@(col.ColumnName.NamingCamelCase())" v-show="editItemIsShow(@jsBool(col.WhetherAdd), @jsBool(col.WhetherUpdate))">
        @:  <@(editor) @(attributes) v-model="state.formData.@(col.ColumnName.NamingCamelCase())" placeholder="@(col.Comment)" >
        if(!string.IsNullOrWhiteSpace(inner)){
        @:    @(inner)
        }
        @:  </@(editor)>
        @:</el-form-item>
      }
      </el-form>
      <template #footer>
        <el-card>
          <el-button @(at)click="state.formShow = false">取消</el-button>
          <el-button type="primary" @(at)click="submitData(state.formData)">确定</el-button>
        </el-card>
      </template>
    </el-drawer>
  </div>
</template>

<script lang="ts" setup>
import { ref, reactive, onMounted, getCurrentInstance, onUnmounted, defineAsyncComponent } from 'vue'
import { FormRules } from 'element-plus'
import { PageInput@(entityNamePc)GetPageInput, @(entityNamePc)GetPageInput, @(entityNamePc)GetPageOutput, @(entityNamePc)GetOutput, @(entityNamePc)AddInput, @(entityNamePc)UpdateInput,
@if(gen.GenGetList){
@:  @(entityNamePc)GetListInput, @(entityNamePc)GetListOutput,
}
@{
    if (includeFields.Any())
    {
        foreach(var incField in includeFields)
        {
            if (incField.IncludeMode == 1)
            {
@:  @(incField.IncludeEntity.Replace("Entity", ""))GetListOutput,
            }
            else
            {
@:  @(incField.IncludeEntity.Replace("Entity", ""))GetOutput,                    
            }
        }
    }
}
} from '/@(at)/api/@(areaNameKc)/data-contracts'
import { @(apiName) } from '/@(at)/api/@(areaNameKc)/@(entityNamePc)'
@if (includeFields.Any())
{
    foreach(var incField in includeFields)
    {
        var incEntityName = incField.IncludeEntity.Replace("Entity", "");
@:import { @(incEntityName)Api } from '/@(at)/api/@(areaNameKc)/@(incEntityName)'
    }
}
import eventBus from '/@(at)/utils/mitt'
import { auth, auths, authAll } from '/@(at)/utils/authFunction'

    @if (hasDict)
    {
@:import dictTreeApi from '@(at)/api/admin/dictionary-tree'        
    }
    @foreach (var comp in uiComponentsInfo)
    {
@:import @(comp.ImportName) from '@(comp.ImportPath)'
    }
    @if (hasUpload)
    {
@:const MyUpload = defineAsyncComponent(() => import('/@(at)/components/my-upload/index.vue'))      
    }

const { proxy } = getCurrentInstance() as any

const dataEditor = ref()

const perms = {
  add:'api:@(permissionArea):add',
  update:'api:@(permissionArea):update',
  delete:'api:@(permissionArea):delete',
  batDelete:'api:@(permissionArea):batch-delete',
  softDelete:'api:@(permissionArea):soft-delete',
  batSoftDelete:'api:@(permissionArea):batch-soft-delete',
}

const actionColWidth = authAll([perms.delete,perms.softDelete])?125:auths([perms.delete,perms.softDelete])?135:70;

const formRules = reactive<FormRules>({
@foreach (var f in gen.Fields.Where(w=>!w.IsIgnoreColumn() && ( w.WhetherAdd || w.WhetherUpdate ) && !w.IsNullable))
{
@:  @(f.ColumnName.NamingCamelCase()):[{ required: true, message: '@(f.Title)不能为空！', trigger: '@(f.FrontendRuleTrigger)'}],
}
})

const addRules = {
@foreach (var f in gen.Fields.Where(w=>!w.IsIgnoreColumn() && w.WhetherAdd && !w.IsNullable))
{
@:  @(f.ColumnName.NamingCamelCase()): formRules.@(f.ColumnName.NamingCamelCase()),
}
}
const updateRules = {
@foreach (var f in gen.Fields.Where(w=>!w.IsIgnoreColumn() && w.WhetherUpdate && !w.IsNullable))
{
@:  @(f.ColumnName.NamingCamelCase()): formRules.@(f.ColumnName.NamingCamelCase()),
}
}

const state = reactive({
  listLoading: false,
  formTitle: '',
  editMode: 'add',
  formShow: false,
  formData: {} as @(entityNamePc)AddInput | @(entityNamePc)UpdateInput,
  sels: [] as Array<@(entityNamePc)GetPageOutput>,
  filterModel: {
@foreach(var f in queryColumns.Where(w=>!w.IsIgnoreColumn())){
@:    @(f.ColumnName.NamingCamelCase()): null,
}
  } as @(entityNamePc)GetPageInput @if(gen.GenGetList)@("| " + entityNamePc+"GetListInput"),
  total:0,
  pageInput:{
    currentPage: 1,
    pageSize: 20,
  } as PageInput@(entityNamePc)GetPageInput,
  @(entityNameCc)s: [] as Array<@(entityNamePc)GetPageOutput>,
  @if(gen.GenGetList){
  @:@(entityNameCc)List: [] as Array<@(entityNamePc)GetListOutput>,
  }
      @if (hasDict)
    {
  @://字典相关
  @:dictSelectorTitle: '字典内容选择',
  @:dictType: '',
  @:dictForm: null,
  @:dictVisible: false,
  @:dicts:{
    foreach (var d in dictCodes)
    {
    @:"@(d)":[],   
    }
  @:}
    }
})

const editItemIsShow = (add: Boolean, edit: Boolean): Boolean => {
    if(add && edit)return true;
    if(add && state.editMode == 'add')return true;
    if(edit && state.editMode == 'edit')return true;
    return false;
}

onMounted(()=>{
  onQuery()
    @if (hasDict)
    {
@:  getDictsTree()      
    }
})

onUnmounted(()=>{

})

@if (hasDict)
{
@://获取需要使用的字典树
@:const getDictsTree = async () {
@:  let me = this;
@:  let res = await dictTreeApi.get({codes:'@(string.Join(',', dictCodes))'})
@:  if(!res?.success)return;
@:  for(var i in res.data){
@:    let item = res.data[i]
@:    var key = item.code
@:    var values = item.childrens
@:    me.dicts[key]= values
@:  }
@:}
}

const showEditor = () => {
  state.formShow = true
  dataEditor?.value?.resetFields()
}

const defaultToAdd = (): @(entityNamePc)AddInput => {
  return {
@foreach(var col in gen.Fields.Where(w=>!w.IsIgnoreColumn())){
@:    @(col.ColumnName.NamingCamelCase()): @(col.GetDefaultValueStringScript()),
}
  } as @(entityNamePc)AddInput
}

const onQuery = async () => {
  state.listLoading = true
  
  var queryParams = state.pageInput;
  queryParams.filter = state.filterModel;
  queryParams.dynamicFilter = {};

  const res = await new @(apiName)().getPage(queryParams)

  state.@(entityNameCc)s = res?.data?.list ?? []
  state.total = res?.data?.total ?? 0
  state.listLoading = false
}
const onSizeChange = (val: number) => {
  state.pageInput.pageSize = val
  onQuery()
}

const onCurrentChange = (val: number) => {
  state.pageInput.currentPage = val
  onQuery()
}

const selsChange = (vals: @(entityNamePc)GetPageOutput[]) => {
  state.sels = vals
}

const onAdd = () => {
  state.editMode = 'add'
  state.formTitle = '新增@(gen.BusName)'
  state.formData = defaultToAdd()
  showEditor()
}

const onEdit = async (row: @(entityNamePc)GetOutput) => {
  state.editMode = 'edit'
  state.formTitle = '编辑@(gen.BusName)'
  const res = await new @(apiName)().get({id: row.id}, { loading: true})
  if (res?.success) {
    showEditor()
    state.formData = res.data as @(entityNamePc)UpdateInput
  }
}

const onDelete = async (row: @(entityNamePc)GetOutput) => {
  proxy.$modal?.confirmDelete(`确定要删除？`).then(async () =>{
      const rst = await new @(apiName)().delete({ id: row.id }, { loading: true, showSuccessMessage: true })
      if(rst?.success){
        onQuery()
      }
    })
}

const onAddPost = async (addData: @(entityNamePc)AddInput) => {
  const res = await new @(apiName)().add(addData, { loading: true, showSuccessMessage: true })
  if (res?.success) {
    onQuery()
    state.formShow = false
  }
}

const onUpdatePost = async (updateData: @(entityNamePc)UpdateInput) => {
  const res = await new @(apiName)().update(updateData, { loading: true, showSuccessMessage: true })
  if (res?.success) {
    onQuery()
    state.formShow = false
  }
}

const submitData = async (editData: @(entityNamePc)AddInput | @(entityNamePc)UpdateInput) => {
  dataEditor?.value?.validate(async (valid: boolean) => {
    if (!valid) return
    
    if (state.editMode == 'add') {
      onAddPost(editData)
    } else if (state.editMode == 'edit') {
      onUpdatePost(editData)
    }
  })
}

@if(gen.GenBatchDelete){
@:const onBatchDelete = async () => {
@:  proxy.$modal?.confirmDelete(`确定要删除选择的${state.sels.length}条记录？`).then(async () =>{
@:    const rst = await new @(apiName)().batchDelete(state.sels?.map(item=>item.id) as number[], { loading: true, showSuccessMessage: true })
@:    if(rst?.success){
@:      onQuery()
@:    }
@:  })
@:}
}

@if(gen.GenSoftDelete){
@:const onSoftDelete = async (row: @(entityNamePc)GetOutput) => {
@:  proxy.$modal?.confirmDelete(`确定要移入回收站？`).then(async () =>{
@:    const rst = await new @(apiName)().softDelete({ id: row.id }, { loading: true, showSuccessMessage: true })
@:    if(rst?.success){
@:      onQuery()
@:    }
@:  })
@:}
}

@if(gen.GenBatchSoftDelete){
@:const onBatchSoftDelete = async () => {
@:  proxy.$modal?.confirmDelete(`确定要将选择的${state.sels.length}条记录移入回收站？`).then(async () =>{
@:    const rst = await new @(apiName)().batchSoftDelete(state.sels?.map(item => item.id) as number[], { loading: true, showSuccessMessage: true })
@:    if(rst?.success){
@:      onQuery()
@:    }
@:  })
@:}
}
</script>

<script lang="ts">
import { defineComponent } from 'vue'
export default defineComponent({
  name: '@(areaNameCc)/@(entityNameCc)'
})
</script>