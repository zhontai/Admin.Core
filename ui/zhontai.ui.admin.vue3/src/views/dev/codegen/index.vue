<template>
  <MyLayout>
    <el-card class="my-query-box mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
      <el-form :inline="true" @submit.stop.prevent>
        <el-form-item label="数据库">
          <el-select v-model="state.filter.dbKey" clearable placeholder="请选择数据库" style="width: 160px">
            <el-option v-for="item in state.dbKeys" :key="item.dbKey" :value="item.dbKey" :label="item.dbKey"></el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="表名">
          <el-input clearable v-model="state.filter.tableName" placeholder="请输入表名" @keyup.enter="getCodeGenData"> </el-input>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Search" @click="getCodeGenData">查询</el-button>
          <el-button type="primary" @click="getTables">查看数据库结构</el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <div class="my-tools-box mb8 my-flex my-flex-between">
        <div>
          <el-space wrap :size="12">
            <el-button v-if="auth(perms.update)" type="primary" icon="ele-Plus" @click="createTable">创建表</el-button>
            <el-dropdown :placement="'bottom-end'">
              <el-button type="primary">
                批量操作 <el-icon class="el-icon--right"><ele-ArrowDown /></el-icon>
              </el-button>
              <template #dropdown>
                <el-dropdown-menu>
                  <el-dropdown-item @click="exportConfig">复制选中配置到剪切板</el-dropdown-item>
                  <el-dropdown-item @click="importConfig">从剪切板导入配置</el-dropdown-item>
                  <el-dropdown-item v-if="auth(perms.batchGen)" @click="batchGenCode">批量生成代码</el-dropdown-item>
                </el-dropdown-menu>
              </template>
            </el-dropdown>
          </el-space>
        </div>
        <div></div>
      </div>
      <el-table
        v-loading="state.loading"
        :data="state.configs"
        highlight-current-row
        ref="listTableRef"
        @row-dblclick="modifyConfig"
        @selection-change="selsChange"
        border
      >
        <el-table-column type="selection" width="50" align="center" header-align="center" />
        <el-table-column prop="tableName" label="表名称" width="200" fixed show-overflow-tooltip></el-table-column>
        <el-table-column prop="entityName" label="实体名" width="180" show-overflow-tooltip></el-table-column>
        <el-table-column prop="namespace" label="命名空间" width="180" show-overflow-tooltip></el-table-column>
        <el-table-column prop="dbKey" label="数据库" width="100" show-overflow-tooltip></el-table-column>
        <el-table-column prop="busName" label="业务名" width="120" show-overflow-tooltip></el-table-column>
        <el-table-column prop="baseEntity" label="基类" width="120" show-overflow-tooltip></el-table-column>
        <el-table-column prop="apiAreaName" label="Api分区" width="100" show-overflow-tooltip></el-table-column>
        <el-table-column prop="backendOut" label="后端输出位置" width="180" show-overflow-tooltip></el-table-column>
        <el-table-column prop="frontendOut" label="前端输出位置" width="180" show-overflow-tooltip></el-table-column>
        <el-table-column prop="dbMigrateSqlOut" label="数据库迁移文件输出位置" width="180" show-overflow-tooltip></el-table-column>
        <el-table-column prop="generateType" label="生成方式" width="100" show-overflow-tooltip>
          <template #default="scope">
            {{ genType(scope.row.generateType) }}
          </template>
        </el-table-column>
        <el-table-column prop="authorName" label="作者" width="100" show-overflow-tooltip></el-table-column>
        <el-table-column prop="comment" label="备注" width="180" show-overflow-tooltip></el-table-column>
        <el-table-column label="操作" width="180" fixed="right" header-align="center" align="center">
          <template #default="scope">
            <el-button icon="ele-EditPen" text type="primary" @click.stop="modifyConfig(scope.row)">编辑</el-button>
            <my-dropdown-more v-if="auths([perms.gen, perms.compile, perms.genCompile, perms.genMenu, perms.delete])" @click.stop>
              <template #dropdown>
                <el-dropdown-menu>
                  <el-dropdown-item v-if="auth(perms.gen)" @click.stop="generate(scope.row)">生成代码</el-dropdown-item>
                  <el-dropdown-item v-if="auth(perms.compile)" @click.stop="compile(scope.row)">生成迁移SQL</el-dropdown-item>
                  <el-dropdown-item v-if="auth(perms.genCompile)" @click.stop="genCompile(scope.row)">执行迁移到数据库</el-dropdown-item>
                  <el-dropdown-item v-if="auth(perms.genMenu)" @click.stop="genMenu(scope.row)">生成菜单数据</el-dropdown-item>
                  <el-dropdown-item v-if="auth(perms.delete)" @click.stop="onDelRow(scope.row)">删除</el-dropdown-item>
                </el-dropdown-menu>
              </template>
            </my-dropdown-more>
          </template>
        </el-table-column>
      </el-table>
    </el-card>
    <codegen-form ref="codegenFormRef" @sure="onConfigEditSure"></codegen-form>
    <el-drawer ref="tablesDrawerRef" v-model="state.dbStructShow" :title="state.filter.dbKey + ' 数据库结构'" direction="rtl">
      <el-tree
        :data="state.dbTree"
        node-key="key"
        :expand-on-click-node="false"
        highlight-current
        :current-node-key="state.filter.dbTree?.key"
        @node-click="tableNodeSelect"
      />
      <template #footer>
        <span style="margin: 8px">
          <el-button @click="state.dbStructShow = false">取消</el-button>
          <el-button
            @click="createConfigFromTable(state.filter.dbTree)"
            type="primary"
            :disabled="!state.filter.dbTree || (state.filter.dbTree != null && state.filter.dbTree.type != 'table')"
          >
            按表结构创建生成配置
          </el-button>
        </span>
      </template>
    </el-drawer>
    <el-dialog
      title="导入确认(双击可编辑直接提交)"
      destroy-on-close
      draggable
      width="80%"
      class="dialog-h50"
      :close-on-click-modal="false"
      v-model="state.importBoxyShow"
    >
      <el-table
        :data="state.importConfigs"
        highlight-current-row
        size="default"
        @selection-change="importSelsChange"
        @row-click="dialogTalbeToggleSelection"
        @row-dblclick="modifyConfig"
        ref="importDialogTableRef"
      >
        <el-table-column type="selection" width="50" />
        <el-table-column prop="importStatus" label="导入状态" width="120" fixed show-overflow-tooltip></el-table-column>
        <el-table-column prop="tableName" label="表名" width="200"></el-table-column>
        <el-table-column prop="entityName" label="实体名" width="180"></el-table-column>
        <el-table-column prop="dbKey" label="数据库" width="100"></el-table-column>
        <el-table-column prop="namespace" label="命名空间" width="180"></el-table-column>
        <el-table-column prop="busName" label="业务名" width="120"></el-table-column>
        <el-table-column prop="baseEntity" label="基类" width="120"></el-table-column>
        <el-table-column prop="apiAreaName" label="Api分区" width="100"></el-table-column>
        <el-table-column prop="generateType" label="生成方式" width="100">
          <template #default="scope">
            {{ genType(scope.row.generateType) }}
          </template>
        </el-table-column>
        <el-table-column prop="authorName" label="作者" width="100"></el-table-column>
        <el-table-column prop="comment" label="备注" width></el-table-column>
      </el-table>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="onCancelImport"> 取消 </el-button>
          <el-button type="primary" @click="onCompleteImport" v-if="state.importSuccess">导入完成</el-button>
          <el-button type="primary" @click="onSureImport" v-else> 确定</el-button>
        </span>
      </template>
    </el-dialog>
  </MyLayout>
</template>

<script lang="ts" setup name="dev/codegen">
import { BaseDataGetOutput, DatabaseGetOutput, CodeGenGetOutput, CodeGenUpdateInput, CodeGenFieldGetOutput } from '/@/api/dev/data-contracts'
import { CodeGenApi } from '/@/api/dev/CodeGen'
import eventBus from '/@/utils/mitt'
import useClipboard from 'vue-clipboard3'
import { auth, auths, authAll } from '/@/utils/authFunction'

const { toClipboard } = useClipboard()
// 引入组件
const CodegenForm = defineAsyncComponent(() => import('./components/codegen-form.vue'))

const { proxy } = getCurrentInstance() as any

const listTableRef = ref()
const importDialogTableRef = ref()

interface DbTree {
  key: string | null | undefined
  label: string | null | undefined
  type: string | null | undefined
  children?: DbTree[] | null
}
const codegenFormRef = ref()

//权限配置
const perms = {
  update: 'api:dev:code-gen:update',
  delete: 'api:dev:code-gen:delete',
  gen: 'api:dev:code-gen:generate',
  batchGen: 'api:dev:code-gen:batch-generate',
  compile: 'api:dev:code-gen:compile',
  genCompile: 'api:dev:code-gen:gen-compile',
  genMenu: 'api:dev:code-gen:gen-menu',
}

const state = reactive({
  filter: {
    dbKey: '',
    tableName: '',
    dbType: '',
    config: {} as CodeGenGetOutput | null,
    dbTree: {} as DbTree,
  },
  // 基础数据定义

  defaultOption: {} as BaseDataGetOutput | any,
  // 界面显示用数据
  dbKeys: [] as Array<DatabaseGetOutput>,

  configs: [] as Array<CodeGenGetOutput>,

  dbTree: [] as Array<DbTree>,
  // 状态器定义
  loading: false,
  dbStructShow: false,
  sels: [] as Array<CodeGenGetOutput>,
  importConfigs: [] as Array<any>,
  importSels: [] as Array<any>,
  importBoxyShow: false,
  importSuccess: false as Boolean,
})

// 初始化数据
const genDefaultConfig = (): CodeGenUpdateInput => {
  return {
    id: 0,
    authorName: state.defaultOption?.authorName ?? 'SirHQ',
    tableName: '',
    dbKey: state.filter.dbKey,
    entityName: '',
    comment: '',
    generateType: '1',
    apiAreaName: state.defaultOption?.apiAreaName ?? 'admin',
    namespace: state.defaultOption?.namespace ?? 'SirHQ.CodeGen.Apps',
    menuAfterText: state.defaultOption?.menuAfterText ?? '',
    busName: '',
    baseEntity: 'EntityBase',
    backendOut: state.defaultOption?.backendOut ?? '',
    frontendOut: state.defaultOption?.frontendOut ?? '',
    dbMigrateSqlOut: state.defaultOption?.dbMigrateSqlOut ?? '',

    genEntity: true,
    genRepository: true,
    genService: true,

    genGet: true,
    genGetPage: true,
    genAdd: true,
    genUpdate: true,
    genSoftDelete: true,

    fields: [],
  }
}

// 获取基础数据
const getBaseData = async () => {
  const res = await new CodeGenApi().getBaseData()
  state.dbKeys = res?.data?.databases ?? []
  delete res?.data?.databases
  state.defaultOption = res?.data
}

// 获取数据库结构
const getTables = async () => {
  if (!state.filter.dbKey) {
    proxy.$modal.msgWarning('请选择数据库')
    return
  }
  const res = await new CodeGenApi().getTables({ dbkey: state.filter.dbKey }, { loading: true })
  state.dbStructShow = true

  let dbTree: DbTree[] = []

  res?.data?.forEach((config: CodeGenGetOutput) => {
    let tree: DbTree = Object.assign(
      {
        key: config.tableName,
        label: config.tableName + ' ' + config.comment,
        type: 'table',
        children: [],
      },
      config
    )

    config.fields?.forEach((col: CodeGenFieldGetOutput) => {
      tree.children?.push({
        key: col.columnName,
        label: col.columnName + (' [' + col.dataType + (col.length != -1 ? '(' + col.length + ')' : '') + ']'),
        type: 'col',
      })
    })

    dbTree.push(tree)
  })
  state.dbTree = dbTree
}

// 获取代码生成数据
const getCodeGenData = async () => {
  state.loading = true
  const res = await new CodeGenApi().getList({ dbkey: state.filter.dbKey, tableName: state.filter.tableName }).catch(() => {})
  state.configs = res?.data ?? []
  state.loading = false
}

// 显示编辑数据
const showEditData = async (data: CodeGenGetOutput & any) => {
  if (data.importStatus == '导入成功') {
    proxy.$modal.msgWarning('导入成功后不能再编辑')
    return
  }
  codegenFormRef?.value?.open(data)
}

// 创建表
const createTable = () => {
  showEditData(genDefaultConfig())
}

// 删除数据
const onDelRow = async (config: CodeGenGetOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除【${config.busName}】?`)
    .then(async () => {
      var res = await new CodeGenApi().delete({ id: config.id }, { loading: true, showSuccessMessage: true })
      if (res?.success) {
        getCodeGenData()
      }
    })
    .catch(() => {})
}

// 修改配置
const modifyConfig = async (config: CodeGenGetOutput) => {
  if (!config) {
    return
  }
  showEditData(JSON.parse(JSON.stringify(config)))
}

const dialogTalbeToggleSelection = async (row: CodeGenGetOutput) => {
  importDialogTableRef.value!.toggleRowSelection(row)
}

// 生成代码
const generate = async (config: CodeGenGetOutput) => {
  if (!config) {
    return
  }

  await new CodeGenApi().generate({ id: config.id }, { loading: true, showSuccessMessage: true })
}

// 编译
const compile = async (config: CodeGenGetOutput) => {
  if (!config) {
    return
  }

  const res = await new CodeGenApi().compile({ id: config.id }, { loading: true, showSuccessMessage: false })
  console.log(res.data)
  if (res.data) {
    toClipboard(res.data)
    proxy.$modal.msgWarning('已将sql复制到剪切板&' + config.backendOut)
  } else {
    proxy.$modal.msgWarning('无迁移SQL')
  }
}

// 获得编译
const genCompile = async (config: CodeGenGetOutput) => {
  if (!config) {
    return
  }

  await new CodeGenApi().genCompile({ id: config.id }, { loading: true, showSuccessMessage: true })
}

// 生成菜单
const genMenu = async (config: CodeGenGetOutput) => {
  if (!config) return
  await new CodeGenApi().genMenu({ id: config.id }, { loading: true, showSuccessMessage: true })
}

// 确定
const onConfigEditSure = async (data: any) => {
  if (!data?.data) return

  let editorForm = data.data as CodeGenUpdateInput
  if (!editorForm.fields) {
    editorForm.fields = []
  }
  // for (let idx in editorForm.columns) {
  //   editorForm.columns[idx].dbKey = state.filter.dbKey
  // }
  const res = await new CodeGenApi().update(editorForm, { loading: true, showSuccessMessage: true })
  if (!res?.success) {
    return res
  }
  if (data.data.importStatus == '等待导入') {
    // console.log(data.data.importKey)
    // console.log(state.importConfigs)
    let selectData = state.importConfigs.find((s) => s.importKey == data.data.importKey)
    // console.log(selectData)
    if (selectData) {
      selectData.importStatus = '导入成功'
    }
    let callback = data.callback as Function
    callback(res)
    return
  }
  // state.filter.config = JSON.parse(JSON.stringify(editorForm))

  let callback = data.callback as Function
  callback(res)
  getCodeGenData()
}

const tableNodeSelect = async (obj: DbTree, node: any, prop: any, event: any) => {
  state.filter.dbTree = obj
}

const createConfigFromTable = async (table: DbTree) => {
  if (!table) return
  state.dbStructShow = false
  var newConfig = Object.assign(JSON.parse(JSON.stringify(table)), state.defaultOption)
  var baseFields = [
    'CreatedUserId',
    'CreatedUserName',
    'CreatedUserRealName',
    'CreatedTime',
    'ModifiedUserId',
    'ModifiedUserName',
    'ModifiedUserRealName',
    'ModifiedTime',
    'IsDeleted',
    'Id',
  ]

  newConfig.fields = newConfig.fields.filter((item: any) => {
    return baseFields.indexOf(item.columnName) < 0
  })

  newConfig.fields.forEach((f: CodeGenFieldGetOutput) => {
    f.columnRawName = f.columnName
  })
  // newConfig.fields.reverse()
  newConfig.baseEntity = 'EntityBase'
  newConfig.generateType = '2'
  newConfig.entityName = ''
  showEditData(newConfig)
}

const genType = (t: number) => {
  if (t == 1) return 'CodeFirst'
  if (t == 2) return 'DbFirst'
  return t
}

onMounted(() => {
  getBaseData()
  getCodeGenData()
  eventBus.on('onConfigEditSure', async (config: CodeGenUpdateInput) => {
    onConfigEditSure(config)
  })
})

onUnmounted(() => {
  eventBus.off('onConfigEditSure')
})

const selsChange = (vals: CodeGenGetOutput[]) => {
  console.log(vals)
  state.sels = vals
}

const batchGenCode = async () => {
  if (state.sels.length == 0) return proxy.$modal.msgWarning('请选择要生成的表')
  await new CodeGenApi().batchGenerate(state.sels.map((s) => s.id) as number[], { loading: true, showSuccessMessage: true })
}

const exportConfig = () => {
  if (state.sels.length == 0) return proxy.$modal.msgWarning('请选择要复制的配置')
  // console.log(JSON.stringify(state.sels))
  toClipboard(JSON.stringify(state.sels))
  return proxy.$modal.msgSuccess('已经配置复制到剪切板，保存成json即可')
}

const importConfig = () => {
  state.importSuccess = false
  navigator.clipboard
    .readText()
    .then((v) => {
      // console.log("获取剪贴板成功：", v);
      let list = JSON.parse(v) as Array<any>
      state.importBoxyShow = true
      let k = 1
      state.importConfigs = list.map((s) => {
        //导入key
        s.importKey = 'import_' + (k++).toString()
        //导入消息
        s.importStatus = '等待导入'
        s.id = 0
        s.fields?.forEach((s2: CodeGenFieldGetOutput) => {
          s2.id = 0
        })
        return s
      })
    })
    .catch((v) => {
      proxy.$modal.msgWarning('请先复制正确的配置')
    })
}

const importSelsChange = (vals: CodeGenGetOutput[]) => {
  state.importSels = vals
}

const onSureImport = () => {
  if (state.importSels.length == 0) return proxy.$modal.msgWarning('选择要导入的数据')
  state.importSels.forEach(async (data) => {
    if (data.importStatus == '导入成功' || data.importStatus == '导入中') return
    let editorForm = data as CodeGenUpdateInput
    // editorForm.id = 0
    if (!editorForm.fields) {
      editorForm.fields = []
    }
    // editorForm.fields.forEach(s => {
    //   s.id = 0
    // })
    data.importStatus = '导入中'
    const res = await new CodeGenApi().update(editorForm, { loading: true, showErrorMessage: false }).catch((e) => {
      data.importStatus = '导入失败:' + e.message
    })
    if (res?.success) {
      data.importStatus = '导入成功'
    }
    state.importSuccess = state.importConfigs.filter((s2) => s2.importStatus === '导入成功').length == state.importConfigs.length
  })
  state.importSuccess = state.importConfigs.filter((s2) => s2.importStatus === '导入成功').length == state.importConfigs.length
}

const onCancelImport = () => {
  state.importBoxyShow = false
}

const onCompleteImport = () => {
  onCancelImport()
  getCodeGenData()
}
</script>
